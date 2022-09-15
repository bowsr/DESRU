using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Win32;
using Serilog;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil {
    public partial class MainWindow: Form {
        private readonly Color FORM_BACKCOLOR = Color.FromArgb(35, 35, 35);
        private readonly Color PANEL_BACKCOLOR = Color.FromArgb(45, 45, 45);
        private readonly Color TEXT_BACKCOLOR = Color.FromArgb(70, 70, 70);
        private readonly Color TEXT_FORECOLOR = Color.FromArgb(230, 230, 230);

        private readonly Keys[] INVALID_KEYS = { Keys.Oemtilde, Keys.LButton, Keys.RButton };

        private const string WINDOW_TITLE = "DOOM ETERNAL SPEEDRUN UTILITY";
        private const string PROFILE_DIR = @"\782330\remote\PROFILE";
        private const string PROFILE_FILE = @"\profile.bin";
        private const string FPSKEYS_JSON = @".\fpskeys.json";

        private const int MAX_SCROLL_DELTA = 100; // Max milliseconds between scroll inputs

        private PrivateFontCollection _fonts = new();
        public static Font EternalUIRegular, EternalUIBold, EternalLogoBold, EternalBattleBold;

        Process? _gameProcess;
        public bool Hooked = false;
        bool _duplicateProcesses = false, _firstRun = true;
        bool _gameInFocus = false;

        bool _mouseDown;
        Point _lastLocation;

        bool _fwRuleExists = false, _fwRestart = false;
        bool _mhExists = false, _mhScheduleRemoval = false, _mhDoRemovalTask = false;
        bool _reshadeExists = false;

        FreescrollMacro? _macroProcess;
        bool _enableMacro = true;

        HotkeyHandler? _hotkeys;
        int _fpsDefault, _minResPercent, _targetFPS;

        MemoryHandler? _memory;

        Timer _formTimer, _statusTimer;

        bool _hkAssignmentMode = false, _mouse1Pressed = false;
        Label _selectedHKField = null;

        List<Label> _hotkeyFields;
        List<TextBox> _fpsLimitFields;

        string _gameDirectory = "", _steamDirectory = "", _steamInstallation = "", _steamID3 = "";
        List<string>? _gameVersions;

        bool _trackScroll = false, _displayPattern = false, _direction = false, _directionChanged = false;
        string _storedDisplay = "";
        long _scrollTime, _scrollDisplayTime;
        ScrollPattern _scrollPattern = new();
        struct ScrollPattern {
            public int ScrollCount { get; set; }
            public long DeltaTotal { get; set; }

            public long Average() => (ScrollCount > 1) ? DeltaTotal / (ScrollCount - 1) : 0;
            public void Reset() {
                ScrollCount = 0;
                DeltaTotal = 0;
            }
        }

        internal event KeyEventHandler RIKeyDown;
        internal event KeyEventHandler RIKeyUp;
        internal event MouseEventHandler RIMouseDown;
        internal event MouseEventHandler RIMouseUp;
        internal event EventHandler<MouseWheelEventArgs> RIMouseScroll;

        public MainWindow() {
            if(Program.UseRawInput) {
                RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, this.Handle);
                RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, this.Handle);
            }

            InitializeComponent();
            _hotkeyFields = new();
            _fpsLimitFields = new();
            CollectHotkeyAndLimitFields(this);

            _formTimer = new Timer();
            _formTimer.Interval = Program.TimerInterval;
            _formTimer.Tick += (sender, e) => { UpdateTick(); };

            _statusTimer = new Timer();
            _statusTimer.Interval = 1000;
            _statusTimer.Tick += (sender, e) => { StatusTick(); };

            desruVersionLabel.Text = Program.APP_VERSION;

            RemoveTabStop(this);
        }

        /// <summary>
        /// Windows Message handler for RawInput handling
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            const RawKeyboardFlags DOWN = RawKeyboardFlags.None;
            const RawKeyboardFlags RDOWN = RawKeyboardFlags.KeyE0;
            const RawKeyboardFlags UP = RawKeyboardFlags.Up;
            const RawKeyboardFlags RUP = RawKeyboardFlags.Up | RawKeyboardFlags.KeyE0;

            if(m.Msg == 0x00FF && _gameInFocus) {
                var data = RawInputData.FromHandle(m.LParam);
                if(data is RawInputMouseData mouse) {
                    if(mouse.Mouse.Buttons == RawMouseButtonFlags.MouseWheel) {
                        var mData = mouse.Mouse.ButtonData;
                        if(mData == 120 || mData == -120) {
                            RIMouseScroll?.Invoke(this, new MouseWheelEventArgs(mData == -120));
                        }
                    }else {
                        var rawButton = mouse.Mouse.Buttons;
                        var button = MouseButtons.None;
                        bool down;
                        switch(rawButton) {
                            case RawMouseButtonFlags.MiddleButtonDown:
                            case RawMouseButtonFlags.MiddleButtonUp:
                                button = MouseButtons.Middle;
                                down = rawButton == RawMouseButtonFlags.MiddleButtonDown;
                                break;
                            case RawMouseButtonFlags.Button4Down:
                            case RawMouseButtonFlags.Button4Up:
                                button = MouseButtons.XButton1;
                                down = rawButton == RawMouseButtonFlags.Button4Down;
                                break;
                            case RawMouseButtonFlags.Button5Down:
                            case RawMouseButtonFlags.Button5Up:
                                button = MouseButtons.XButton2;
                                down = rawButton == RawMouseButtonFlags.Button5Down;
                                break;
                            default:
                                base.WndProc(ref m);
                                return;
                        }
                        if(button != MouseButtons.None) {
                            if(_hotkeys.HookedHotkeys.Contains(HotkeyHandler.ConvertMouseButton(button))) {
                                var mea = new MouseEventArgs(button, 1, 0, 0, 0);
                                if(down) {
                                    RIMouseDown?.Invoke(this, mea);
                                } else {
                                    RIMouseUp?.Invoke(this, mea);
                                }
                            }
                        }
                    }
                }else if(data is RawInputKeyboardData keyboard) {
                    var key = (Keys) keyboard.Keyboard.VirutalKey;
                    var flags = keyboard.Keyboard.Flags;
                    bool up = flags == UP || flags == RUP;
                    switch(key) {
                        case Keys.Menu:
                            key = (flags == RDOWN || flags == RUP) ? Keys.RMenu : Keys.LMenu;
                            break;
                        case Keys.ControlKey:
                            key = (flags == RDOWN || flags == RUP) ? Keys.RControlKey : Keys.LControlKey;
                            break;
                        case Keys.ShiftKey:
                            key = HotkeyHandler.ModKeySelector(1);
                            break;
                    }
                    if(_hotkeys.HookedHotkeys.Contains(key)) {
                        var kea = new KeyEventArgs(key);
                        if(!up) {
                            RIKeyDown?.Invoke(this, kea);
                        }else {
                            RIKeyUp?.Invoke(this, kea);
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        // Main timer method that runs this utility's logic.
        private void UpdateTick() {
            if(_gameProcess == null || _gameProcess.HasExited) {
                Hooked = false;
                _hotkeys.DisableHotkeys();
                _gameProcess = null;
                if(_memory != null) _memory.MemoryTimer.Stop();
                _memory = null;
                _gameInFocus = false;

                unlockResButton.Enabled = false;
                unlockResButton.Text = "Unlock Resolution Scaling";
            }
            if(!_fwRestart) firewallRestartLabel.ForeColor = PANEL_BACKCOLOR;
            firewallToggleButton.Text = _fwRuleExists ? "Remove Firewall Rule" : "Create Firewall Rule";

            CheckForMeathook();
            MeathookRemoval();

            if(!meathookToggleButton.Enabled) {
                meathookToggleButton.Enabled = !_mhScheduleRemoval && !_mhExists;
            }

            if(!Hooked) {
                try {
                    Hooked = Hook();
                }catch(Exception e) {
                    Log.Error(e, "An error occured when attempting to hook into the game.");
                    return;
                }
                _firstRun = false;
            }
            if(!Hooked) {
                versionDropDownSelector.Enabled = true;
                _macroProcess.Stop(true);
                _fwRestart = false;
                meathookRestartLabel.ForeColor = PANEL_BACKCOLOR;
                return;
            }

            /** Toggling Hotkeys/Macro when game changes focus **/
            try {
                _gameInFocus = CheckIfGameIsInFocus();
                if(enableHotkeysCheckbox.Checked) {
                    if(!_memory.GetFlag("unlockscheduled")) {
                        if(!_gameInFocus) {
                            _hotkeys.DisableHotkeys();
                        }else {
                            _hotkeys.EnableHotkeys();
                        }
                    }
                }
                if(!_gameInFocus) {
                    _macroProcess.Stop(true);
                }else {
                    if(_enableMacro) {
                        if(Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalMacro")).Count > 1) {
                            _macroProcess.Restart();
                        }
                        if(!_macroProcess.IsRunning) _macroProcess.Start();
                    }
                }
            }catch(Exception e) {
                Log.Error(e, "Failed to check if DOOM Eternal was in focus.");
            }

            if(!_enableMacro) _macroProcess.Stop(true);
            
            /** Memory Flags **/
            if(!_fwRuleExists) _memory.SetFlag(false, "firewall");
            if(_enableMacro) {
                _memory.SetFlag(_macroProcess.HasKeyBound(), "macro");
            }else {
                _memory.SetFlag(false, "macro");
            }

            if(_memory.GetFlag("resunlocked") && !_memory.GetFlag("unlockscheduled")) {
                unlockResButton.Enabled = true;
                unlockResButton.Text = "Update Minimum Resolution";
            }else if(!_memory.GetFlag("resunlocked") && !_memory.GetFlag("unlockscheduled")) {
                unlockResButton.Enabled = true;
                unlockResButton.Text = "Unlock Resolution Scaling";
            }
            if(_memory.GetFlag("unlockscheduled")) {
                unlockResButton.Enabled = false;
                unlockResButton.Text = "Unlock in Progress";
            }

            /** Scroll Pattern Tracking **/
            if(_trackScroll) {
                var delta = DateTime.Now.Ticks - _scrollTime;
                if((delta / 10000) >= MAX_SCROLL_DELTA || _directionChanged) {
                    string display = "";
                    if(!_directionChanged) {
                        _trackScroll = false;
                        var avg = _scrollPattern.Average() / 10000f;
                        display = _scrollPattern.ScrollCount + " (" + ((avg > 0f) ? avg.ToString("0.0") + "ms" : "-") + ")";
                    }else {
                        display = _storedDisplay;
                        _directionChanged = false;
                    }
                    _displayPattern = true;
                    _scrollDisplayTime = DateTime.Now.Ticks;
                    _memory.SetScrollPatternString(display);
                }
            }
            if(_displayPattern && ((DateTime.Now.Ticks - _scrollDisplayTime) / 10000) >= 2000) {
                _displayPattern = false;
                _memory.SetScrollPatternString(string.Empty);
            }
        }

        // Timer method used to update the info panel on the main form
        private void StatusTick() {
            // Checking for the Firewall rule can take upwards of 8ms
            // so it was moved out of the main timer and into this one since the interval is longer, for better performance
            try {
                _fwRuleExists = FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", false);
            }catch(Exception e) {
                Log.Error(e, "Failed to check if firewall rule exists.");
            }

            macroStatus.Text = (_macroProcess.IsRunning) ? "Running" : "Stopped";
            macroStatus.ForeColor = (_macroProcess.IsRunning) ? Color.Lime : TEXT_FORECOLOR;
            hotkeyStatus.Text = (_hotkeys.Enabled) ? "Enabled" : "Disabled";

            if(_memory != null) {
                var hz = _memory.ReadMaxHz();
                var v = _memory.Version;
                gameStatus.Text = v;
                if(v == "1.0 (Release)") {
                    slopeboostStatus.Text = (_memory.GetFlag("slopeboost")) ? "Disabled" : "Enabled";
                }else {
                    slopeboostStatus.Text = "N/A";
                }
                currentFPSCap.Text = (hz != -1) ? hz.ToString() : "N/A";
                var ms = _memory.ReadRaiseMillis();
                var min = _memory.ReadMinRes();
                if((ms > 0 && ms < 16) && min > 0) {
                    var rs = "Enabled (" + ((int) (1000 / (ms / 0.95f))) + "FPS)";
                    resScaleStatus.Text = (_memory.ReadDynamicRes() && min < 1.0f) ? rs : "Disabled";
                }else {
                    resScaleStatus.Text = "Disabled";
                }
            }else {
                slopeboostStatus.Text = "-";
                currentFPSCap.Text = "-";
                resScaleStatus.Text = "-";
                gameStatus.Text = "Not Running";
            }

            balanceStatus.Text = (_fwRuleExists) ? ((_fwRestart) ? "Allowed*" : "Blocked") : "Allowed";
            if(Hooked) {
                if(_memory.GetFlag("cheats")) {
                    cheatsStatus.Text = "Enabled";
                    cheatsStatus.ForeColor = Color.Red;
                }else {
                    cheatsStatus.Text = "Disabled";
                    cheatsStatus.ForeColor = TEXT_FORECOLOR;
                }
                if(_reshadeExists) {
                    reshadeStatus.Text = "Enabled";
                }else {
                    reshadeStatus.Text = "Disabled";
                }
            }else {
                cheatsStatus.Text = "-";
                cheatsStatus.ForeColor = TEXT_FORECOLOR;
                reshadeStatus.Text = "-";
            }
        }

        private bool CheckIfGameIsInFocus() {
            try {
                return _gameProcess.MainWindowHandle == GetForegroundWindow();
            }catch(Exception e) {
                Log.Error(e, "An error occured when checking if the game is in focus.");
                throw new Exception("Could not check if game window was in focus.");
            }
        }

        /// <summary>
        /// Counts every scroll input and the delta time between each input.
        /// </summary>
        /// <param name="dir">Scroll direction. <see langword="true"/> = down</param>
        public void TrackMouseWheel(bool dir) {
            var now = DateTime.Now.Ticks;
            if(!_trackScroll) {
                _trackScroll = true;
                _direction = dir;
                _scrollPattern.Reset();
                _scrollTime = now;
            }
            if(_trackScroll) {
                var delta = now - _scrollTime;
                if(dir != _direction) {
                    _directionChanged = true;
                    _scrollTime = now;
                    _direction = dir;
                    var avg = _scrollPattern.Average() / 10000f;
                    _storedDisplay = _scrollPattern.ScrollCount + " (" + ((avg > 0f) ? avg.ToString("0.0") + "ms" : "-") + ")";
                    _scrollPattern.Reset();
                }
                if(_scrollPattern.ScrollCount > 0) _scrollPattern.DeltaTotal += delta;
                _scrollPattern.ScrollCount++;
                _scrollTime = now;
            }
        }

        /// <summary>
        /// Updates all hotkey and input fields with their respective values.
        /// </summary>
        public void UpdateHotkeyAndInputFields() {
            foreach(Label l in _hotkeyFields) {
                string tag = (string) l.Tag;
                Keys key = tag switch {
                    "hkMacroDown" => _macroProcess.GetHotkey(true),
                    "hkMacroUp" => _macroProcess.GetHotkey(false),
                    "hkResToggle" => _hotkeys.ResScaleHotkey,
                    _ => _hotkeys.FPSHotkeys.GetKeyFromID(int.TryParse(tag.Replace("hkFps", ""), out int id) ? id : -1),
                };
                l.Text = HotkeyHandler.TranslateKeyNames(key);
                l.ForeColor = TEXT_FORECOLOR;
                l.BackColor = TEXT_BACKCOLOR;
            }
            foreach(TextBox t in _fpsLimitFields) {
                string tag = (string) t.Tag;
                int limit = _hotkeys.FPSHotkeys.GetLimitFromID(int.TryParse(tag.Replace("fpscap", ""), out int id) ? id : -1);
                t.Text = (limit != -1) ? limit.ToString() : "";
            }
            minResInput.Text = _minResPercent.ToString();
            targetFPSInput.Text = _targetFPS.ToString();
        }

        /// <summary>
        /// Populates the Version selector with all currently detected game versions
        /// </summary>
        public void PopulateVersionDropDown() {
            versionDropDownSelector.Items.Clear();
            for(int i = 0; i < _gameVersions.Count; i++) {
                string v = _gameVersions.ElementAt(i);
                versionDropDownSelector.Items.Add(v);
                if(v == GetCurrentVersion()) versionDropDownSelector.SelectedIndex = i;
            }
            changeVersionButton.Enabled = false;
        }

        /// <summary>
        /// Retrieves the currently installed version by reading gameVersion.txt
        /// </summary>
        /// <returns>A <see langword="string"/> representing a game version</returns>
        public string GetCurrentVersion() {
            if(_gameDirectory != string.Empty) {
                try {
                    string s = File.ReadAllText(_gameDirectory + "\\gameVersion.txt").Trim();
                    return s.Substring(s.IndexOf('=') + 1);
                }catch(Exception e) {
                    Log.Error(e, "An error occured when trying to read gameVersion.txt. Directory: {Directory}", _gameDirectory);

                }
            }
            return "Unknown";
        }

        /// <summary>
        /// Sets <c>com_adaptiveTickMaxHz</c> to the desired cap value. Sets to <c>250</c> if already at the desired cap.
        /// </summary>
        /// <param name="fps">FPS Limit</param>
        public void ToggleFPSCap(int fps) {
            if(!Hooked || fps == -1) return;
            int current = _memory.ReadMaxHz();
            _memory.SetMaxHz((current != fps) ? fps : _fpsDefault);
        }

        public void ToggleResScaling() {
            if(Hooked) {
                _memory.SetMinRes(((int) (_memory.ReadMinRes() * 100)) == _minResPercent ? 1f : _minResPercent / 100f);
                _memory.ScheduleResUnlock(false, _targetFPS);
            }
        }

        // Adds a MouseDown event to every control in the form, recursively.
        private void AddMouseIntercepts(Control control) {
            foreach(Control c in control.Controls) {
                c.MouseDown += new MouseEventHandler(HotkeyAssignment_MouseDown);
                if(c.Controls.Count > 0) AddMouseIntercepts(c);
            }
        }

        private void RemoveTabStop(Control control) {
            foreach(Control c in control.Controls) {
                c.TabStop = false;
                if(c.Controls.Count > 0) RemoveTabStop(c);
            }
        }

        #region Game and Saves Detection
        // Auto-detects game directory. Asks for manual selection if it cannot be found.
        private void SearchForSteamGameDir() {
            List<string> steamLibraryDrives = new();
            var vdfLocation = "";
            if(_gameDirectory == string.Empty) {
                string steamPath;
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Valve\\");

                // Finding every Steam Library
                using RegistryKey subKey = key.OpenSubKey("Steam");
                try {
                    steamPath = subKey.GetValue("InstallPath").ToString();
                    _steamInstallation = steamPath;
                } catch(Exception e) {
                    Log.Error(e, "Couldn't find a Steam installation!");

                    using GameDirectoryDialog gameSelection = new();
                    if(gameSelection.ShowDialog() == DialogResult.OK) {
                        _gameDirectory = gameSelection.FileName.Remove(gameSelection.FileName.IndexOf("\\DOOMEternalx64vk.exe"));
                        _steamDirectory = _gameDirectory.Remove(_gameDirectory.IndexOf("\\DOOMEternal"));
                        Log.Information("User manually selected their game directory.");
                        DetectAllGameVersions();
                    } else {
                        this.Close();
                    }
                    return;
                }

                vdfLocation = steamPath + @"\steamapps\libraryfolders.vdf";
            }else {
                Log.Information("Game directory loaded from user settings. Directory: {GameDirectory}", _gameDirectory);
            }
            Log.Information("Found Steam Installation.");
            string driveRegex = @"[A-Z]:\\";
            if(File.Exists(vdfLocation)) {
                string[] vdfLines = File.ReadAllLines(vdfLocation);

                foreach(string s in vdfLines) {
                    Match match = Regex.Match(s, driveRegex);
                    if(s != string.Empty && match.Success) {
                        string matched = match.ToString();
                        string item = s.Substring(s.IndexOf(matched));
                        item = item.Replace("\\\\", "\\");
                        item = item.Replace("\"", "\\steamapps\\common\\");
                        steamLibraryDrives.Add(item);
                    }
                }
            }

            // Finding which library contains the game
            foreach(string dir in steamLibraryDrives) {
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
            if(_gameDirectory == string.Empty) {
                Log.Error("Couldn't find the game installation!");
                using GameDirectoryDialog gameSelection = new();
                if(gameSelection.ShowDialog() == DialogResult.OK) {
                    _gameDirectory = gameSelection.FileName.Remove(gameSelection.FileName.IndexOf("\\DOOMEternalx64vk.exe"));
                    Log.Information("User manually selected their game directory.");
                    DetectAllGameVersions();
                } else {
                    Log.CloseAndFlush();
                    this.Close();
                }
                gameSelection.Dispose();
            }
            _steamDirectory = _gameDirectory.Remove(_gameDirectory.IndexOf("\\DOOMEternal"));
            DetectAllGameVersions();
        }

        // Detects any extra game dirs in the same library and adds a gameVersion.txt for version swapping purposes
        // Folders MUST be in the format "DOOMEternal <versionString>"
        // List of valid version strings can be found in MemoryHandler.IsValidVersionString(); can also be found in validVersions.txt
        private void DetectAllGameVersions() {
            List<string> directories = new();
            if(_steamDirectory != string.Empty) {
                if(!_steamDirectory.ToLower().Contains("steam")) {
                    _steamInstallation = "n/a";
                    return;
                }
                string[] gameDirs = Directory.GetDirectories(_steamDirectory);
                foreach(var dir in gameDirs) {
                    if(!dir.Contains("DOOMEternal")) continue;
                    directories.Add(dir);

                    var subDir = dir.Substring(dir.IndexOf("DOOMEternal"));
                    if(!subDir.StartsWith("DOOMEternal ")) continue;

                    var ver = subDir.Substring(subDir.IndexOf(' ') + 1);
                    if(MemoryHandler.IsValidVersionString(ver)) File.WriteAllText(dir + "\\gameVersion.txt", "version=" + ver);
                    Log.Information("Found extra game installation; Version: {Version}", ver);
                }
            }

            // Once all the gameVersion.txt files are in place, this populates the dropdown selector for version swapping
            _gameVersions = new();
            if(directories.Count > 0) {
                foreach(var dir in directories) {
                    try {
                        string txt = File.ReadAllText(dir + "\\gameVersion.txt").Trim();
                        string v = txt.Substring(txt.IndexOf('=') + 1);
                        if(MemoryHandler.IsValidVersionString(v)) _gameVersions.Add(v);
                    } catch(Exception e) {
                        Log.Error(e, "An error occured when trying to read gameVersion.txt. Directory: {Directory}", dir);
                        continue;
                    }
                }
            }
            _gameVersions.Sort();
            PopulateVersionDropDown();
        }

        private void SearchForGameSaves() {
            if(_steamInstallation == string.Empty) {
                using RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Valve\\Steam");
                try {
                    _steamInstallation = key.GetValue("InstallPath").ToString();
                } catch(Exception e) {
                    Log.Error(e, "Encountered an error when attempting to detect Steam's installation location.");
                    return;
                }
            }
            List<string> userProfiles = new(), id3s = new();
            if(_steamID3 == string.Empty) {
                foreach(string profile in Directory.GetDirectories(_steamInstallation + "\\userdata")) {
                    if(!long.TryParse(profile[(profile.LastIndexOf('\\') + 1)..], out long id3)) continue;
                    Log.Information("Found Steam userdata. steamID3: {ID3}", id3);
                    userProfiles.Add(id3.ToString());
                    id3s.Add(id3.ToString());
                }
                Log.Information("Found {Num} Steam userdata directories.", userProfiles.Count);
                if(userProfiles.Count == 0) return;
                foreach(string profile in userProfiles) {
                    if(!File.Exists(_steamInstallation + "\\userdata\\" + profile + PROFILE_DIR + PROFILE_FILE)) id3s.Remove(profile);
                }
            }
            if(id3s.Count == 1) {
                _steamID3 = id3s[0];
                Log.Information("DE saves folder detected. steamID3: {ID3}", _steamID3);
                return;
            }else if(id3s.Count > 1) {
                Log.Warning("Multiple Steam userdata folders with DOOMEternal data detected. Prompting user to select the correct profile.");
                id3s.Sort();
                UserdataDialog dialog = new(id3s);
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    _steamID3 = dialog.ID3;
                    Log.Information("User selected steamID3: {ID3}", _steamID3);
                }else {
                    Log.CloseAndFlush();
                    this.Close();
                }
            }
        }
        #endregion

        private bool CheckForMeathook() {
            bool mh = File.Exists(_gameDirectory + "\\XINPUT1_3.dll");
            meathookToggleButton.Text = mh ? "Disable Cheats" : "Enable Cheats";
            _mhExists = mh;
            return mh;
        }

        // Checks if ReShade is both installed for Vulkan and set to run over DOOMEternalx64vk
        private bool CheckForReShade() {
            using RegistryKey vkKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Khronos\Vulkan\ImplicitLayers\");
            var names = vkKey.GetValueNames();
            foreach(string s in names) {
                if(s.Contains("ReShade")) {
                    Log.Information("ReShade Vulkan Layer found in registry.");
                    try {
                        var rs =  File.ReadAllText(@"C:\ProgramData\ReShade\ReShadeApps.ini").Contains(_gameDirectory);
                        if(rs) Log.Information("ReShade is installed and is running over DOOMEternal.");
                        else Log.Information("ReShade is not running over DOOMEternal.");
                        return rs;
                    } catch(Exception e) {
                        Log.Error(e, "An error occured when checking ReShade files. Assuming ReShade is running over DOOMEternal.");
                        return true;
                    }
                }
            }
            Log.Information("ReShade Vulkan Layer not found in registry.");
            return false;
        }

        private void MeathookRemoval() {
            if(_mhScheduleRemoval) {
                if(Hooked) {
                    if(_memory.GetFlag("cheats")) {
                        _mhDoRemovalTask = true;
                        meathookToggleButton.Enabled = false;
                        meathookRestartLabel.ForeColor = TEXT_FORECOLOR;
                        return;
                    }
                }
                // Can't remove the DLL while the game is running, so it's scheduled, then deleted after a delay 
                // once the game is detected as closed.
                if(_mhDoRemovalTask) {
                    _mhScheduleRemoval = false;
                    Task.Run(async delegate {
                        _mhDoRemovalTask = false;
                        await Task.Delay(2000);
                        if(_mhExists) {
                            try {
                                File.Delete(_gameDirectory + "\\XINPUT1_3.dll");
                                Log.Information("meath00k uninstalled.");
                            } catch(Exception e) {
                                Log.Error(e, "An error occurred when attempting to uninstall meath00k.");
                            }
                        }
                        meathookRestartLabel.ForeColor = PANEL_BACKCOLOR;
                    });
                } else {
                    if(_mhScheduleRemoval == true && _mhExists) {
                        try {
                            File.Delete(_gameDirectory + "\\XINPUT1_3.dll");
                            Log.Information("meath00k uninstalled.");
                        } catch(Exception e) {
                            Log.Error(e, "An error occurred when attempting to uninstall meath00k.");
                        }
                        _mhScheduleRemoval = false;
                        meathookRestartLabel.ForeColor = PANEL_BACKCOLOR;
                    }
                }
            }
        }

        // Hooks into the DOOMEternalx64vk.exe process, then sets up pointers for memory reading/writing.
        private bool Hook() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
            if(procList.Count == 0) {
                _gameProcess = null;
                _duplicateProcesses = false;
                return false;
            }
            if(procList.Count > 1) {
                if(!_duplicateProcesses) {
                    _duplicateProcesses = true;
                    Log.Error("Multiple DOOM Eternal processes detected!");
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show(this, "Multiple instances of DOOM Eternal have been detected.\n" +
                    "Close them or restart your system to clear them out.", "Multiple DOOMEternalx64vk.exe Instances Detected");
                }
                _gameProcess = null;
                return false;
            }
            _gameProcess = procList[0];
            changeVersionButton.Enabled = false;

            if(_gameProcess.HasExited || _gameProcess == null) return false;
            Log.Information("Found a DOOMEternalx64vk.exe process.");

            _reshadeExists = CheckForReShade();

            try {
                _memory = new MemoryHandler(_gameProcess, _hotkeys);
            }catch(ArgumentNullException ex) {
                Log.Error(ex, "An error occured when attempting to hook into the game.");
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(_memory == null) {
                Log.Error("MemoryHandler was somehow null. Retrying hook.");
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(_memory.Reset) {
                Log.Error("Something went wrong when setting up the MemoryHandler. Retrying hook.");
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(enableHotkeysCheckbox.Checked) {
                _hotkeys.EnableHotkeys();
                _hotkeys.ToggleResScaleKey(false);
            }
            SetGameInfoByModuleSize();
            try {
                versionDropDownSelector.SelectedItem = GetCurrentVersion();
            }catch(Exception e) {
                Log.Error(e, "An error occured when attempting to change the version selector's index.");
            }
            versionDropDownSelector.Enabled = false;
            _memory.SetFlag(_fwRuleExists, "firewall");
            _memory.SetFlag(CheckForMeathook(), "cheats");
            _memory.SetFlag(_reshadeExists, "reshade");
            _memory.SetFlag(Program.UpdateDetected, "outofdate");
            _memory.SetFlag(_firstRun, "restart");
            if(_firstRun) Log.Warning("Game requires a restart. Utility must be running before the game is launched.");
            _memory.SetMinRes(_minResPercent / 100f);
            if(unlockOnStartupCheckbox.Checked) {
                _memory.ScheduleResUnlock(autoDynamicCheckbox.Checked, _targetFPS);
                _hotkeys.DisableHotkeys();
            }else {
                if(autoDynamicCheckbox.Checked) _memory.ScheduleDynamicScaling(_targetFPS);
            }
            _memory.SetMaxHz(_fpsDefault);
            _memory.MemoryTimer.Start();
            return true;
        }

        // Sets various game info variables based on the detected module size.
        private void SetGameInfoByModuleSize() {
            gameStatus.Text = _memory.Version;
            if(_gameDirectory != string.Empty) {
                File.WriteAllText(_gameDirectory + "\\gameVersion.txt", "version=" + _memory.Version);
            }
            if(!_memory.CanCapFPS()) _hotkeys.DisableHotkeys();
        }

        private void InitializeFonts() {
            foreach(string file in Directory.GetFiles(@".\fonts")) _fonts.AddFontFile(file);
            foreach(FontFamily ff in _fonts.Families) {
                switch(ff.Name) {
                    case "Eternal UI 2":
                        EternalUIRegular = new(ff, 11.25f, FontStyle.Regular, GraphicsUnit.Point);
                        EternalUIBold = new(ff, 11.25f, FontStyle.Bold, GraphicsUnit.Point);
                        break;
                    case "Eternal Battle":
                        EternalBattleBold = new(ff, 20.25f, FontStyle.Bold, GraphicsUnit.Point);
                        break;
                    case "Eternal Logo":
                        EternalLogoBold = new(ff, 14f, FontStyle.Bold, GraphicsUnit.Point);
                        break;
                }
            }
            SetFonts();
        }

        private void CollectHotkeyAndLimitFields(Control control) {
            foreach(Control c in control.Controls) {
                if(c.Tag != null) {
                    string tag = (string) c.Tag;
                    if(tag.StartsWith("hk")) {
                        c.Click += new EventHandler(HotkeyAssignment_FieldSelected);
                        _hotkeyFields.Add((Label) c);
                    }else if(tag.StartsWith("fpscap")) {
                        c.KeyPress += new KeyPressEventHandler(Input_KeyPressNumericOnly);
                        c.KeyUp += new KeyEventHandler(FPSInput_KeyUp);
                        _fpsLimitFields.Add((TextBox) c);
                    }
                }
                if(c.Controls.Count > 0) CollectHotkeyAndLimitFields(c);
            }
        }

        private void SetFonts() {
            // Eternal UI 2 Regular 11.25point
            foreach(Control c in _hotkeyFields) c.Font = EternalUIRegular;
            macroUpKeyLabel.Font = EternalUIRegular;
            macroDownKeyLabel.Font = EternalUIRegular;
            resKeyLabel.Font = EternalUIRegular;
            fpsKey0Label.Font = EternalUIRegular;
            fpsKey1Label.Font = EternalUIRegular;
            fpsKey2Label.Font = EternalUIRegular;
            fpsInput0.Font = EternalUIRegular;
            fpsInput1.Font = EternalUIRegular;
            fpsInput2.Font = EternalUIRegular;
            fpsLabel0.Font = EternalUIRegular;
            fpsLabel1.Font = EternalUIRegular;
            fpsLabel2.Font = EternalUIRegular;
            defaultFPSLabel.Font = EternalUIRegular;
            defaultFPS.Font = EternalUIRegular;
            versionDropDownSelector.Font = EternalUIRegular;
            autorunMacroCheckbox.Font = EternalUIRegular;
            enableHotkeysCheckbox.Font = EternalUIRegular;
            gameStatus.Font = EternalUIRegular;
            currentFPSCap.Font = EternalUIRegular;
            macroStatus.Font = EternalUIRegular;
            slopeboostStatus.Font = EternalUIRegular;
            balanceStatus.Font = EternalUIRegular;
            cheatsStatus.Font = EternalUIRegular;
            reshadeStatus.Font = EternalUIRegular;
            resScaleStatus.Font = EternalUIRegular;
            hotkeyStatus.Font = EternalUIRegular;
            unlockOnStartupCheckbox.Font = EternalUIRegular;
            autoDynamicCheckbox.Font = EternalUIRegular;
            enableMouseHookCheckbox.Font = EternalUIRegular;
            minResLabel.Font = EternalUIRegular;
            dynamicTargetLabel.Font = EternalUIRegular;
            minResInput.Font = EternalUIRegular;
            targetFPSInput.Font = EternalUIRegular;
            percentLabel.Font = EternalUIRegular;
            targetFPSLabel.Font = EternalUIRegular;

            // Eternal UI 2 Bold 11.25point
            versionChangedLabel.Font = EternalUIBold;
            changeVersionButton.Font = EternalUIBold;
            refreshVersionsButton.Font = EternalUIBold;
            firewallToggleButton.Font = EternalUIBold;
            meathookToggleButton.Font = EternalUIBold;
            firewallRestartLabel.Font = EternalUIBold;
            meathookRestartLabel.Font = EternalUIBold;
            gameStatusLabel.Font = EternalUIBold;
            fpsCapLabel.Font = EternalUIBold;
            macroStatusLabel.Font = EternalUIBold;
            slopeboostStatusLabel.Font = EternalUIBold;
            balanceStatusLabel.Font = EternalUIBold;
            cheatsStatusLabel.Font = EternalUIBold;
            reshadeStatusLabel.Font = EternalUIBold;
            resScaleStatusLabel.Font = EternalUIBold;
            hotkeyStatusLabel.Font = EternalUIBold;
            exitButton.Font = EternalUIBold;
            downpatcherButton.Font = EternalUIBold;
            discordButton.Font = EternalUIBold;
            helpButton.Font = EternalUIBold;
            unlockResButton.Font = EternalUIBold;

            // Eternal Logo Bold 14point
            hotkeysTitle.Font = EternalLogoBold;
            versionTitle.Font = EternalLogoBold;
            optionsTitle.Font = EternalLogoBold;
            resTitle.Font = EternalLogoBold;
            infoPanelTitle.Font = EternalLogoBold;

            // Eternal Battle Bold 20.25point
            windowTitle.Font = EternalBattleBold;
        }

        public bool IsFormOnScreen() {
            Screen[] screens = Screen.AllScreens;
            foreach(var display in screens) {
                Rectangle rect = new(this.Left, this.Top, this.Width, this.Height);
                if(display.WorkingArea.Contains(rect)) return true;
            }
            return false;
        }

        #region COMPONENTS
        public class DESRUShadowLabel: Label {
            public DESRUShadowLabel(Font font, string text, Point loc, Color color, Color back) {
                this.AutoSize = true;
                this.Font = font;
                this.Location = loc;
                this.ForeColor = color;
                this.BackColor = back;
                this.Text = text;
                this.Padding = new Padding(0, 0, 0, 0);
            }
            protected override void OnPaint(PaintEventArgs e) {
                //base.OnPaint(e);
                e.Graphics.DrawString(Text, Font, new SolidBrush(Color.Black), 2, 2);
                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), 0, 0);
            }
        }
        #endregion

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
    }
}