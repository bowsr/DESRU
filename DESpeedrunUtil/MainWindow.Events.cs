using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using static DESpeedrunUtil.Interop.DLLImports;

namespace DESpeedrunUtil {
    partial class MainWindow {
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
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macroProcess, _hotkeys);
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
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type, _macroProcess, _hotkeys);
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
                if(Hooked && !_memory.DynamicEnabled()) _memory.EnableDynamicScaling(_targetFPS);
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
                var dir = _steamInstallation + "\\userdata\\" + _steamID3 + PROFILE_DIR;
                try {
                    if(desired == "3.1") {
                        File.Move(dir + PROFILE_FILE, dir + "\\main.bin");
                        if(File.Exists(dir + "\\3.1.bin")) File.Move(dir + "\\3.1.bin", dir + PROFILE_FILE);
                    } else {
                        if(current == "3.1") {
                            if(File.Exists(dir + "\\main.bin")) {
                                if(File.Exists(dir + PROFILE_FILE)) File.Move(dir + PROFILE_FILE, dir + "\\3.1.bin");
                                File.Move(dir + "\\main.bin", dir + PROFILE_FILE);
                            } else {
                                File.Copy(dir + PROFILE_FILE, dir + "\\3.1.bin");
                            }
                        }
                    }
                } catch(Exception ex) {
                    Log.Error(ex, "An error occured when attempting to change profile.bin files.");
                }
            }
            changeVersionButton.Enabled = false;
            Task.Run(async delegate {
                versionChangedLabel.ForeColor = Color.LimeGreen;
                await Task.Delay(3000);
                versionChangedLabel.ForeColor = PANEL_BACKCOLOR;
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
                firewallRestartLabel.ForeColor = PANEL_BACKCOLOR;
            } else {
                FirewallHandler.CreateFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe");
                firewallToggleButton.Text = "Remove Firewall Rule";
                _fwRestart = true;
                _fwRuleExists = true;
                if(Hooked) firewallRestartLabel.ForeColor = TEXT_FORECOLOR;
            }
        }
        private void MeathookToggle_Click(object sender, EventArgs e) {
            // _mhRestart
            // disable button when removing mh while game is open - "schedule" removal after game stops
            // if mh isn't installed when game is launched, can freely add/remove mh, but still needs restart to take effect
            if(_mhExists) {
                _mhScheduleRemoval = true;
                Log.Information("meath00k scheduled for removal.");
            } else {
                File.Copy(@".\meath00k\XINPUT1_3.dll", _gameDirectory + "\\XINPUT1_3.dll");
                if(Hooked) meathookRestartLabel.ForeColor = TEXT_FORECOLOR;
                Log.Information("meath00k installed.");
            }
        }
        private void MoreKeysButton_Click(object sender, EventArgs e) {
            if(this.Height == 725) {
                this.Height = 943;
                extraFPSHotkeysPanel.Visible = true;
                showMoreKeysButton.Text = "Hide Extra FPS Hotkeys";
            } else {
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
            MemoryHandler.OffsetList = JsonConvert.DeserializeObject<List<MemoryHandler.KnownOffsets>>(File.ReadAllText(@".\offsets.json"));

            InitializeFonts();
            var titleBar = new DESRUShadowLabel(windowTitle.Font, WINDOW_TITLE, windowTitle.Location, Color.FromArgb(190, 34, 34), FORM_BACKCOLOR);
            titleBar.MouseMove += new MouseEventHandler(DragWindow_MouseMove);
            titleBar.MouseUp += new MouseEventHandler(DragWindow_MouseUp);
            this.Controls.Add(titleBar);
            this.Controls.Add(new DESRUShadowLabel(hotkeysTitle.Font, "KEYBINDS", hotkeysTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));
            this.Controls.Add(new DESRUShadowLabel(optionsTitle.Font, "OPTIONS", optionsTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));
            this.Controls.Add(new DESRUShadowLabel(versionTitle.Font, "CHANGE VERSION", versionTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));
            this.Controls.Add(new DESRUShadowLabel(infoPanelTitle.Font, "INFO PANEL", infoPanelTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));
            this.Controls.Add(new DESRUShadowLabel(resTitle.Font, "RESOLUTION SCALING", resTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));
            this.Controls.Add(new DESRUShadowLabel(moreHotkeysTitle.Font, "MORE FPS HOTKEYS", moreHotkeysTitle.Location, TEXT_FORECOLOR, FORM_BACKCOLOR));

            // User Settings
            var fpsJson = "";
            if(File.Exists(FPSKEYS_JSON)) fpsJson = File.ReadAllText(FPSKEYS_JSON);
            _macroProcess = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
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
            Log.Information("Loaded MainWindow");
        }

        // Event method that runs upon closing of the <c>MainWindow</c> form.
        private void MainWindow_Closing(object sender, FormClosingEventArgs e) {
            // User Settings
            Properties.Settings.Default.DownScrollKey = (int) _macroProcess.GetHotkey(true);
            Properties.Settings.Default.UpScrollKey = (int) _macroProcess.GetHotkey(false);
            Properties.Settings.Default.DefaultFPSCap = _fpsDefault;
            Properties.Settings.Default.MacroEnabled = autorunMacroCheckbox.Checked;
            Properties.Settings.Default.HotkeysEnabled = enableHotkeysCheckbox.Checked;
            Properties.Settings.Default.GameLocation = _gameDirectory;
            Properties.Settings.Default.StartupUnlock = unlockOnStartupCheckbox.Checked;
            Properties.Settings.Default.AutoDynamic = autoDynamicCheckbox.Checked;
            Properties.Settings.Default.MinResPercent = _minResPercent;
            Properties.Settings.Default.TargetFPSScaling = _targetFPS;
            Properties.Settings.Default.SteamInstallation = _steamInstallation;
            Properties.Settings.Default.SteamID3 = _steamID3;
            Properties.Settings.Default.ReplaceProfile = replaceProfileCheckbox.Checked;
            if(WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = Location;
            else if(WindowState == FormWindowState.Minimized) Properties.Settings.Default.Location = RestoreBounds.Location;

            File.WriteAllText(FPSKEYS_JSON, _hotkeys.FPSHotkeys.SerializeIntoJSON());

            Properties.Settings.Default.Save();
            Log.Information("User settings saved");

            _hotkeys.DisableHotkeys();
            _macroProcess.Stop(false);
        }
    }
}
