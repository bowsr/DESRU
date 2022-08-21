using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil {
    public partial class MainWindow: Form {
        private readonly Color FORM_BACKCOLOR = Color.FromArgb(45, 45, 45);
        private readonly Color TEXT_BACKCOLOR = Color.FromArgb(60, 60, 60);

        Process _gameProcess;
        public bool Hooked = false;

        bool _fwRuleExists = false;
        bool _fwRestart = false;

        bool _mhExists = false, _mhScheduleRemoval = false, _mhDoRemovalTask = false;
        bool _mhRestart = false;

        FreescrollMacro _macroProcess;
        bool _enableMacro = true;

        HotkeyHandler _hotkeys;
        int _fps0, _fps1, _fps2;

        MemoryHandler _memory;

        Timer _formTimer;

        bool _hkAssignmentMode = false, _mouse1Pressed = false;
        Label _selectedHKField = null;

        Keys[] _invalidKeys = { Keys.Oemtilde, Keys.LButton, Keys.RButton };

        List<Label> _hotkeyFields;

        string _gameDirectory = "", _steamDirectory = "", _libraryVDFLocation = "";
        List<string> _gameVersions;

        public MainWindow() {
            InitializeComponent();
            SearchForSteamGameDir();
            _fwRuleExists = FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", false);
            firewallToggleButton.Text = _fwRuleExists ? "Remove Firewall Rule" : "Create Firewall Rule";
            _mhExists = File.Exists(_gameDirectory + "\\XINPUT1_3.dll");
            meathookToggleButton.Text = _mhExists ? "Disable meath00k" : "Enable meath00k";

            _hotkeyFields = new();
            _hotkeyFields.Add(hotkeyField0);
            _hotkeyFields.Add(hotkeyField1);
            _hotkeyFields.Add(hotkeyField2);
            _hotkeyFields.Add(hotkeyField3);
            _hotkeyFields.Add(hotkeyField4);

            _formTimer = new Timer();
            _formTimer.Interval = 8;
            _formTimer.Tick += new EventHandler(UpdateTick);

            this.KeyDown += new KeyEventHandler(HotkeyAssignment_KeyDown);
            this.MouseDown += new MouseEventHandler(HotkeyAssignment_MouseDown);
            this.FormClosing += new FormClosingEventHandler(MainWindow_Closing);
            this.Load += new EventHandler(MainWindow_Load);

            fpsInput0.KeyPress += new KeyPressEventHandler(FPSInput_KeyPressNumericOnly);
            fpsInput1.KeyPress += new KeyPressEventHandler(FPSInput_KeyPressNumericOnly);
            fpsInput2.KeyPress += new KeyPressEventHandler(FPSInput_KeyPressNumericOnly);
            fpsInput0.KeyUp += new KeyEventHandler(FPSInput_KeyUp);
            fpsInput1.KeyUp += new KeyEventHandler(FPSInput_KeyUp);
            fpsInput2.KeyUp += new KeyEventHandler(FPSInput_KeyUp);

            hotkeyField0.Click += new EventHandler(HotkeyAssignment_FieldSelected);
            hotkeyField1.Click += new EventHandler(HotkeyAssignment_FieldSelected);
            hotkeyField2.Click += new EventHandler(HotkeyAssignment_FieldSelected);
            hotkeyField3.Click += new EventHandler(HotkeyAssignment_FieldSelected);
            hotkeyField4.Click += new EventHandler(HotkeyAssignment_FieldSelected);

            autorunMacroCheckbox.CheckedChanged += new EventHandler(AutoStartMacro_CheckChanged);
            enableHotkeysCheckbox.CheckedChanged += new EventHandler(EnableHotkeys_CheckChanged);

            changeVersionButton.Click += new EventHandler(ChangeVersion_Click);
            refreshVersionsButton.Click += new EventHandler(RefreshVersions_Click);
            firewallToggleButton.Click += new EventHandler(FirewallToggle_Click);
            meathookToggleButton.Click += new EventHandler(MeathookToggle_Click);

            versionDropDownSelector.SelectedIndexChanged += new EventHandler(DropDown_IndexChanged);

            AddMouseIntercepts(this);
        }

        // Main timer method that runs this utility's logic.
        private void UpdateTick(object sender, EventArgs e) {
            if(_gameProcess == null || _gameProcess.HasExited) {
                Hooked = false;
                _gameProcess = null;
                _memory = null;
            }
            _fwRuleExists = FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", false);
            MeathookRemoval();
            _mhExists = CheckForMeathook();
            if(!meathookToggleButton.Enabled) {
                meathookToggleButton.Enabled = !_mhScheduleRemoval && !_mhExists;
            }

            if(_macroProcess.IsRunning) {
                macroStatus.Text = "Running";
                macroStatus.ForeColor = Color.Lime;
            } else {
                macroStatus.Text = "Stopped";
                macroStatus.ForeColor = Color.Red;
            }

            if(!Hooked) Hooked = Hook();
            if(!Hooked) {
                _macroProcess.Stop(true);
                if(_gameProcess != null) changeVersionButton.Enabled = true;
                _fwRestart = false;
                _mhRestart = false;
                return;
            }

            if(_enableMacro) _macroProcess.Start();
            else _macroProcess.Stop(true);

            _memory.DerefPointers();

            if(_memory.CanCapFPS() && _memory.ReadMaxHz() > 250) _memory.CapFPS(250);
            _memory.SetFlag(_fwRuleExists, "firewall");
            _memory.SetFlag(_macroProcess.IsRunning, "macro");
            _memory.SetMetrics(2);
            _memory.ModifyMetricRows();
        }

        /// <summary>
        /// Updates all hotkey selection fields with their respective current hotkeys.
        /// </summary>
        public void UpdateHotkeyFields() {
            foreach(Label l in _hotkeyFields) {
                Keys key = Keys.None;
                switch(l.Tag) {
                    case "macroDown":
                        key = _macroProcess.GetHotkey(true);
                        break;
                    case "macroUp":
                        key = _macroProcess.GetHotkey(false);
                        break;
                    case "fps0":
                        key = _hotkeys.GetHotkeyByNumber(0);
                        break;
                    case "fps1":
                        key = _hotkeys.GetHotkeyByNumber(1);
                        break;
                    case "fps2":
                        key = _hotkeys.GetHotkeyByNumber(2);
                        break;
                }
                l.Text = HotkeyHandler.TranslateKeyNames(key);
                l.ForeColor = Color.LightGray;
                l.BackColor = TEXT_BACKCOLOR;
            }
            fpsInput0.Text = _fps0.ToString();
            fpsInput1.Text = _fps1.ToString();
            fpsInput2.Text = _fps2.ToString();
        }

        public void PopulateVersionDropDown() {
            versionDropDownSelector.Items.Clear();
            for(int i = 0; i < _gameVersions.Count; i++) {
                string v = _gameVersions.ElementAt(i);
                versionDropDownSelector.Items.Add(v);
                if(v == GetCurrentVersion()) versionDropDownSelector.SelectedIndex = i;
            }
            if(!Hooked) changeVersionButton.Enabled = false;
        }

        public string GetCurrentVersion() {
            if(_gameDirectory != string.Empty) {
                string s = File.ReadAllText(_gameDirectory + "\\gameVersion.txt").Trim();
                return s.Substring(s.IndexOf('=') + 1);
            }
            return "Unknown";
        }

        /// <summary>
        /// Sets <c>com_adaptiveTickMaxHz</c> to the desired cap value. Sets to <c>250</c> if already at the desired cap.
        /// </summary>
        /// <param name="fpsHotkey">Which hotkey to trigger</param>
        public void ToggleFPSCap(int fpsHotkey) {
            int newFPS = 250;
            switch(fpsHotkey) {
                case 0:
                    if(_fps0 != -1) if(_memory.ReadMaxHz() != _fps0) newFPS = _fps0;
                    break;
                case 1:
                    if(_fps1 != -1) if(_memory.ReadMaxHz() != _fps1) newFPS = _fps1;
                    break;
                case 2:
                    if(_fps2 != -1) if(_memory.ReadMaxHz() != _fps2) newFPS = _fps2;
                    break;
            }
            _memory.CapFPS(newFPS);
        }

        private void ToggleIndividualHotkeys() {
            _hotkeys.ToggleIndividualHotkeys(0, !(_fps0 == -1));
            _hotkeys.ToggleIndividualHotkeys(1, !(_fps1 == -1));
            _hotkeys.ToggleIndividualHotkeys(2, !(_fps2 == -1));
        }

        // Adds a MouseDown event to every control in the form, recursively.
        private void AddMouseIntercepts(Control control) {
            foreach(Control c in control.Controls) {
                c.MouseDown += new MouseEventHandler(HotkeyAssignment_MouseDown);
                if(c.Controls.Count > 0) AddMouseIntercepts(c);
            }
        }

        // Auto-detects game directory. Asks for manual selection if it cannot be found.
        private void SearchForSteamGameDir() {
            List<string> SteamLibraryDrives = new();
            if(_libraryVDFLocation == string.Empty) {
                string steamPath, vdfPath;
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Valve\\");

                // Finding every Steam Library
                using RegistryKey subKey = key.OpenSubKey("Steam");
                try {
                    steamPath = subKey.GetValue("InstallPath").ToString();
                } catch(Exception) {
                    Debug.WriteLine("Couldn't find Steam install path! Does it even exist?");

                    // TODO - Popup window to manually select game dir
                    return;
                }

                vdfPath = steamPath + @"\steamapps\libraryfolders.vdf";
                _libraryVDFLocation = vdfPath;
            }
            string driveRegex = @"[A-Z]:\\";
            if(File.Exists(_libraryVDFLocation)) {
                string[] vdfLines = File.ReadAllLines(_libraryVDFLocation);

                foreach(string s in vdfLines) {
                    Match match = Regex.Match(s, driveRegex);
                    if(s != string.Empty && match.Success) {
                        string matched = match.ToString();
                        string item = s.Substring(s.IndexOf(matched));
                        item = item.Replace("\\\\", "\\");
                        item = item.Replace("\"", "\\steamapps\\common\\");
                        SteamLibraryDrives.Add(item);
                    }
                }
            }

            // Finding which library contains the game
            foreach(string dir in SteamLibraryDrives) {
                if(dir != string.Empty) {
                    if(Directory.Exists(dir + "DOOMEternal")) {
                        var exe = dir + "DOOMEternal\\DOOMEternalx64vk.exe";
                        if(File.Exists(exe)) {
                            _steamDirectory = dir;
                            _gameDirectory = dir + "DOOMEternal";
                            break;
                        }
                    }
                }
            }
            DetectAllGameVersions();
        }

        // Detects any extra game dirs in the same library and adds a gameVersion.txt for version swapping purposes
        // Folders MUST be in the format "DOOMEternal <versionString>"
        // List of valid version strings can be found in MemoryHandler.IsValidVersionString(); will have an info button with this list in the program window
        private void DetectAllGameVersions() {
            List<string> Directories = new();
            if(_steamDirectory != string.Empty) {
                string[] gameDirs = Directory.GetDirectories(_steamDirectory);
                foreach(var dir in gameDirs) {
                    if(!dir.Contains("DOOMEternal")) continue;
                    Directories.Add(dir);

                    var subDir = dir.Substring(dir.IndexOf("DOOMEternal"));
                    if(!subDir.StartsWith("DOOMEternal ")) continue;

                    var ver = subDir.Substring(subDir.IndexOf(' ') + 1);
                    if(MemoryHandler.IsValidVersionString(ver)) File.WriteAllText(dir + "\\gameVersion.txt", "version=" + ver);
                }
            }

            // Once all the gameVersion.txt files are in place, this populates the dropdown selector for version swapping
            _gameVersions = new();
            if(Directories.Count > 0) {
                foreach(var dir in Directories) {
                    string txt = File.ReadAllText(dir + "\\gameVersion.txt").Trim();
                    string v = txt.Substring(txt.IndexOf('=') + 1);
                    if(MemoryHandler.IsValidVersionString(v)) _gameVersions.Add(v);
                }
            }
            _gameVersions.Sort();
            PopulateVersionDropDown();
        }

        private bool CheckForMeathook() {
            bool mh = File.Exists(_gameDirectory + "\\XINPUT1_3.dll");
            meathookToggleButton.Text = mh ? "Disable meath00k" : "Enable meath00k";
            return mh;
        }

        private void MeathookRemoval() {
            if(_mhScheduleRemoval) {
                if(Hooked) {
                    if(_memory.GetFlag("cheats")) {
                        _mhDoRemovalTask = true;
                        meathookToggleButton.Enabled = false;
                        meathookRestartLabel.ForeColor = Color.LightGray;
                        return;
                    }
                }
                if(_mhDoRemovalTask) {
                    _mhScheduleRemoval = false;
                    Task.Run(async delegate {
                        _mhDoRemovalTask = false;
                        await Task.Delay(2000);
                        if(_mhExists) {
                            try {
                                File.Delete(_gameDirectory + "\\XINPUT1_3.dll");
                            } catch(Exception e) {
                                Debug.WriteLine(e.StackTrace);
                            }
                        }
                        meathookRestartLabel.ForeColor = Color.FromArgb(45, 45, 45);
                    });
                }else {
                    if(_mhScheduleRemoval == true && _mhExists) {
                        File.Delete(_gameDirectory + "\\XINPUT1_3.dll");
                        _mhScheduleRemoval = false;
                    }
                }
            }
        }

        // Hooks into the DOOMEternalx64vk.exe process, then sets up pointers for memory reading/writing.
        private bool Hook() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
            if(procList.Count == 0) {
                _gameProcess = null;
                return false;
            }
            _gameProcess = procList[0];
            changeVersionButton.Enabled = false;

            if(_gameProcess.HasExited) return false;

            try {
                _memory = new MemoryHandler(_gameProcess);
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
                return false;
            }
            SetGameInfoByModuleSize();
            _hotkeys.EnableHotkeys();
            if(File.Exists(_gameDirectory + "\\XINPUT1_3.dll")) _memory.SetFlag(true, "cheats");
            return true;
        }

        // Sets various game info variables based on the detected module size.
        private void SetGameInfoByModuleSize() {
            gameVersion.Text = _memory.Version;
            if(_gameDirectory != string.Empty) {
                File.WriteAllText(_gameDirectory + "\\gameVersion.txt", "version=" + _memory.Version);
            }
            if(!_memory.CanCapFPS()) _hotkeys.DisableHotkeys();
        }

        #region EVENTS
        private void HotkeyAssignment_KeyDown(object sender, KeyEventArgs e) {
            if(!_hkAssignmentMode) return;
            Keys pressedKey;

            if(e.Control) pressedKey = HotkeyHandler.ModKeySelector(0);
            else if(e.Shift) pressedKey = HotkeyHandler.ModKeySelector(1);
            else if(e.Alt) pressedKey = HotkeyHandler.ModKeySelector(2);
            else pressedKey = e.KeyCode;
            if(pressedKey == Keys.Escape) pressedKey = Keys.None;
            bool isValid = !_invalidKeys.Contains(pressedKey);

            string tag = (string) _selectedHKField.Tag;
            _hkAssignmentMode = false;
            _selectedHKField = null;
            if(isValid) {
                int type = -1;
                switch(tag) {
                    case "macroDown":
                        type = 3;
                        break;
                    case "macroUp":
                        type = 4;
                        break;
                    case "fps0":
                        type = 0;
                        break;
                    case "fps1":
                        type = 1;
                        break;
                    case "fps2":
                        type = 2;
                        break;
                }
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macroProcess, _hotkeys);
            }
            UpdateHotkeyFields();
            e.Handled = true;
        }
        private void HotkeyAssignment_MouseDown(object sender, MouseEventArgs e) {
            if(!_hkAssignmentMode) return;

            Keys pressedKey = HotkeyHandler.ConvertMouseButton(e.Button);
            bool isValid = !_invalidKeys.Contains(pressedKey);

            string tag = (string) _selectedHKField.Tag;
            _hkAssignmentMode = false;
            if(pressedKey == Keys.LButton && sender.Equals(_selectedHKField)) _mouse1Pressed = true;
            _selectedHKField = null;
            if(isValid) {
                int type = -1;
                switch(tag) {
                    case "macroDown":
                        type = 3;
                        break;
                    case "macroUp":
                        type = 4;
                        break;
                    case "fps0":
                        type = 0;
                        break;
                    case "fps1":
                        type = 1;
                        break;
                    case "fps2":
                        type = 2;
                        break;
                }
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macroProcess, _hotkeys);
            }
            UpdateHotkeyFields();
        }
        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            if(_mouse1Pressed) {
                _mouse1Pressed = false;
                return;
            }

            if((HotkeyHandler.GetAsyncKeyState(Keys.LButton) & 0x01) == 1) {
                _selectedHKField = (Label) sender;
                _selectedHKField.Text = "Press a key";
                _selectedHKField.BackColor = Color.FromKnownColor(KnownColor.ScrollBar);
                _selectedHKField.ForeColor = Color.Black;
                this.ActiveControl = null;

                _hkAssignmentMode = true;
            }
        }
        private void FPSInput_KeyPressNumericOnly(object sender, KeyPressEventArgs e) {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
                return;
            }
        }
        private void FPSInput_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;
            int p;
            try {
                p = int.Parse(text);
            } catch(FormatException) {
                p = -1;
            }
            if(p > 250) p = 250;
            if(p != -1) {
                if(p == 0) p = 1;
                ((TextBox) sender).Text = p.ToString();
            } else {
                ((TextBox) sender).Text = "";
            }

            var tag = ((Control) sender).Tag;
            switch(tag) {
                case "fpscap0":
                    _fps0 = p;
                    break;
                case "fpscap1":
                    _fps1 = p;
                    break;
                case "fpscap2":
                    _fps2 = p;
                    break;
            }
            ToggleIndividualHotkeys();
        }
        private void AutoStartMacro_CheckChanged(object sender, EventArgs e) => _enableMacro = ((CheckBox) sender).Checked;
        private void EnableHotkeys_CheckChanged(object sender, EventArgs e) {
            bool val = ((CheckBox) sender).Checked;
            if(val) {
                _hotkeys.EnableHotkeys();
            } else {
                _hotkeys.DisableHotkeys();
            }
        }
        private void RefreshVersions_Click(object sender, EventArgs e) {
            if(_steamDirectory != string.Empty) DetectAllGameVersions();
        }
        private void ChangeVersion_Click(object sender, EventArgs e) {
            string current = GetCurrentVersion(), desired = versionDropDownSelector.Text;
            if(current == desired) return;
            if(Directory.Exists(_steamDirectory + "\\DOOMEternal " + current)) return; // Eventually add a popup saying there's a folder conflict
            Directory.Move(_gameDirectory, _gameDirectory + " " + current);
            Directory.Move(_gameDirectory + " " + desired, _gameDirectory);
            changeVersionButton.Enabled = false;
            Task.Run(async delegate {
                versionChangedLabel.ForeColor = Color.LightGray;
                await Task.Delay(3000);
                versionChangedLabel.ForeColor = FORM_BACKCOLOR;
            });
        }
        private void FirewallToggle_Click(object sender, EventArgs e) {
            if(_fwRuleExists) {
                FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", true);
                firewallToggleButton.Text = "Create Firewall Rule";
                if(_fwRestart) _fwRestart = false;
                _fwRuleExists = false;
                firewallRestartLabel.ForeColor = Color.FromArgb(45, 45, 45);
            } else {
                FirewallHandler.CreateFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe");
                firewallToggleButton.Text = "Remove Firewall Rule";
                _fwRestart = true;
                _fwRuleExists = true;
                firewallRestartLabel.ForeColor = Color.LightGray;
            }
        }
        private void MeathookToggle_Click(object sender, EventArgs e) {
            // _mhRestart
            // disable button when removing mh while game is open - "schedule" removal after game stops
            // if mh isn't installed when game is launched, can freely add/remove mh, but still needs restart to take effect
            if(_mhExists) {
                _mhScheduleRemoval = true;
            }else {
                File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + "\\XINPUT1_3.dll");
                _mhRestart = true;
            }
        }
        private void DropDown_IndexChanged(object sender, EventArgs e) {
            if(!Hooked) changeVersionButton.Enabled = ((ComboBox) sender).Text != GetCurrentVersion();
        }

        // Event method that runs upon loading of the MainWindow form.
        private void MainWindow_Load(object sender, EventArgs e) {
            if(!File.Exists(@".\offsets.json")) File.WriteAllText(@".\offsets.json", System.Text.Encoding.UTF8.GetString(Properties.Resources.OffsetsJSON));
            MemoryHandler.OffsetList = JsonSerializer.Deserialize<List<MemoryHandler.KnownOffsets>>(File.ReadAllText(@".\offsets.json"));

            // User Settings
            _macroProcess = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
            _hotkeys = new HotkeyHandler((Keys) Properties.Settings.Default.FPS0Key, (Keys) Properties.Settings.Default.FPS1Key, (Keys) Properties.Settings.Default.FPS2Key, this);
            _fps0 = Properties.Settings.Default.FPSCap0;
            _fps1 = Properties.Settings.Default.FPSCap1;
            _fps2 = Properties.Settings.Default.FPSCap2;
            autorunMacroCheckbox.Checked = Properties.Settings.Default.MacroEnabled;
            enableHotkeysCheckbox.Checked = Properties.Settings.Default.FPSHotkeysEnabled;
            _libraryVDFLocation = Properties.Settings.Default.SteamVDFLocation;
            ToggleIndividualHotkeys();
            UpdateHotkeyFields();

            Point loc = Properties.Settings.Default.Location;
            if(loc != Point.Empty) Location = loc;

            _formTimer.Start();
        }

        // Event method that runs upon closing of the <c>MainWindow</c> form.
        private void MainWindow_Closing(object sender, FormClosingEventArgs e) {
            // User Settings
            Properties.Settings.Default.DownScrollKey = (int) _macroProcess.GetHotkey(true);
            Properties.Settings.Default.UpScrollKey = (int) _macroProcess.GetHotkey(false);
            Properties.Settings.Default.FPS0Key = (int) _hotkeys.GetHotkeyByNumber(0);
            Properties.Settings.Default.FPS1Key = (int) _hotkeys.GetHotkeyByNumber(1);
            Properties.Settings.Default.FPS2Key = (int) _hotkeys.GetHotkeyByNumber(2);
            Properties.Settings.Default.FPSCap0 = _fps0;
            Properties.Settings.Default.FPSCap1 = _fps1;
            Properties.Settings.Default.FPSCap2 = _fps2;
            Properties.Settings.Default.MacroEnabled = _enableMacro;
            Properties.Settings.Default.FPSHotkeysEnabled = _hotkeys.Enabled;
            Properties.Settings.Default.SteamVDFLocation = _libraryVDFLocation;
            if(WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = Location;
            else if(WindowState == FormWindowState.Minimized) Properties.Settings.Default.Location = RestoreBounds.Location;

            Properties.Settings.Default.Save();

            _hotkeys.DisableHotkeys();
            _macroProcess.Stop(false);
        }
        #endregion
    }
}