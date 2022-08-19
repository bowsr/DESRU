using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil {
    public partial class MainWindow: Form {
        Process GameProcess;
        public bool Hooked = false;

        FreescrollMacro MacroProcess;
        bool AutoRunMacro = true;

        HotkeyHandler Hotkeys;
        int fps0, fps1, fps2;

        MemoryHandler Memory;

        Timer FormTimer;

        bool HKAssignmentMode = false, Mouse1Pressed = false;
        Label SelectedHKField = null;

        Keys[] InvalidKeys = { Keys.Escape, Keys.Oemtilde, Keys.LButton, Keys.RButton };

        List<Label> HotkeyFields;

        string GameDirectory = "", SteamDirectory = "";

        public MainWindow() {
            InitializeComponent();
            SearchForSteamGameDir();

            HotkeyFields = new List<Label>();
            HotkeyFields.Add(hotkeyField0);
            HotkeyFields.Add(hotkeyField1);
            HotkeyFields.Add(hotkeyField2);
            HotkeyFields.Add(hotkeyField3);
            HotkeyFields.Add(hotkeyField4);

            FormTimer = new Timer();
            FormTimer.Interval = 8;
            FormTimer.Tick += new EventHandler(UpdateTick);

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

            AddMouseIntercepts(this);
        }

        
        // Main timer method that runs this utility's logic.
        private void UpdateTick(object sender, EventArgs e) {
            if(GameProcess == null || GameProcess.HasExited) {
                GameProcess = null;
                Hooked = false;
                MacroProcess.Stop(true);
            }

            if(MacroProcess.IsRunning) {
                macroStatus.Text = "Running";
                macroStatus.ForeColor = Color.Lime;
            }else {
                macroStatus.Text = "Stopped";
                macroStatus.ForeColor = Color.Red;
            }

            if(!Hooked) Hooked = Hook();
            if(!Hooked) return;

            if(AutoRunMacro) MacroProcess.Start();

            //Memory.DerefPointers();
            //Memory.TestRows();
        }
        #region EVENTS
        private void HotkeyAssignment_KeyDown(object sender, KeyEventArgs e) {
            if(!HKAssignmentMode) return;
            Keys pressedKey;

            if(e.Control) pressedKey = HotkeyHandler.ModKeySelector(0);
            else if(e.Shift) pressedKey = HotkeyHandler.ModKeySelector(1);
            else if(e.Alt) pressedKey = HotkeyHandler.ModKeySelector(2);
            else pressedKey = e.KeyCode;
            bool isValid = !InvalidKeys.Contains(pressedKey);

            string tag = (string) SelectedHKField.Tag;
            HKAssignmentMode = false;
            SelectedHKField = null;
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
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, MacroProcess, Hotkeys);
            }
            UpdateHotkeyFields();
            e.Handled = true;
        }
        private void HotkeyAssignment_MouseDown(object sender, MouseEventArgs e) {
            if(!HKAssignmentMode) return;

            Keys pressedKey = HotkeyHandler.ConvertMouseButton(e.Button);
            bool isValid = !InvalidKeys.Contains(pressedKey);

            string tag = (string) SelectedHKField.Tag;
            HKAssignmentMode = false;
            if(pressedKey == Keys.LButton && sender.Equals(SelectedHKField)) Mouse1Pressed = true;
            SelectedHKField = null;
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
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, MacroProcess, Hotkeys);
            }
            UpdateHotkeyFields();
        }
        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            if(Mouse1Pressed) {
                Mouse1Pressed = false;
                return;
            }

            if((HotkeyHandler.GetAsyncKeyState(Keys.LButton) & 0x01) == 1) {
                SelectedHKField = (Label) sender;
                SelectedHKField.Text = "Press a key";
                SelectedHKField.BackColor = Color.Gold;
                this.ActiveControl = null;

                HKAssignmentMode = true;
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
            }catch(FormatException) {
                p = -1;
            }
            if(p > 250) p = 250;
            if(p != -1) {
                if(p == 0) p = 1;
                ((TextBox) sender).Text = p.ToString();
            }else {
                ((TextBox) sender).Text = "";
            }

            var tag = ((Control) sender).Tag;
            switch(tag) {
                case "fpscap0":
                    fps0 = p;
                    break;
                case "fpscap1":
                    fps1 = p;
                    break;
                case "fpscap2":
                    fps2 = p;
                    break;
            }
            if(fps0 == -1) Hotkeys.ToggleIndividualHotkeys(0, false);
            if(fps1 == -1) Hotkeys.ToggleIndividualHotkeys(1, false);
            if(fps2 == -1) Hotkeys.ToggleIndividualHotkeys(2, false);
        }

        // Event method that runs upon loading of the <c>MainWindow</c> form.
        private void MainWindow_Load(object sender, EventArgs e) {
            // User Settings
            MacroProcess = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
            Hotkeys = new HotkeyHandler((Keys) Properties.Settings.Default.FPS0Key, (Keys) Properties.Settings.Default.FPS1Key, (Keys) Properties.Settings.Default.FPS2Key, this);
            UpdateHotkeyFields();

            Point loc = Properties.Settings.Default.Location;
            if(loc != Point.Empty) Location = loc;

            FormTimer.Start();
        }
        
        // Event method that runs upon closing of the <c>MainWindow</c> form.
        private void MainWindow_Closing(object sender, FormClosingEventArgs e) {
            MacroProcess.Stop(false);
            Hotkeys.DisableHotkeys();

            // User Settings
            Properties.Settings.Default.DownScrollKey = (int) MacroProcess.GetHotkey(true);
            Properties.Settings.Default.UpScrollKey = (int) MacroProcess.GetHotkey(false);
            Properties.Settings.Default.FPS0Key = (int) Hotkeys.GetHotkeyByNumber(0);
            Properties.Settings.Default.FPS1Key = (int) Hotkeys.GetHotkeyByNumber(1);
            Properties.Settings.Default.FPS2Key = (int) Hotkeys.GetHotkeyByNumber(2);
            if(WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = Location;
            else if(WindowState == FormWindowState.Minimized) Properties.Settings.Default.Location = RestoreBounds.Location;

            Properties.Settings.Default.Save();
        }
        #endregion

        /// <summary>
        /// Updates all hotkey selection fields with their respective current hotkeys.
        /// </summary>
        public void UpdateHotkeyFields() {
            foreach(Label l in HotkeyFields) {
                Keys key = Keys.None;
                switch(l.Tag) {
                    case "macroDown":
                        key = MacroProcess.GetHotkey(true);
                        break;
                    case "macroUp":
                        key = MacroProcess.GetHotkey(false);
                        break;
                    case "fps0":
                        key = Hotkeys.GetHotkeyByNumber(0);
                        break;
                    case "fps1":
                        key = Hotkeys.GetHotkeyByNumber(1);
                        break;
                    case "fps2":
                        key = Hotkeys.GetHotkeyByNumber(2);
                        break;
                }
                l.Text = HotkeyHandler.TranslateKeyNames(key);
                l.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
        }

        /// <summary>
        /// Sets <c>com_adaptiveTickMaxHz</c> to the desired cap value. Sets to <c>250</c> if already at the desired cap.
        /// </summary>
        /// <param name="fpsHotkey">Which hotkey to trigger</param>
        public void ToggleFPSCap(int fpsHotkey) {
            switch(fpsHotkey) {
                case 0:
                    // toggleFPS
                    break;
                case 1:
                    // toggleFPS1
                    break;
                case 2:
                    // toggleFPS2
                    break;
                default:
                    return;
            }
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
            List<string> SteamLibraryDrives = new List<string>();
            string steamPath, vdfPath;
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Valve\\");

            // Finding every Steam Library
            using(RegistryKey subKey = key.OpenSubKey("Steam")) {
                try {
                    steamPath = subKey.GetValue("InstallPath").ToString();
                }catch(Exception) {
                    Debug.WriteLine("Couldn't find Steam install path! Does it even exist?");
                    // TODO - Popup window to manually select game dir
                    return;
                }

                vdfPath = steamPath + @"\steamapps\libraryfolders.vdf";
                string driveRegex = @"[A-Z]:\\";
                if(File.Exists(vdfPath)) {
                    string[] vdfLines = File.ReadAllLines(vdfPath);

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
            }

            // Finding which library contains the game
            foreach(string dir in SteamLibraryDrives) {
                if(dir != string.Empty) {
                    if(Directory.Exists(dir + "DOOMEternal")) {
                        var exe = dir + "DOOMEternal\\DOOMEternalx64vk.exe";
                        if(File.Exists(exe)) {
                            Debug.WriteLine("Found Game! - " + exe);
                            SteamDirectory = dir;
                            GameDirectory = dir + "DOOMEternal\\";
                            break;
                        }
                    }
                    Debug.WriteLine(dir);
                }
            }

            // Detects any extra game dirs in the same library for downpatching purposes
            // Downpatch folders MUST be in the format "DOOMEternal <versionString>"
            // List of valid version strings can be found in MemoryHandler.IsValidVersionString(); will have an info button with this list in the program window
            if(SteamDirectory != string.Empty) {
                string[] gameDirs = Directory.GetDirectories(SteamDirectory);
                foreach(var dir in gameDirs) {
                    if(!dir.Contains("DOOMEternal")) continue;

                    var subDir = dir.Substring(dir.IndexOf("DOOMEternal"));
                    if(!subDir.StartsWith("DOOMEternal ")) continue;
                    Debug.WriteLine(subDir);

                    var ver = dir.Substring(dir.IndexOf(' ') + 1);
                    if(MemoryHandler.IsValidVersionString(ver)) File.WriteAllText(dir + "\\gameVersion.txt", "version=" + ver);
                }
            }
        }
        
        // Hooks into the DOOMEternalx64vk.exe process, then sets up pointers for memory reading/writing.
        private bool Hook() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
            if(procList.Count == 0) {
                GameProcess = null;
                return false;
            }
            GameProcess = procList[0];

            if(GameProcess.HasExited) return false;

            try {
                Memory = new MemoryHandler(GameProcess);
                SetGameInfoByModuleSize();
                return true;
            }catch (Win32Exception ex) {
                Debug.WriteLine(ex.ErrorCode);
                return false;
            }
        }

        // Sets various game info variables based on the detected module size.
        private void SetGameInfoByModuleSize() {
            gameVersion.Text = Memory.VersionString();
            if(GameDirectory != string.Empty) {
                File.WriteAllText(GameDirectory + "\\gameVersion.txt", "version=" + Memory.VersionString());
            }
        }
    }
}