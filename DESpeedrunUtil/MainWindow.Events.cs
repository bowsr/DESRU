using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using Linearstar.Windows.RawInput;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using static DESpeedrunUtil.Define.Structs;
using static DESpeedrunUtil.Interop.DLLImports;

namespace DESpeedrunUtil {
    partial class MainWindow {

        /** RawInput Events **/
        internal event KeyEventHandler RIKeyDown;
        internal event KeyEventHandler RIKeyUp;
        internal event MouseEventHandler RIMouseDown;
        internal event MouseEventHandler RIMouseUp;
        internal event EventHandler<MouseWheelEventArgs> RIMouseScroll;

        #region Hotkey Assignment
        private void HotkeyAssignment_KeyDown(object sender, KeyEventArgs e) {
            if(!_hkAssignmentMode) return;
            Keys pressedKey;

            if(e.Control) pressedKey = HotkeyHandler.ModKeySelector(0);
            else if(e.Shift) pressedKey = HotkeyHandler.ModKeySelector(1);
            else if(e.Alt) pressedKey = HotkeyHandler.ModKeySelector(2);
            else pressedKey = e.KeyCode;
            if(pressedKey == Keys.Escape) pressedKey = Keys.None;
            bool isValid = !INVALID_KEYS.Contains(pressedKey);

            string tag = (string) _selectedHKField.Tag;
            _hkAssignmentMode = false;
            _selectedHKField = null;
            if(isValid) {
                int type = -1;
                try {
                    type = tag switch {
                        "hkMacroDown" => 0,
                        "hkMacroUp" => 1,
                        "hkResToggle" => 2,
                        _ => int.TryParse(tag.Replace("hkFps", ""), out int id) ? id + 3 : -1,
                    };
                } catch(FormatException f) {
                    Log.Error(f, "Attempted to parse a hotkeyField's tag as an fpskey despite it not being one.");
                }
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macro, _hotkeys);
            }
            UpdateHotkeyAndInputFields();
            e.Handled = true;
        }
        private void HotkeyAssignment_MouseDown(object sender, MouseEventArgs e) {
            if(!_hkAssignmentMode) {
                if((sender is MainWindow || (sender is DESRUShadowLabel && ((Label) sender).Text == WINDOW_TITLE)) && !_mouseDown) {
                    _mouseDown = true;
                    _lastLocation = e.Location;
                }
                return;
            }

            Keys pressedKey = HotkeyHandler.ConvertMouseButton(e.Button);
            bool isValid = !INVALID_KEYS.Contains(pressedKey);

            string tag = (string) _selectedHKField.Tag;
            _hkAssignmentMode = false;
            if(pressedKey == Keys.LButton && sender.Equals(_selectedHKField)) _mouse1Pressed = true;
            _selectedHKField = null;
            if(isValid) {
                int type = tag switch {
                    "hkMacroDown" => 0,
                    "hkMacroUp" => 1,
                    "hkResToggle" => 2,
                    _ => int.TryParse(tag.Replace("hkFps", ""), out int id) ? id + 3 : -1,
                };
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macro, _hotkeys);
            }
            UpdateHotkeyAndInputFields();
        }

        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            if(_mouse1Pressed) {
                _mouse1Pressed = false;
                return;
            }

            if((GetAsyncKeyState(Keys.LButton) & 0x01) == 1) {
                _selectedHKField = (Label) sender;
                _selectedHKField.Text = "Press a key";
                _selectedHKField.BackColor = Color.WhiteSmoke;
                _selectedHKField.ForeColor = Color.Black;
                this.ActiveControl = null;

                _hkAssignmentMode = true;
            }
        }
        #endregion

        private void DragWindow_MouseMove(object sender, MouseEventArgs e) {
            if(_mouseDown) {
                this.Location = new Point(
                    (this.Location.X - _lastLocation.X) + e.X,
                    (this.Location.Y - _lastLocation.Y) + e.Y);
                this.Update();
            }
        }

        private void DragWindow_MouseUp(object sender, MouseEventArgs e) => _mouseDown = false;

        private void MainWindow_KeyPreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            switch(e.KeyCode) {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        #region Textbox Input Filtering
        private void Input_KeyPressNumericOnly(object sender, KeyPressEventArgs e) {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
                return;
            }
        }

        private void FPSInput_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;
            string tag = (string) ((Control) sender).Tag;

            if(!int.TryParse(text, out int p)) p = -1;

            if(p > 250) p = 250;
            if(tag == "maxfps" && p <= 0) p = 250;
            if(p != -1) {
                if(p == 0) p = 1;
                ((TextBox) sender).Text = p.ToString();
            } else {
                ((TextBox) sender).Text = "";
            }

            switch(tag) {
                case "maxfps":
                    _fpsDefault = p;
                    break;
                default:
                    _hotkeys.FPSHotkeys.ChangeLimit(int.TryParse(tag.Replace("fpscap", ""), out int id) ? id : -1, p);
                    break;
            }
        }

        private void MinResInput_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;

            if(!int.TryParse(text, out int resPercent)) resPercent = 50;

            if(resPercent <= 1) resPercent = 1;
            if(resPercent > 100) resPercent = 100;
            _minResPercent = resPercent;
            ((TextBox) sender).Text = resPercent.ToString();

            if(Hooked) {
                _memory.SetMinRes(resPercent / 100f);
            }
        }
        private void TargetFPS_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;

            if(!int.TryParse(text, out int target)) target = 1000;

            if(target < 60) target = 60;
            if(target > 1000) target = 1000;
            _targetFPS = target;
            ((TextBox) sender).Text = target.ToString();
        }
        #endregion

        #region Checkboxes
        private void AutoStartMacro_CheckChanged(object sender, EventArgs e) => _enableMacro = ((CheckBox) sender).Checked;
        private void EnableHotkeys_CheckChanged(object sender, EventArgs e) {
            bool val = ((CheckBox) sender).Checked;
            if(val) {
                if(Hooked) _hotkeys.EnableHotkeys();
            }else {
                _hotkeys.DisableHotkeys();
            }
        }
        private void AutoDynamic_CheckChanged(object sender, EventArgs e) {
            if(((CheckBox) sender).Checked) {
                if(Hooked && !_memory.ReadDynamicRes()) _memory.EnableDynamicScaling(_targetFPS);
            }
        }
        private void MaxFPS_CheckChanged(object sender, EventArgs e) {
            if(!((CheckBox) sender).Checked && !_justLaunched) {
                Log.Warning("Max FPS Limiter disabled.");
                if(Hooked) {
                    _memory.SetMaxHz(1000);
                    _memory.SetFlag(false, "limiter");
                }
                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show("Disabling this option means you MUST limit your framerate to 250 or lower through external means.\n\n" +
                    "Common options include Rivatuner Statistics Server (RTSS), NVIDIA Control Panel, etc.\n\n" +
                    "If you use a 3rd party program like RTSS, ensure that it is running at all times during your run.", "External FPS Limit Required");
            }else {
                Log.Information("Max FPS Limiter enabled.");
                if(Hooked) {
                    _memory.SetMaxHz(_fpsDefault);
                    _memory.SetFlag(true, "limiter");
                }
            }
        }
        #endregion

        #region Buttons
        private void ExitButton_Click(object sender, EventArgs e) => this.Close();
        private void HelpButton_Click(object sender, EventArgs e) {
            new HelpPage().Show();
        }
        private void UnlockRes_Click(object sender, EventArgs e) {
            if(Hooked) _memory.ScheduleResUnlock(autoDynamicCheckbox.Checked, _targetFPS);
        }
        private void RefreshVersions_Click(object sender, EventArgs e) {
            if(_steamDirectory != string.Empty) DetectAllGameVersions();
        }
        private void Downpatcher_Click(object sender, EventArgs e) {
            Process.Start(new ProcessStartInfo("https://github.com/mcdalcin/DoomEternalDownpatcher/releases/latest") { UseShellExecute = true });
        }
        private void Discord_Click(object sender, EventArgs e) {
            Process.Start(new ProcessStartInfo("https://discord.gg/dtDa9VZ") { UseShellExecute = true });
        }

        private void ChangeVersion_Click(object sender, EventArgs e) {
            if(versionDropDownSelector.Text == string.Empty) return;
            string current = GetCurrentVersion(), desired = versionDropDownSelector.Text;
            if(current == "Unknown") {
                MessageBox.Show("Please launch DOOM Eternal at least once before attempting to change versions.", "Unknown Current Game Version");
                return;
            }
            if(current == desired) return;
            if(Directory.Exists(_steamDirectory + "\\DOOMEternal " + current)) return; // Eventually add a popup saying there's a folder conflict
            Directory.Move(_gameDirectory, _gameDirectory + " " + current);
            Directory.Move(_gameDirectory + " " + desired, _gameDirectory);
            Log.Information("Game Version changed to [{Version}]", desired);
            if(_steamID3 != string.Empty && replaceProfileCheckbox.Checked) {
                var dir = _steamInstallation + "\\userdata\\" + _steamID3 + PATH_PROFILE_DIR;
                try {
                    if(desired == "3.1") {
                        File.Move(dir + PATH_PROFILE_FILE, dir + "\\main.bin");
                        if(File.Exists(dir + "\\3.1.bin")) File.Move(dir + "\\3.1.bin", dir + PATH_PROFILE_FILE);
                    }else {
                        if(current == "3.1") {
                            if(File.Exists(dir + "\\main.bin")) {
                                if(File.Exists(dir + PATH_PROFILE_FILE)) File.Move(dir + PATH_PROFILE_FILE, dir + "\\3.1.bin");
                                File.Move(dir + "\\main.bin", dir + PATH_PROFILE_FILE);
                            }else {
                                File.Copy(dir + PATH_PROFILE_FILE, dir + "\\3.1.bin");
                            }
                        }
                    }
                }catch(Exception ex) {
                    Log.Error(ex, "An error occured when attempting to change profile.bin files.");
                }
            }
            changeVersionButton.Enabled = false;
            Task.Run(async delegate {
                versionChangedLabel.ForeColor = Color.LimeGreen;
                await Task.Delay(3000);
                versionChangedLabel.ForeColor = COLOR_PANEL_BACK;
            });
        }
        private void FirewallToggle_Click(object sender, EventArgs e) {
            if(_fwRuleExists) {
                FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", true);
                firewallToggleButton.Text = "Create Firewall Rule";
                if(_fwRestart) _fwRestart = false;
                _fwRuleExists = false;
                if(Hooked) {
                    _memory.SetFlag(_fwRuleExists, "firewall");
                }
                firewallRestartLabel.ForeColor = COLOR_PANEL_BACK;
            }else {
                FirewallHandler.CreateFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe");
                firewallToggleButton.Text = "Remove Firewall Rule";
                _fwRestart = true;
                _fwRuleExists = true;
                if(Hooked) firewallRestartLabel.ForeColor = COLOR_TEXT_FORE;
            }
        }
        private void MeathookToggle_Click(object sender, EventArgs e) {
            // _mhRestart
            // disable button when removing mh while game is open - "schedule" removal after game stops
            // if mh isn't installed when game is launched, can freely add/remove mh, but still needs restart to take effect
            if(_mhExists) {
                _mhScheduleRemoval = true;
                Log.Information("meath00k scheduled for removal.");
            }else {
                File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + "\\XINPUT1_3.dll");
                foreach(string v in _gameVersions) {
                    try {
                        File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + " " + v + "\\XINPUT1_3.dll");
                    }catch(Exception ex) {
                        Log.Error(ex, "An error occured when attempting to install meath00k. v: {Version}", v);
                        continue;
                    }
                }
                foreach(string dir in _extraGameDirectories) {
                    try {
                        File.Copy(@".\meath00k\XINPUT1_3.dll", dir + "\\XINPUT1_3.dll");
                    }catch(Exception ex) {
                        Log.Error(ex, "An error occured when attempting to install meath00k in directory: {Directory}", dir);
                        continue;
                    }
                }
                if(Hooked) meathookRestartLabel.ForeColor = COLOR_TEXT_FORE;
                Log.Information("meath00k installed.");
            }
        }
        private void MoreKeysButton_Click(object sender, EventArgs e) {
            if(this.Height == 725) {
                this.Height = 943;
                extraFPSHotkeysPanel.Visible = true;
                showMoreKeysButton.Text = "Hide Extra FPS Hotkeys";
            }else {
                this.Height = 725;
                extraFPSHotkeysPanel.Visible = false;
                showMoreKeysButton.Text = "Show More FPS Hotkeys";
            }
        }
        #endregion

        private void DropDown_IndexChanged(object sender, EventArgs e) {
            if(!Hooked) changeVersionButton.Enabled = ((ComboBox) sender).Text != GetCurrentVersion();
        }

        // Event method that runs upon loading of the MainWindow form.
        private void MainWindow_Load(object sender, EventArgs e) {
            if(!File.Exists(@".\offsets.json")) {
                File.WriteAllText(@".\offsets.json", System.Text.Encoding.UTF8.GetString(Properties.Resources.offsets));
                Log.Information("offsets.json does not exist. Using template from application resources.");
            }
            MemoryHandler.OffsetList = JsonConvert.DeserializeObject<List<KnownOffsets>>(File.ReadAllText(@".\offsets.json"));

            InitializeFonts();
            var titleBar = new DESRUShadowLabel(windowTitle.Font, WINDOW_TITLE, windowTitle.Location, Color.FromArgb(190, 34, 34), COLOR_FORM_BACK);
            titleBar.MouseMove += new MouseEventHandler(DragWindow_MouseMove);
            titleBar.MouseUp += new MouseEventHandler(DragWindow_MouseUp);
            this.Controls.Add(titleBar);
            this.Controls.Add(new DESRUShadowLabel(hotkeysTitle.Font, "KEYBINDS", hotkeysTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            this.Controls.Add(new DESRUShadowLabel(optionsTitle.Font, "OPTIONS", optionsTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            this.Controls.Add(new DESRUShadowLabel(versionTitle.Font, "CHANGE VERSION", versionTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            this.Controls.Add(new DESRUShadowLabel(infoPanelTitle.Font, "INFO PANEL", infoPanelTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            this.Controls.Add(new DESRUShadowLabel(resTitle.Font, "RESOLUTION SCALING", resTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            this.Controls.Add(new DESRUShadowLabel(moreHotkeysTitle.Font, "MORE FPS HOTKEYS", moreHotkeysTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));

            // User Settings
            var fpsJson = "";
            if(File.Exists(PATH_FPSKEYS_JSON)) fpsJson = File.ReadAllText(PATH_FPSKEYS_JSON);
            _macro = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
            _hotkeys = new HotkeyHandler((Keys) Properties.Settings.Default.ResScaleKey, fpsJson, this);
            _fpsDefault = Properties.Settings.Default.DefaultFPSCap;
            autorunMacroCheckbox.Checked = Properties.Settings.Default.MacroEnabled;
            _enableMacro = Properties.Settings.Default.MacroEnabled;
            enableHotkeysCheckbox.Checked = Properties.Settings.Default.HotkeysEnabled;
            _gameDirectory = Properties.Settings.Default.GameLocation;
            unlockOnStartupCheckbox.Checked = Properties.Settings.Default.StartupUnlock;
            autoDynamicCheckbox.Checked = Properties.Settings.Default.AutoDynamic;
            _minResPercent = Properties.Settings.Default.MinResPercent;
            _targetFPS = Properties.Settings.Default.TargetFPSScaling;
            _steamInstallation = Properties.Settings.Default.SteamInstallation;
            _steamID3 = Properties.Settings.Default.SteamID3;
            replaceProfileCheckbox.Checked = Properties.Settings.Default.ReplaceProfile;
            enableMaxFPSCheckbox.Checked = Properties.Settings.Default.EnableMaxFPSLimit;
            _extraGameDirectories = new List<string>();
            string directories = Properties.Settings.Default.ExtraGameDirectories;
            while(directories.Contains('|')) {
                var d = directories[..directories.IndexOf('|')];
                if(File.Exists(d + "\\DOOMEternalx64vk.exe")) _extraGameDirectories.Add(d);
                directories = directories.Replace(d + "|", "");
            }
            UpdateHotkeyAndInputFields();

            var defaultLocation = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
            Point loc = Properties.Settings.Default.Location;
            if(loc != Point.Empty) Location = loc;
            this.Height = 725;
            if(!IsFormOnScreen() || loc == Point.Empty) Location = defaultLocation;
            Log.Information("Loaded user settings");

            AddMouseIntercepts(this);
            
            SearchForSteamGameDir();
            if(_steamInstallation != "n/a") SearchForGameSaves();
            _formTimer.Start();
            _statusTimer.Start();
            _settingsTimer.Start();
            _justLaunched = false;
            Log.Information("Loaded MainWindow");
        }

        // Event method that runs upon closing of the <c>MainWindow</c> form.
        private void MainWindow_Closing(object sender, FormClosingEventArgs e) {
            _formTimer.Stop();
            _statusTimer.Stop();
            _settingsTimer.Stop();

            // User Settings
            SaveSettings();

            File.WriteAllText(PATH_FPSKEYS_JSON, _hotkeys.FPSHotkeys.SerializeIntoJSON());

            Log.Information("User settings saved");

            _hotkeys.DisableHotkeys();
            _macro.Stop(false);
            if(_memory != null) _memory.ClosingDESRU();

            RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }

        private void MainWindow_Shown(object sender, EventArgs e) {
            if(Properties.Settings.Default.ShowFPSLimitWarning) {
                var dialog = new FPSLimitWarning(this.Location, this.Width, this.Height);
                Log.Information("Displayed FPS Limiter Warning to user.");
                if(dialog.ShowDialog() == DialogResult.Yes) {
                    _justLaunched = true;
                    enableMaxFPSCheckbox.Checked = false;
                    _justLaunched = false;
                    Log.Warning("User chose to disable the max FPS limiter.");
                }
                Properties.Settings.Default.ShowFPSLimitWarning = !dialog.disableWarningCheckbox.Checked;
                if(!Properties.Settings.Default.ShowFPSLimitWarning) Log.Information("FPS Limiter Warning will not be displayed again.");
                Properties.Settings.Default.Save();
            }
        }
    }
}
