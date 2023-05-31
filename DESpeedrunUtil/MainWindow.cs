using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using DESpeedrunUtil.Util;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using Microsoft.Win32;
using Serilog;
using System.Diagnostics;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using static DESpeedrunUtil.Define.Constants;
using static DESpeedrunUtil.Define.Structs;
using static DESpeedrunUtil.Interop.DLLImports;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil {
    public partial class MainWindow: Form {

        public static MainWindow Instance { get; private set; }

        internal SettingsPage? SettingsWindow;

        private PrivateFontCollection _fonts = new();
        public static Font EternalUIRegular, EternalUIRegular10, EternalUIRegular20, EternalUIRegular32, EternalUIBold, EternalUIBold20, EternalLogoBold, EternalBattleBold;


        Process? _gameProcess;
        List<int> _ghostProcIDs = new();
        List<int> _ghostIDsChecked = new();
        public bool Hooked = false;
        bool _duplicateProcesses = false, _firstRun = true, _justLaunched = true;
        bool _gameInFocus = false;

        bool _mouseDown;
        Point _lastLocation;

        bool _fwRuleExists = false, _fwRestart = false;
        bool _mhExists = false, _mhInstallation = false;
        bool _reshadeExists = false;

        bool _enableMacro = true, _startingMacro = false;

        int _fpsDefault, _minResPercent, _targetFPS,
            _resPercent0, _resPercent1, _resPercent2, _resPercent3;

        MemoryHandler? _memory;

        Timer _formTimer, _statusTimer, _settingsTimer;

        bool _hkAssignmentMode = false, _mouse1Pressed = false;
        Label _selectedHKField = null;

        List<Label> _hotkeyFields;
        List<TextBox> _fpsLimitFields;
        List<Process> _openProcesses;

        DESRUShadowLabel _moreHotkeysLabel;
        Speedometer _speedometer;

        bool _smallDisplay = false;

        string _gameDirectory = "", _steamDirectory = "", _steamInstallation = "", _steamID3 = "", _rtssExecutable = "";
        List<string>? _gameVersions, _extraGameDirectories;

        bool _trackScroll = false, _displayPattern = false, _direction = false, _directionChanged = false;
        string _storedDisplay = "";
        long _scrollTime, _scrollDisplayTime;
        ScrollPattern _scrollPattern = new();

        public MainWindow() {
            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, this.Handle);
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, this.Handle);

            InitializeComponent();
            _hotkeyFields = new();
            _fpsLimitFields = new();
            _openProcesses = new();
            CollectHotkeyAndLimitFields(this);

            _formTimer = new Timer();
            _formTimer.Interval = Program.TimerInterval;
            _formTimer.Tick += (sender, e) => { UpdateTick(); };

            _statusTimer = new Timer();
            _statusTimer.Interval = 1000;
            _statusTimer.Tick += (sender, e) => { StatusTick(); };

            _settingsTimer = new Timer();
            _settingsTimer.Interval = 300000;
            _settingsTimer.Tick += (sender, e) => { SaveSettings(); };

            desruVersionLabel.Text = Program.APP_VERSION;

            SetToolTips();
            RemoveTabStop(this);

            Instance = this;
        }

        /// <summary>
        /// Windows Message handler for RawInput handling
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
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
                    } else {
                        var rawButton = mouse.Mouse.Buttons;
                        MouseButtons button;
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
                            if(HotkeyHandler.Instance.HookedHotkeys.Contains(HotkeyHandler.ConvertMouseButton(button))) {
                                var mea = new MouseEventArgs(button, 1, 0, 0, 0);
                                if(down) {
                                    RIMouseDown?.Invoke(this, mea);
                                } else {
                                    RIMouseUp?.Invoke(this, mea);
                                }
                            }
                        }
                    }
                } else if(data is RawInputKeyboardData keyboard) {
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
                    if(HotkeyHandler.Instance.HookedHotkeys.Contains(key)) {
                        var kea = new KeyEventArgs(key);
                        if(!up) {
                            RIKeyDown?.Invoke(this, kea);
                        } else {
                            RIKeyUp?.Invoke(this, kea);
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        // Main timer method that runs this utility's logic.
        private void UpdateTick() {
            if(_gameProcess == null || _gameProcess.HasExited) DisposeGameProcess();

            speedometerPrecisionCheckbox.Visible = speedometerCheckbox.Checked;
            rightAlignCheckbox.Visible = speedometerCheckbox.Checked;
            hideDuringLoadsCheckbox.Visible = speedometerCheckbox.Checked;
            trainerRadioLabel.Visible = speedometerCheckbox.Checked;
            velocityRadioNone.Visible = speedometerCheckbox.Checked;
            velocityRadioTotal.Visible = speedometerCheckbox.Checked;
            velocityRadioVertical.Visible = speedometerCheckbox.Checked;

            if(!_fwRestart) firewallRestartLabel.ForeColor = COLOR_PANEL_BACK;
            firewallToggleButton.Text = _fwRuleExists ? "Disable Firewall Rule" : "Enable Firewall Rule";

            CheckForMeathook();

            if(!Hooked) {
                try {
                    Hooked = Hook();
                } catch(Exception e) {
                    Log.Error(e, "An error occured when attempting to hook into the game.");
                    return;
                }
                if(Hooked && _memory == null) {
                    _gameProcess = null;
                    return;
                }
            }
            if(!Hooked) {
                if(_gameDirectory.ToLower().Contains("steam")) versionDropDownSelector.Enabled = true;
                FreescrollMacro.Instance.Stop(true);
                _fwRestart = false;
                if(!_mhInstallation) {
                    cheatsToggleButton.Enabled = true;
                    cheatsToggleButton.Text = _mhExists ? "Uninstall Meath00k" : "Install Meath00k";
                }
                return;
            }

            /** Toggling Hotkeys/Macro when game changes focus **/
            try {
                _gameInFocus = CheckIfGameIsInFocus();
                if(enableHotkeysCheckbox.Checked) {
                    if(!_memory.GetFlag("unlockscheduled")) {
                        if(!_gameInFocus) {
                            HotkeyHandler.Instance.DisableHotkeys();
                        } else {
                            HotkeyHandler.Instance.EnableHotkeys();
                        }
                    }
                }
                if(!_gameInFocus) {
                    FreescrollMacro.Instance.Stop(true);
                    _startingMacro = false;
                } else {
                    if(_enableMacro) {
                        if(_openProcesses.FindAll(x => x.ProcessName.Contains("DOOMEternalMacro")).Count > 1) {
                            FreescrollMacro.Instance.Restart();
                            _openProcesses.Clear();
                        }
                        if(!FreescrollMacro.Instance.IsRunning && !_startingMacro) {
                            FreescrollMacro.Instance.Start();
                            _startingMacro = true;
                        } else if(FreescrollMacro.Instance.IsRunning && _startingMacro) {
                            _startingMacro = false;
                        }
                    }
                }
            } catch(Exception e) {
                Log.Error(e, "Failed to check if DOOM Eternal was in focus.");
            }

            if(!_enableMacro) FreescrollMacro.Instance.Stop(true);

            /** Memory Flags **/
            var v = _memory.Version.Name.Contains("Unknown");
            if(!_fwRuleExists) _memory.SetFlag(false, "firewall");
            _memory.SetFlag(_enableMacro && FreescrollMacro.Instance.HasKeyBound(), "macro");
            _memory.SetFlag(!v && enableMaxFPSCheckbox.Checked, "limiter");
            _memory.SetFlag(trainerOSDCheckbox.Checked, "trainer");

            enableMaxFPSCheckbox.Enabled = !v;
            if(!v) {
                if(_memory.GetFlag("resunlocked") && !_memory.GetFlag("unlockscheduled")) {
                    unlockResButton.Enabled = true;
                    unlockResButton.Text = "Update Minimum Resolution";
                } else if(!_memory.GetFlag("resunlocked") && !_memory.GetFlag("unlockscheduled")) {
                    unlockResButton.Enabled = true;
                    unlockResButton.Text = "Unlock Resolution Scaling";
                }
                if(_memory.GetFlag("unlockscheduled")) {
                    unlockResButton.Enabled = false;
                    unlockResButton.Text = "Unlock in Progress";
                }
            } else {
                unlockResButton.Enabled = false;
                unlockResButton.Text = "Version Unsupported";
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
                    } else {
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

            /** Trainer **/
            var (velX, velY, velZ, hVel, totalVel) = _memory.GetPlayerVelocity();
            bool showDuringLoads = !_memory.IsLoadingOrInMenu() || !hideDuringLoadsCheckbox.Checked;
            if(_memory.GetFlag("meath00k") || _memory.EnableCheats) {
                trainerOSDCheckbox.Enabled = true;
                if(_memory.TrainerSupported) {
                    var (posX, posY, posZ, yaw, pitch) = _memory.GetPlayerPosition();

                    if(!speedometerCheckbox.Checked) {
                        positionTextBox.Text = string.Format(TEXTBOX_POSITION_TEXT, posX, posY, posZ, yaw, pitch);
                        velocityTextBox.Text = string.Format(TEXTBOX_VELOCITY_TEXT, velX, velY, velZ, hVel, totalVel);

                        ToggleTrainerControls(true);
                    } else {
                        ToggleTrainerControls(false);

                        altPositionTextbox.Text = string.Format(TEXTBOX_ALT_TEXT_POS, posX, posY, posZ, yaw, pitch);

                        _speedometer.Visible = showDuringLoads;

                        if(_speedometer.Visible) {
                            _speedometer.VerticalVelocity = velZ;
                            _speedometer.HorizontalVelocity = hVel;
                            _speedometer.TotalVelocity = totalVel;
                            _speedometer.ShowVerticalVelocity = velocityRadioVertical.Checked;
                            _speedometer.IncreasedPrecision = speedometerPrecisionCheckbox.Checked;
                            _speedometer.HideSecondaryVelocity = velocityRadioNone.Checked;
                            _speedometer.RightAlignText = rightAlignCheckbox.Checked;
                            _speedometer.Refresh();
                        }
                    }
                } else {
                    positionTextBox.Visible = false;
                    velocityTextBox.Visible = false;
                    _speedometer.Visible = false;

                    altPositionTextbox.Text = TRAINER_UNSUPPORTED_WARNING;
                    altPositionTextbox.Visible = true;
                }
            } else {
                trainerOSDCheckbox.Enabled = false;
                positionTextBox.Visible = false;
                velocityTextBox.Visible = false;
                _speedometer.Visible = speedometerCheckbox.Checked && showDuringLoads;

                altPositionTextbox.Text = (speedometerCheckbox.Checked) ? string.Empty : TRAINER_NOCHEATS_WARNING;
                altPositionTextbox.Visible = true;

                if(_speedometer.Visible) {
                    _speedometer.VerticalVelocity = velZ;
                    _speedometer.HorizontalVelocity = hVel;
                    _speedometer.TotalVelocity = totalVel;
                    _speedometer.ShowVerticalVelocity = velocityRadioVertical.Checked;
                    _speedometer.IncreasedPrecision = speedometerPrecisionCheckbox.Checked;
                    _speedometer.HideSecondaryVelocity = velocityRadioNone.Checked;
                    _speedometer.RightAlignText = rightAlignCheckbox.Checked;
                    _speedometer.Refresh();
                }
            }
        }

        // Timer method used to update the info panel on the main form
        private void StatusTick() {
            _openProcesses = Process.GetProcesses().ToList();
            var rtss = _openProcesses.FindAll(p => p.ProcessName.ToLower().Contains("rtss")).Count > 0;
            if(!rtss && launchRTSSCheckbox.Enabled && launchRTSSCheckbox.Checked) LaunchRTSS();

            // Checking for the Firewall rule can take upwards of 8ms
            // so it was moved out of the main timer and into this one since the interval is longer, for better performance
            try {
                if(Hooked && !_gameProcess.MainModule.FileName.Contains("WindowsApps")) {
                    _fwRuleExists = FirewallHandler.CheckForFirewallRule(_gameProcess.MainModule.FileName, false);
                } else {
                    _fwRuleExists = FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", false);
                }
            } catch(Exception e) {
                Log.Error(e, "Failed to check if firewall rule exists.");
            }

            macroStatus.Text = (FreescrollMacro.Instance.IsRunning) ? "Running" : "Stopped";
            macroStatus.ForeColor = (FreescrollMacro.Instance.IsRunning) ? Color.Lime : COLOR_TEXT_FORE;
            hotkeyStatus.Text = (HotkeyHandler.Instance.Enabled) ? "Enabled" : "Disabled";
            rtssStatus.Text = (_openProcesses.FindAll(p => p.ProcessName.ToLower().Contains("rtss")).Count > 0) ? "Running" : "Closed";

            if(_memory != null) {
                var hz = _memory.ReadMaxHz();
                var v = _memory.Version.Name.Replace("(Gamepass)", "(GP)");
                gameStatus.Text = v;
                if(v == "1.0 (Release)") {
                    slopeboostStatus.Text = (_memory.GetFlag("slopeboost")) ? "Disabled" : "Enabled";
                } else {
                    slopeboostStatus.Text = "N/A";
                }
                if(hz == -1) {
                    currentFPSCap.ForeColor = Color.Red;
                    currentFPSCap.Text = "UNSUPPORTED";
                    toolTip7500.SetToolTip(currentFPSCap, "This version of DOOM Eternal is not fully supported by DESRU.\n" +
                        "You will need to cap your FPS to 250 through another method, like RTSS or the NVIDIA Control Panel.\n" +
                        "Please contact bowsr in the MDSR Discord with the version of the game you're playing, and what platform you're playing on (Steam, Gamepass).");
                } else {
                    currentFPSCap.ForeColor = COLOR_TEXT_FORE;
                    currentFPSCap.Text = hz.ToString();
                    toolTip7500.SetToolTip(currentFPSCap, null);
                }
                var ms = _memory.ReadRaiseMillis();
                if(ms > 0 && ms < 16) {
                    var rs = string.Format("{0:0.00}% (" + (_memory.ReadDynamicRes() ? _memory.GetTargetFPS() + "FPS)" : "Static)"), _memory.CurrentResScaling);
                    if(rs.StartsWith("0.0")) rs = rs[3..];
                    else if(rs.StartsWith("0.")) rs = rs[2..];
                    resScaleStatus.Text = (_memory.ReadDynamicRes() || _memory.ReadForceRes() > 0f) ? rs.Replace(".", "") : "Disabled";
                    toolTip7500.SetToolTip(resScaleStatus, null);
                } else {
                    if(v.Contains("Unknown")) {
                        resScaleStatus.Text = "UNSUPPORTED";
                        toolTip7500.SetToolTip(resScaleStatus, "This version of DOOM Eternal is not fully supported by DESRU.\n" +
                            "Please contact bowsr in the MDSR Discord with the version of the game you're playing, and what platform you're playing on (Steam, Gamepass).");
                    } else {
                        resScaleStatus.Text = "N/A";
                        toolTip7500.SetToolTip(resScaleStatus, null);
                    }
                }
            } else {
                slopeboostStatus.Text = "-";
                currentFPSCap.Text = "-";
                currentFPSCap.ForeColor = COLOR_TEXT_FORE;
                toolTip7500.SetToolTip(currentFPSCap, null);
                resScaleStatus.Text = "-";
                toolTip7500.SetToolTip(resScaleStatus, null);
                gameStatus.Text = "Not Running";
            }

            balanceStatus.Text = (_fwRuleExists) ? ((_fwRestart) ? "Allowed*" : "Blocked") : "Allowed";
            if(Hooked) {
                if(_memory.GetFlag("meath00k")) {
                    cheatsStatus.Text = "meath00k";
                    cheatsStatus.ForeColor = Color.Orange;
                } else if(_memory.EnableCheats) {
                    cheatsStatus.Text = "Enabled";
                    cheatsStatus.ForeColor = Color.Red;
                } else {
                    cheatsStatus.Text = "Disabled";
                    cheatsStatus.ForeColor = COLOR_TEXT_FORE;
                }
                if(_reshadeExists) {
                    reshadeStatus.Text = "Enabled";
                } else {
                    reshadeStatus.Text = "Disabled";
                }
            } else {
                if(_mhExists) {
                    cheatsStatus.Text = "meath00k";
                    cheatsStatus.ForeColor = Color.Orange;
                } else {
                    cheatsStatus.Text = "-";
                    cheatsStatus.ForeColor = COLOR_TEXT_FORE;
                }
                reshadeStatus.Text = "-";
            }
        }

        private bool CheckIfGameIsInFocus() {
            try {
                return _gameProcess.MainWindowHandle == GetForegroundWindow();
            } catch(Exception e) {
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
                    "hkMacroDown" => FreescrollMacro.Instance.GetHotkey(true),
                    "hkMacroUp" => FreescrollMacro.Instance.GetHotkey(false),
                    "hkResToggle" => HotkeyHandler.Instance.ResScaleHotkey,
                    "hkResToggle0" => HotkeyHandler.Instance.ResToggleHotkey0,
                    "hkResToggle1" => HotkeyHandler.Instance.ResToggleHotkey1,
                    "hkResToggle2" => HotkeyHandler.Instance.ResToggleHotkey2,
                    "hkResToggle3" => HotkeyHandler.Instance.ResToggleHotkey3,
                    _ => HotkeyHandler.Instance.FPSHotkeys.GetKeyFromID(int.TryParse(tag.Replace("hkFps", ""), out int id) ? id : -1),
                };
                l.Text = HotkeyHandler.TranslateKeyNames(key);
                l.ForeColor = COLOR_TEXT_FORE;
                l.BackColor = COLOR_TEXT_BACK;
            }
            foreach(TextBox t in _fpsLimitFields) {
                string tag = (string) t.Tag;
                int limit = HotkeyHandler.Instance.FPSHotkeys.GetLimitFromID(int.TryParse(tag.Replace("fpscap", ""), out int id) ? id : -1);
                t.Text = (limit != -1) ? limit.ToString() : "";
            }
            minResInput.Text = _minResPercent.ToString();
            resPercent0.Text = _resPercent0 != -1 ? _resPercent0.ToString() : string.Empty;
            resPercent1.Text = _resPercent1 != -1 ? _resPercent1.ToString() : string.Empty;
            resPercent2.Text = _resPercent2 != -1 ? _resPercent2.ToString() : string.Empty;
            resPercent3.Text = _resPercent3 != -1 ? _resPercent3.ToString() : string.Empty;
            targetFPSInput.Text = _targetFPS.ToString();
        }

        /// <summary>
        /// Populates the Version selector with all currently detected game versions
        /// </summary>
        public void PopulateVersionDropDown() {
            changeVersionButton.Enabled = false;
            versionDropDownSelector.Items.Clear();
            if(!_gameDirectory.ToLower().Contains("steam")) {
                versionDropDownSelector.Enabled = false;
                return;
            }
            for(int i = 0; i < _gameVersions.Count; i++) {
                string v = _gameVersions.ElementAt(i);
                versionDropDownSelector.Items.Add(v);
                if(v == GetCurrentVersion()) versionDropDownSelector.SelectedIndex = i;
            }
        }

        /// <summary>
        /// Retrieves the currently installed version by reading gameVersion.txt
        /// </summary>
        /// <returns>A <see langword="string"/> representing a game version</returns>
        public string GetCurrentVersion() {
            if(_gameDirectory != string.Empty) {
                try {
                    string s = File.ReadAllText(_gameDirectory + "\\gameVersion.txt").Trim();
                    return s[(s.IndexOf('=') + 1)..];
                } catch(Exception e) {
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
            _memory.SetMaxHz((current != fps) ? fps : (enableMaxFPSCheckbox.Checked) ? _fpsDefault : 1000);
        }

        public void ToggleResScaling() {
            if(Hooked) {
                _memory.ToggleResolutionScaling();
            }
        }

        public void ChangeResScale(int id) {
            var percent = id switch {
                0 => _resPercent0 / 100f,
                1 => _resPercent1 / 100f,
                2 => _resPercent2 / 100f,
                3 => _resPercent3 / 100f,
                _ => -1
            };
            if(!Hooked || percent == -1) return;
            targetFPSInput.Text = _targetFPS.ToString();
            float current = _memory.GetMinRes();
            if(percent != current) {
                _memory.SetMinRes(percent);
            } else {
                if(_memory.ReadDynamicRes() || _memory.ReadForceRes() > 0f) _memory.SetMinRes(_minResPercent / 100f);
            }
            _memory.ScheduleResUnlock(false, _targetFPS);
            _memory.EnableResolutionScaling();
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
            if(!File.Exists(_gameDirectory + "\\DOOMEternalx64vk.exe")) _gameDirectory = string.Empty;
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
                    gameSelection.Dispose();
                    return;
                }

                vdfLocation = steamPath + @"\steamapps\libraryfolders.vdf";
            } else {
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
                            _gameDirectory = dir + "DOOMEternal";
                            break;
                        }
                    }
                }
            }
            if(_gameDirectory == string.Empty) {
                var gpDir = @"C:\XboxGames\Doom Eternal - PC\Content";
                if(File.Exists(gpDir + "\\DOOMEternalx64vk.exe")) {
                    _gameDirectory = gpDir;
                    Log.Information("Found Gamepass installation.");
                } else {
                    Log.Error("Couldn't find the game installation!");
                    using GameDirectoryDialog gameSelection = new();
                    if(gameSelection.ShowDialog() == DialogResult.OK) {
                        _gameDirectory = gameSelection.FileName.Remove(gameSelection.FileName.IndexOf("\\DOOMEternalx64vk.exe"));
                        Log.Information("User manually selected their game directory.");
                    } else {
                        Log.CloseAndFlush();
                        this.Close();
                    }
                    gameSelection.Dispose();
                }
            }
            if(_gameDirectory.ToLower().Contains("steam")) _steamDirectory = _gameDirectory.Remove(_gameDirectory.IndexOf("\\DOOMEternal"));
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

                    var subDir = dir[dir.IndexOf("DOOMEternal")..];
                    if(!subDir.StartsWith("DOOMEternal ")) continue;

                    var ver = subDir[(subDir.IndexOf(' ') + 1)..];
                    if(GameVersion.IsValidVersionString(ver)) File.WriteAllText(dir + "\\gameVersion.txt", "version=" + ver);
                    Log.Information("Found extra game installation; Version: {Version}", ver);
                }
            }

            // Once all the gameVersion.txt files are in place, this populates the dropdown selector for version swapping
            _gameVersions = new();
            if(directories.Count > 0) {
                foreach(var dir in directories) {
                    try {
                        string txt = File.ReadAllText(dir + "\\gameVersion.txt").Trim();
                        string v = txt[(txt.IndexOf('=') + 1)..];
                        if(GameVersion.IsValidVersionString(v)) _gameVersions.Add(v);
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
                    if(!File.Exists(_steamInstallation + "\\userdata\\" + profile + PATH_PROFILE_DIR + PATH_PROFILE_FILE)) id3s.Remove(profile);
                }
            }
            if(id3s.Count == 1) {
                _steamID3 = id3s[0];
                Log.Information("DE saves folder detected. steamID3: {ID3}", _steamID3);
                return;
            } else if(id3s.Count > 1) {
                Log.Warning("Multiple Steam userdata folders with DOOMEternal data detected. Prompting user to select the correct profile.");
                id3s.Sort();
                UserdataDialog dialog = new(id3s);
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    _steamID3 = dialog.ID3;
                    Log.Information("User selected steamID3: {ID3}", _steamID3);
                } else {
                    Log.CloseAndFlush();
                    this.Close();
                }
            }
        }
        #endregion

        private bool CheckForMeathook() {
            bool mh = File.Exists(_gameDirectory + "\\XINPUT1_3.dll");
            _mhExists = mh;
            return mh;
        }
        private bool CheckForMeathookInPath(string path) => File.Exists(path + "\\XINPUT1_3.dll");

        // Checks if ReShade is both installed for Vulkan and set to run over DOOMEternalx64vk
        private bool CheckForReShade() {
            using RegistryKey? localMachineKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Khronos\Vulkan\ImplicitLayers\");
            string[] names = localMachineKey?.GetValueNames() ?? Array.Empty<string>();
            var foundLayer = false;
            foreach(string s in names) {
                if(s.ToLower().Contains("reshade")) {
                    foundLayer = true;
                    Log.Information("ReShade Vulkan Layer found in Local Machine registry.");
                }
            }
            if(!foundLayer) {
                using RegistryKey? currentUserKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Khronos\Vulkan\ImplicitLayers\");
                names = currentUserKey?.GetValueNames() ?? Array.Empty<string>();
                foreach(string s in names) {
                    if(s.ToLower().Contains("reshade")) {
                        foundLayer = true;
                        Log.Information("ReShade Vulkan Layer found in Current User registry.");
                    }
                }
            }
            if(foundLayer) {
                try {
                    var rs = File.ReadAllText(@"C:\ProgramData\ReShade\ReShadeApps.ini").ToLower().Contains(_gameDirectory.ToLower());
                    if(rs) Log.Information("ReShade is installed and is running over DOOMEternal.");
                    else Log.Information("ReShade is not running over DOOMEternal.");
                    return rs;
                } catch(Exception e) {
                    Log.Error(e, "An error occured when checking ReShade files. Assuming ReShade is running over DOOMEternal.");
                    return true;
                }
            }
            Log.Information("ReShade Vulkan Layer not found in registry.");
            return false;
        }

        private bool DetectRTSSExecutable() {
            using RegistryKey? rtssKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\RTSS");
            string uninstallStr = rtssKey?.GetValue("UninstallString").ToString() ?? "";
            if(uninstallStr != string.Empty) _rtssExecutable = uninstallStr[1..uninstallStr.LastIndexOf('\\')] + @"\RTSS.exe";
            if(!File.Exists(_rtssExecutable)) _rtssExecutable = "";
            return _rtssExecutable != string.Empty;
        }

        private void InstallMeathook() {
            _mhInstallation = true;
            cheatsToggleButton.Enabled = false;
            cheatsToggleButton.Text = "Installing...";
            try {
                File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + "\\XINPUT1_3.dll");
            } catch(Exception ex) {
                Log.Error(ex, "An error occured when attempting to install meath00k in directory: {Directory}", _gameDirectory);
            }
            foreach(string v in _gameVersions) {
                try {
                    File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + " " + v + "\\XINPUT1_3.dll");
                } catch(Exception ex) {
                    Log.Error(ex, "An error occured when attempting to install meath00k in directory: {Directory}", _gameDirectory + " " + v);
                    continue;
                }
            }
            foreach(string dir in _extraGameDirectories) {
                try {
                    File.Copy(@".\meath00k\XINPUT1_3.dll", dir + "\\XINPUT1_3.dll");
                } catch(Exception ex) {
                    Log.Error(ex, "An error occured when attempting to install meath00k in directory: {Directory}", dir);
                    continue;
                }
            }
            Log.Information("meath00k installed.");
            _mhInstallation = false;
        }

        private void UninstallMeathook() {
            _mhInstallation = true;
            cheatsToggleButton.Enabled = false;
            cheatsToggleButton.Text = "Uninstalling...";
            try {
                File.Delete(_gameDirectory + "\\XINPUT1_3.dll");
            } catch(Exception e) {
                Log.Error(e, "An error occurred when attempting to uninstall meath00k from directory: {Directory}", _gameDirectory);
            }

            foreach(string v in _gameVersions) {
                try {
                    File.Delete(_gameDirectory + " " + v + "\\XINPUT1_3.dll");
                } catch(Exception e) {
                    Log.Error(e, "An error occured when attempting to uninstall meath00k from directory: {Directory}", _gameDirectory + " " + v);
                    continue;
                }
            }
            foreach(string dir in _extraGameDirectories) {
                try {
                    File.Delete(dir + "\\XINPUT1_3.dll");
                } catch(Exception e) {
                    Log.Error(e, "An error occured when attempting to uninstall meath00k from directory: {Directory}", dir);
                    continue;
                }
            }
            Log.Information("meath00k uninstalled.");
            _mhInstallation = false;
        }

        // Hooks into the DOOMEternalx64vk.exe process, then sets up pointers for memory reading/writing.
        private bool Hook() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
            if(procList.Count == 0) {
                DisposeGameProcess();
                _duplicateProcesses = false;
                _firstRun = false;
                _ghostProcIDs.Clear();
                _ghostIDsChecked.Clear();
                return false;
            }

            if(procList.Count > 1) Log.Warning("Found {Count} instances of DOOMEternalx64vk", procList.Count);

            // Running through every matched process to check if they're valid
            bool multipleProcesses = false;
            foreach(var gameProc in procList) {
                //if(_ghostProcIDs.Contains(gameProc.Id)) {
                //    // Skipping already checked ghost processes to prevent slowdowns and log file size ballooning
                //    if(_ghostIDsChecked.Contains(gameProc.Id)) continue;
                //    Log.Information("Skipping process with id {ID} as it was already checked", gameProc.Id);
                //    _ghostIDsChecked.Add(gameProc.Id);
                //    continue;
                //}
                if(!gameProc.HasExited) {
                    try {
                        // Attempts to check the MainModuleMemorySize if a process is found that hasn't exited
                        Log.Information("Potential game candidate. id={ID} moduleSize={ModuleSize}", gameProc.Id, gameProc.MainModule?.ModuleMemorySize);
                    } catch(Exception e) {
                        // If the ModuleSize check fails, adds process ID to list of ghost processes as it's not a valid process to hook in to
                        Log.Error(e, "Failed to check ModuleSize of game process. id={ID}", gameProc.Id);
                        //if(!_ghostProcIDs.Contains(gameProc.Id)) _ghostProcIDs.Add(gameProc.Id);
                        continue;
                    }
                    if(_gameProcess != null) {
                        // If a valid process was already found, trigger the duplicate process warning telling the user to clear them out
                        Log.Error("Found multiple valid DOOMEternalx64vk processes! Cannot properly hook into the game");
                        _gameProcess = null;
                        multipleProcesses = true;
                        break;
                    }
                    _gameProcess = gameProc;
                    continue;
                }
                Log.Information("DOOMEternalx64vk process has exited. id={ID}", gameProc.Id);
            }

            // Error catching/logging in the event of multiple valid game processes being detected
            if(multipleProcesses) {
                if(!_duplicateProcesses) {
                    _duplicateProcesses = true;
                    Log.Error("Multiple DOOM Eternal processes detected!");
                    for(int i = 0; i < procList.Count; i++) {
                        var proc = procList[i];
                        Log.Information("procList[{Index}]:", i);
                        Log.Information(" * ID = {ID}", proc.Id);
                        Log.Information(" * Name = {Name}", proc.ProcessName);
                        Log.Information(" * HasExited = {HasExited}", proc.HasExited);
                        try {
                            Log.Information(" * Path = {Path}", proc.MainModule?.FileName);
                            Log.Information(" * MainModuleMemorySize = {MainModuleSize}", proc.MainModule?.ModuleMemorySize);
                            Log.Information(" * {ModuleCount} Loaded Modules:", proc.Modules.Count);
                            for(int j = 1; j <= proc.Modules.Count; j++) {
                                Log.Information(" * {Index}) {ModuleName}", j, proc.Modules[j - 1].ModuleName);
                            }
                        } catch(Exception e) {
                            Log.Error(e, "Failed to log procList[" + i + "] modules.", e);
                        }
                        Log.Information(" ================================ ");
                    }
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show(this, "Multiple instances of DOOM Eternal have been detected.\n" +
                    "Close them or restart your system to clear them out.", "Multiple DOOMEternalx64vk.exe Instances Detected");
                }
                _gameProcess = null;
                return false;
            }

            changeVersionButton.Enabled = false;

            if(_gameProcess == null || _gameProcess.HasExited) {
                if(!_duplicateProcesses) _firstRun = false;
                return false;
            }
            _duplicateProcesses = false;
            _ghostProcIDs.Clear();
            _ghostIDsChecked.Clear();
            Log.Information("Starting to hook into the DOOMEternalx64vk.exe process. firstRun={FirstRun}", _firstRun);

            _reshadeExists = CheckForReShade();

            try {
                _memory = new MemoryHandler(_gameProcess);
            } catch(ArgumentNullException ex) {
                Log.Error(ex, "An error occured when attempting to hook into the game.");
                _gameProcess?.Dispose();
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(_memory == null) {
                Log.Error("MemoryHandler was somehow null. Retrying hook.");
                _gameProcess?.Dispose();
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(_memory.Reset) {
                Log.Error("Something went wrong when setting up the MemoryHandler. Retrying hook.");
                _gameProcess?.Dispose();
                _gameProcess = null;
                _memory = null;
                return false;
            }
            if(enableHotkeysCheckbox.Checked) {
                HotkeyHandler.Instance.EnableHotkeys();
                HotkeyHandler.Instance.ToggleResScaleKeys(false);
            }
            SetGameInfoByModuleSize();
            try {
                versionDropDownSelector.SelectedItem = GetCurrentVersion();
            } catch(Exception e) {
                Log.Error(e, "An error occured when attempting to change the version selector's index.");
            }
            versionDropDownSelector.Enabled = false;
            _memory.SetFlag(_fwRuleExists, "firewall");
            var gamePath = _gameProcess.MainModule.FileName;
            var gameDir = gamePath[..gamePath.LastIndexOf('\\')];
            bool meath00k = CheckForMeathookInPath(gameDir);
            foreach(ProcessModule module in _gameProcess.Modules) {
                if(module.ModuleName.ToLower().Contains("xinput1_3.dll") && module.FileName.ToLower().Contains(gameDir.ToLower()))
                    meath00k = true;
            }
            _memory.SetFlag(meath00k, "meath00k");
            cheatsToggleButton.Enabled = !meath00k;
            if(!meath00k) {
                cheatsToggleButton.Text = "Enable Cheats";
            }
            var checksum = Checksums.GetMD5ChecksumFromFile(gamePath);
            if(!Checksums.Compare(checksum, _memory.Version.MD5)) {
                _memory.SetFlag(true, "modded");
                Log.Warning("Potentially modified client detected. md5: {Checksum}", checksum);
            }
            _memory.SetFlag(_reshadeExists, "reshade");
            _memory.SetFlag(Program.UpdateDetected, "outofdate");
            _memory.SetFlag(_firstRun && !_memory.GetFlag("meath00k"), "restart");
            _memory.FirstRun = _firstRun;
            _memory.EnableOSD = enableOSDCheckbox.Checked;
            _memory.UseDynamicScaling = dynScalingRadioButton.Checked;
            _memory.SetFlag(!_memory.Version.Name.Contains("Unknown") && enableMaxFPSCheckbox.Checked, "limiter");
            _memory.SetFlag(minimalOSDCheckbox.Checked, "minimal");
            if(_memory.GetFlag("restart")) Log.Warning("Game requires a restart. Utility must be running before the game is launched.");
            _memory.SetMinRes(_minResPercent / 100f);
            if(unlockOnStartupCheckbox.Checked) {
                _memory.ScheduleResUnlock(autoScalingCheckbox.Checked, _targetFPS);
                HotkeyHandler.Instance.DisableHotkeys();
            } else {
                if(autoScalingCheckbox.Checked) _memory.ScheduleResolutionScaling(_targetFPS);
            }
            _memory.SetMaxHz((enableMaxFPSCheckbox.Checked) ? _fpsDefault : 1000);

            _memory.SetCVAR(Properties.Settings.Default.AntiAliasing, "antialiasing");
            _memory.SetCVAR(Properties.Settings.Default.UNDelay, "undelay");
            _memory.SetCVAR(Properties.Settings.Default.AutoContinue, "autocontinue");

            _memory.MemoryTimer.Start();
            _firstRun = false;
            SettingsWindow?.UpdateMemoryHandler(_memory);
            return true;
        }

        // Sets various game info variables based on the detected module size.
        private void SetGameInfoByModuleSize() {
            gameStatus.Text = _memory.Version.Name.Replace("(Gamepass)", "(GP)");
            var gamePath = _gameProcess.MainModule.FileName;
            var dir = gamePath[..gamePath.LastIndexOf('\\')];
            if(gamePath.ToLower().Contains("steam")) {
                File.WriteAllText(dir + "\\gameVersion.txt", "version=" + _memory.Version.Name);
            }

            if(!gamePath.Contains(_gameDirectory)) {
                if(!_extraGameDirectories.Contains(dir)) {
                    _extraGameDirectories.Add(dir);
                    FirewallHandler.CreateFirewallRule(gamePath, _extraGameDirectories.Count);
                }
            }

            if(!_memory.CanCapFPS()) HotkeyHandler.Instance.DisableHotkeys();
        }

        private void LaunchRTSS() => Process.Start(new ProcessStartInfo(_rtssExecutable) { WorkingDirectory = _rtssExecutable[.._rtssExecutable.LastIndexOf('\\')] });

        private void DisposeGameProcess() {
            Hooked = false;
            HotkeyHandler.Instance.DisableHotkeys();
            _gameProcess?.Dispose();
            _gameProcess = null;
            _memory?.MemoryTimer.Stop();
            _memory = null;
            _gameInFocus = false;

            unlockResButton.Enabled = false;
            unlockResButton.Text = "Game Not Running";

            enableMaxFPSCheckbox.Enabled = true;

            HideTrainerControls();
        }

        private void SaveSettings() {
            Properties.Settings.Default.DownScrollKey = (int) FreescrollMacro.Instance.GetHotkey(true);
            Properties.Settings.Default.UpScrollKey = (int) FreescrollMacro.Instance.GetHotkey(false);
            Properties.Settings.Default.DefaultFPSCap = _fpsDefault;
            Properties.Settings.Default.MacroEnabled = autorunMacroCheckbox.Checked;
            Properties.Settings.Default.HotkeysEnabled = enableHotkeysCheckbox.Checked;
            Properties.Settings.Default.GameLocation = _gameDirectory;
            Properties.Settings.Default.StartupUnlock = unlockOnStartupCheckbox.Checked;
            Properties.Settings.Default.AutoDynamic = autoScalingCheckbox.Checked;
            Properties.Settings.Default.MinResPercent = _minResPercent;
            Properties.Settings.Default.TargetFPSScaling = _targetFPS;
            Properties.Settings.Default.SteamInstallation = _steamInstallation;
            Properties.Settings.Default.SteamID3 = _steamID3;
            Properties.Settings.Default.ReplaceProfile = replaceProfileCheckbox.Checked;
            Properties.Settings.Default.ResScaleKey = (int) HotkeyHandler.Instance.ResScaleHotkey;
            Properties.Settings.Default.EnableMaxFPSLimit = enableMaxFPSCheckbox.Checked;
            Properties.Settings.Default.RTSSPath = _rtssExecutable;
            Properties.Settings.Default.LaunchRTSS = launchRTSSCheckbox.Checked;
            Properties.Settings.Default.MinimalOSD = minimalOSDCheckbox.Checked;
            Properties.Settings.Default.TrainerOSD = trainerOSDCheckbox.Checked;
            Properties.Settings.Default.ShowSpeedometer = speedometerCheckbox.Checked;
            Properties.Settings.Default.IncreasedPrecision = speedometerPrecisionCheckbox.Checked;
            Properties.Settings.Default.ResToggleKey0 = (int) HotkeyHandler.Instance.ResToggleHotkey0;
            Properties.Settings.Default.ResToggleKey1 = (int) HotkeyHandler.Instance.ResToggleHotkey1;
            Properties.Settings.Default.ResToggleKey2 = (int) HotkeyHandler.Instance.ResToggleHotkey2;
            Properties.Settings.Default.ResToggleKey3 = (int) HotkeyHandler.Instance.ResToggleHotkey3;
            Properties.Settings.Default.ResTogglePercent0 = _resPercent0;
            Properties.Settings.Default.ResTogglePercent1 = _resPercent1;
            Properties.Settings.Default.ResTogglePercent2 = _resPercent2;
            Properties.Settings.Default.ResTogglePercent3 = _resPercent3;
            Properties.Settings.Default.UseDynamicScaling = dynScalingRadioButton.Checked;
            Properties.Settings.Default.RightAlignSpeedometer = rightAlignCheckbox.Checked;
            int radio = 1;
            if(velocityRadioNone.Checked) radio = 0;
            if(velocityRadioVertical.Checked) radio = 2;
            Properties.Settings.Default.SecondaryVelocity = radio;
            if(WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = Location;
            else if(WindowState == FormWindowState.Minimized) Properties.Settings.Default.Location = RestoreBounds.Location;
            var dirs = "";
            foreach(string d in _extraGameDirectories) dirs += d + "|";
            Properties.Settings.Default.ExtraGameDirectories = dirs;
            Properties.Settings.Default.Save();
            Log.Information("Saved user settings");
        }

        private void InitializeFonts() {
            foreach(string file in Directory.GetFiles(@".\fonts")) _fonts.AddFontFile(file);
            foreach(FontFamily ff in _fonts.Families) {
                switch(ff.Name) {
                    case "Eternal UI 2":
                        EternalUIRegular = new(ff, 11.25f, FontStyle.Regular, GraphicsUnit.Point);
                        EternalUIRegular10 = new(ff, 9.75f, FontStyle.Regular, GraphicsUnit.Point);
                        EternalUIRegular20 = new(ff, 20f, FontStyle.Regular, GraphicsUnit.Point);
                        EternalUIRegular32 = new(ff, 32f, FontStyle.Regular, GraphicsUnit.Point);
                        EternalUIBold = new(ff, 11.25f, FontStyle.Bold, GraphicsUnit.Point);
                        EternalUIBold20 = new(ff, 20f, FontStyle.Bold, GraphicsUnit.Point);
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
                    } else if(tag.StartsWith("fpscap")) {
                        c.KeyPress += new KeyPressEventHandler(Input_KeyPressNumericOnly);
                        c.KeyUp += new KeyEventHandler(FPSInput_KeyUp);
                        _fpsLimitFields.Add((TextBox) c);
                    }
                }
                if(c.Controls.Count > 0) CollectHotkeyAndLimitFields(c);
            }
        }

        private void ToggleTrainerControls(bool state) {
            positionTextBox.Visible = state;
            velocityTextBox.Visible = state;
            _speedometer.Visible = !state;
            altPositionTextbox.Visible = !state;
            rightAlignCheckbox.Visible = !state;
        }
        private void HideTrainerControls() {
            trainerOSDCheckbox.Enabled = true;
            positionTextBox.Visible = false;
            velocityTextBox.Visible = false;
            _speedometer.Visible = false;
            altPositionTextbox.Visible = false;
        }

        private void ModifyWindowForSmallDisplays() {
            Screen screen = Screen.FromHandle(this.Handle);
            if((screen.WorkingArea.Height < this.Height + 2 && !extraFPSHotkeysPanel.Visible) || (screen.WorkingArea.Height < WINDOW_HEIGHT_DEFAULT + WINDOW_EXTRAHEIGHT_MOREKEYS + 2 && extraFPSHotkeysPanel.Visible)) {
                this.Height = screen.WorkingArea.Height - 100;
                this.Width = WINDOW_WIDTH_DEFAULT;
                collapsiblePanel.Height = PANEL_HEIGHT_DEFAULT - (WINDOW_HEIGHT_DEFAULT - this.Height) - 5;
                _smallDisplay = true;
            } else {
                this.Height = (!extraFPSHotkeysPanel.Visible) ? WINDOW_HEIGHT_DEFAULT : WINDOW_HEIGHT_DEFAULT + WINDOW_EXTRAHEIGHT_MOREKEYS;
                this.Width = WINDOW_WIDTH_DEFAULT - WINDOW_EXTRAWIDTH;
                collapsiblePanel.Height = (!extraFPSHotkeysPanel.Visible) ? PANEL_HEIGHT_DEFAULT : PANEL_HEIGHT_DEFAULT + PANEL_EXTRAHEIGHT_MOREKEYS;
                _smallDisplay = false;
            }
        }

        private void SetToolTips() {
            /** Options **/
            toolTip7500.SetToolTip(enableHotkeysCheckbox, "Toggle global hotkeys for res. scaling and fps limits");
            toolTip7500.SetToolTip(autorunMacroCheckbox, "Toggle the Freescroll Emulation Macro");
            toolTip7500.SetToolTip(minimalOSDCheckbox, "Displays the in-game information on a single line if enabled");
            toolTip7500.SetToolTip(defaultFPS, "Set the max FPS you want DOOM Eternal to run at\n" +
                "Value must be in the range 1-250");
            toolTip7500.SetToolTip(launchRTSSCheckbox, "Automatically launches RTSS with DESRU\n" +
                "Will launch RTSS immediately if it isn't open");
            toolTip7500.SetToolTip(firewallToggleButton, "Toggle a firewall rule blocking DOOM Eternal's connection\n" +
                "This prevents server side balance updates from being downloaded\n" +
                "Required to be enabled for Leaderboard Runs");
            toolTip7500.SetToolTip(cheatsToggleButton, "Enables cheats if the game is running\n" +
                "Installs or Uninstalls meath00k if the game is closed\n" +
                "Required to be disabled for Leaderboard Runs");

            /** Change Version **/
            toolTip7500.SetToolTip(versionDropDownSelector, "Select which verison of DOOM Eternal to switch to");
            toolTip7500.SetToolTip(replaceProfileCheckbox, "Replace your profile.bin when switching to/from 3.1\n" +
                "There's a known crash on version 3.1 with some profile.bin files. Read the help page for more info");
            toolTip7500.SetToolTip(refreshVersionsButton, "Refresh the list of installed versions");

            /** Resolution Scaling **/
            toolTip7500.SetToolTip(unlockOnStartupCheckbox, "Unlock Resolution Scaling when the game is launched");
            toolTip7500.SetToolTip(autoScalingCheckbox, "Automatically enable Dynamic Res. Scaling");
            toolTip7500.SetToolTip(minResInput, "Minimum resolution scale allowed when dynamic scaling is enabled\n" +
                "Value must be in the range 1-100");
            toolTip7500.SetToolTip(targetFPSInput, "Target FPS for Dynamic Resolution Scaling\n" +
                "Value must be in the range 60-1000");
            toolTip7500.SetToolTip(unlockResButton, "Schedule Res. Scale Unlocking\n" +
                "Res. Scaling will be unlocked after a few seconds when the game is in focus again");
        }

        private void SetFonts() {
            // Eternal UI 2 Regular 11.25point
            foreach(Control c in _hotkeyFields) c.Font = EternalUIRegular;
            foreach(Control c in _fpsLimitFields) c.Font = EternalUIRegular;
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
            minimalOSDCheckbox.Font = EternalUIRegular;
            enableMaxFPSCheckbox.Font = EternalUIRegular;
            launchRTSSCheckbox.Font = EternalUIRegular;
            gameStatus.Font = EternalUIRegular;
            currentFPSCap.Font = EternalUIRegular;
            macroStatus.Font = EternalUIRegular;
            slopeboostStatus.Font = EternalUIRegular;
            balanceStatus.Font = EternalUIRegular;
            cheatsStatus.Font = EternalUIRegular;
            reshadeStatus.Font = EternalUIRegular;
            resScaleStatus.Font = EternalUIRegular;
            rtssStatus.Font = EternalUIRegular;
            hotkeyStatus.Font = EternalUIRegular;
            unlockOnStartupCheckbox.Font = EternalUIRegular;
            autoScalingCheckbox.Font = EternalUIRegular;
            replaceProfileCheckbox.Font = EternalUIRegular;
            minResLabel.Font = EternalUIRegular;
            dynamicTargetLabel.Font = EternalUIRegular;
            minResInput.Font = EternalUIRegular;
            targetFPSInput.Font = EternalUIRegular;
            percentLabel.Font = EternalUIRegular;
            targetFPSLabel.Font = EternalUIRegular;
            trainerOSDCheckbox.Font = EternalUIRegular;
            positionTextBox.Font = EternalUIRegular;
            velocityTextBox.Font = EternalUIRegular;
            resToggleLabel0.Font = EternalUIRegular;
            resToggleLabel1.Font = EternalUIRegular;
            resToggleLabel2.Font = EternalUIRegular;
            resToggleLabel3.Font = EternalUIRegular;
            resPercent0.Font = EternalUIRegular;
            resPercent1.Font = EternalUIRegular;
            resPercent2.Font = EternalUIRegular;
            resPercent3.Font = EternalUIRegular;
            resHotkeyField0.Font = EternalUIRegular;
            resHotkeyField1.Font = EternalUIRegular;
            resHotkeyField2.Font = EternalUIRegular;
            resHotkeyField3.Font = EternalUIRegular;
            enableOSDCheckbox.Font = EternalUIRegular;
            trainerRadioLabel.Font = EternalUIRegular;
            velocityRadioNone.Font = EternalUIRegular;
            velocityRadioTotal.Font = EternalUIRegular;
            velocityRadioVertical.Font = EternalUIRegular;
            speedometerCheckbox.Font = EternalUIRegular;
            speedometerPrecisionCheckbox.Font = EternalUIRegular;
            altPositionTextbox.Font = EternalUIRegular;
            rightAlignCheckbox.Font = EternalUIRegular;
            hideDuringLoadsCheckbox.Font = EternalUIRegular;
            staticScalingRadioButton.Font = EternalUIRegular;
            dynScalingRadioButton.Font = EternalUIRegular;

            label1.Font = EternalUIRegular;
            label2.Font = EternalUIRegular;
            label3.Font = EternalUIRegular;
            label4.Font = EternalUIRegular;
            label5.Font = EternalUIRegular;
            label6.Font = EternalUIRegular;
            label7.Font = EternalUIRegular;
            label9.Font = EternalUIRegular;
            label10.Font = EternalUIRegular;
            label11.Font = EternalUIRegular;
            label12.Font = EternalUIRegular;
            label14.Font = EternalUIRegular;
            label15.Font = EternalUIRegular;
            label16.Font = EternalUIRegular;
            label17.Font = EternalUIRegular;
            label18.Font = EternalUIRegular;
            label19.Font = EternalUIRegular;
            label21.Font = EternalUIRegular;
            label22.Font = EternalUIRegular;
            label23.Font = EternalUIRegular;
            label25.Font = EternalUIRegular;
            label27.Font = EternalUIRegular;
            label28.Font = EternalUIRegular;
            label30.Font = EternalUIRegular;
            label34.Font = EternalUIRegular;
            label36.Font = EternalUIRegular;
            label24.Font = EternalUIRegular;
            label29.Font = EternalUIRegular;

            // Eternal UI 2 Bold 11.25point
            versionChangedLabel.Font = EternalUIBold;
            changeVersionButton.Font = EternalUIBold;
            refreshVersionsButton.Font = EternalUIBold;
            firewallToggleButton.Font = EternalUIBold;
            cheatsToggleButton.Font = EternalUIBold;
            firewallRestartLabel.Font = EternalUIBold;
            gameStatusLabel.Font = EternalUIBold;
            fpsCapLabel.Font = EternalUIBold;
            macroStatusLabel.Font = EternalUIBold;
            slopeboostStatusLabel.Font = EternalUIBold;
            balanceStatusLabel.Font = EternalUIBold;
            cheatsStatusLabel.Font = EternalUIBold;
            reshadeStatusLabel.Font = EternalUIBold;
            resScaleStatusLabel.Font = EternalUIBold;
            rtssStatusLabel.Font = EternalUIBold;
            hotkeyStatusLabel.Font = EternalUIBold;
            exitButton.Font = EternalUIBold;
            downpatcherButton.Font = EternalUIBold;
            discordButton.Font = EternalUIBold;
            helpButton.Font = EternalUIBold;
            unlockResButton.Font = EternalUIBold;
            showMoreKeysButton.Font = EternalUIBold;

            // Eternal Logo Bold 14point
            hotkeysTitle.Font = EternalLogoBold;
            versionTitle.Font = EternalLogoBold;
            optionsTitle.Font = EternalLogoBold;
            resTitle.Font = EternalLogoBold;
            infoPanelTitle.Font = EternalLogoBold;
            trainerTitle.Font = EternalLogoBold;
            moreHotkeysTitle.Font = EternalLogoBold;

            // Eternal Battle Bold 20.25point
            windowTitle.Font = EternalBattleBold;
        }

        public bool IsFormOnScreen() {
            foreach(var display in Screen.AllScreens) {
                Rectangle rect = new(this.Left, this.Top, 1, 1);
                if(display.WorkingArea.Contains(rect)) return true;
            }
            return false;
        }

        private void SnapFormToScreen() {
            Screen display = Screen.FromHandle(this.Handle);
            if(this.Top < display.WorkingArea.Top) this.Top = display.WorkingArea.Top + 1;
            if(this.Left < display.WorkingArea.Left) this.Left = display.WorkingArea.Left + 1;
        }

        internal void ToggleButtonStates(string button, bool state) {
            switch(button) {
                case "help":
                    helpButton.Enabled = state;
                    break;
                case "settings":
                    settingsButton.Enabled = state;
                    break;
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
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
        public class Speedometer: Panel {

            private static readonly Color EMPTY = Color.FromArgb(45, 45, 45);
            private static readonly Color GREEN = Color.FromArgb(255, 15, 115, 0);
            private static readonly Color ORANGE = Color.FromArgb(255, 200, 136, 0);
            private static readonly Color RED = Color.FromArgb(255, 141, 0, 0);

            private static readonly StringFormat FORMAT_RIGHT_ALIGN = new() { Alignment = StringAlignment.Far };

            private const string TEXT_FORMAT = "{0:0.0} M/S";
            private const string TEXT_FORMAT_PRECISION = "{0:0.00} M/S";

            private const double BHOP_SPEEDCAP = 38.5;

            public float HorizontalVelocity { get; set; } = 0f;
            public float VerticalVelocity { get; set; } = 0f;
            public float TotalVelocity { get; set; } = 0f;

            public Font AltFont { get; init; }

            public bool ShowVerticalVelocity { get; set; } = false;
            public bool IncreasedPrecision { get; set; } = false;
            public bool HideSecondaryVelocity { get; set; } = false;

            public bool RightAlignText { get; set; } = false;

            public Speedometer(Font font, Font alt, Point loc, Size size, Color textColor, Color backColor) {
                this.Font = font;
                AltFont = alt;
                this.Location = loc;
                this.Size = size;
                this.ForeColor = textColor;
                this.BackColor = backColor;
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            }
            protected override void OnPaint(PaintEventArgs e) {
                base.OnPaint(e);

                string hVelText = FormatVelocity(HorizontalVelocity);
                string visualizerText = FormatVelocity((ShowVerticalVelocity) ? VerticalVelocity : TotalVelocity);
                var sizePercent = 1.0;
                var colorPercent = 0.0;
                if(!ShowVerticalVelocity) {
                    sizePercent = TotalVelocity >= BHOP_SPEEDCAP ? 1 : TotalVelocity / BHOP_SPEEDCAP;
                    colorPercent = TotalVelocity >= (BHOP_SPEEDCAP * 1.5) ? 1 : TotalVelocity / (BHOP_SPEEDCAP * 1.5);
                }
                if(!HideSecondaryVelocity) {
                    // Total/Vertical Velocity
                    if(RightAlignText) {
                        e.Graphics.FillRectangle(new SolidBrush(GradientPick(colorPercent, GREEN, ORANGE, RED)), new Rectangle(111 + (175 - ((int) (175 * sizePercent))), this.Size.Height - 36, (int) (175 * sizePercent), 36));

                        Rectangle visShadow = new(15, this.Size.Height - 33, 267, 26),
                                    visText = new(13, this.Size.Height - 35, 267, 26);
                        e.Graphics.DrawString(visualizerText, AltFont, new SolidBrush(Color.Black), visShadow, FORMAT_RIGHT_ALIGN);
                        e.Graphics.DrawString(visualizerText, AltFont, new SolidBrush(this.ForeColor), visText, FORMAT_RIGHT_ALIGN);
                    } else {
                        e.Graphics.FillRectangle(new SolidBrush(GradientPick(colorPercent, GREEN, ORANGE, RED)), new Rectangle(8, this.Size.Height - 36, (int) (175 * sizePercent), 36));

                        e.Graphics.DrawString(visualizerText, AltFont, new SolidBrush(Color.Black), 15, this.Size.Height - 33);
                        e.Graphics.DrawString(visualizerText, AltFont, new SolidBrush(this.ForeColor), 13, this.Size.Height - 35);
                    }
                }

                // Horizontal Velocity
                if(RightAlignText) {
                    Rectangle mainShadow = new(2, this.Size.Height - 80, 294, 40),
                                mainText = new(0, this.Size.Height - 82, 294, 40);
                    e.Graphics.DrawString(hVelText, this.Font, new SolidBrush(Color.Black), mainShadow, FORMAT_RIGHT_ALIGN);
                    e.Graphics.DrawString(hVelText, this.Font, new SolidBrush(this.ForeColor), mainText, FORMAT_RIGHT_ALIGN);
                } else {
                    e.Graphics.DrawString(hVelText, this.Font, new SolidBrush(Color.Black), 2, this.Size.Height - 80);
                    e.Graphics.DrawString(hVelText, this.Font, new SolidBrush(this.ForeColor), 0, this.Size.Height - 82);
                }
            }

            private static Color GradientPick(double percent, Color start, Color middle, Color end) {
                if(percent <= 0.5) {
                    if(percent == 0) return EMPTY;
                    return GREEN;
                } else if(percent > 0.5 && percent <= 0.75) {
                    return ColorInterp(start, middle, (percent - 0.5) / 0.25);
                } else {
                    return ColorInterp(middle, end, (percent - 0.75) / 0.25);
                }
            }

            private string FormatVelocity(float velocity) => string.Format((IncreasedPrecision) ? TEXT_FORMAT_PRECISION : TEXT_FORMAT, velocity);

            private static Color ColorInterp(Color start, Color end, double percent) => Color.FromArgb(LinearInterpolate(start.A, end.A, percent),
                LinearInterpolate(start.R, end.R, percent),
                LinearInterpolate(start.G, end.G, percent),
                LinearInterpolate(start.B, end.B, percent));

            private static byte LinearInterpolate(int start, int end, double percent) => (byte) (start + (int) Math.Round(percent * (end - start)));
        }
        #endregion
    }
}