using DESpeedrunUtil.Firewall;
using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using DESpeedrunUtil.Memory;
using DESpeedrunUtil.Util;
using Linearstar.Windows.RawInput;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using WindowsInput.Events;
using static DESpeedrunUtil.Define.Constants;
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
                        "hkResToggle0" => 3,
                        "hkResToggle1" => 4,
                        "hkResToggle2" => 5,
                        "hkResToggle3" => 6,
                        _ => int.TryParse(tag.Replace("hkFps", ""), out int id) ? id + HotkeyHandler.FPSKEYS_START_ID : -1,
                    };
                } catch(FormatException f) {
                    Log.Error(f, "Attempted to parse a hotkeyField's tag as an fpskey despite it not being one.");
                }
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type);
            }
            UpdateHotkeyAndInputFields();
            e.Handled = true;
        }
        private void HotkeyAssignment_MouseDown(object sender, MouseEventArgs e) {
            Log.Verbose("HotkeyAssignment_MouseDown");
            if(!_hkAssignmentMode) {
                if((sender is MainWindow || (sender is Panel && ((Control) sender).Name == "collapsiblePanel") || (sender is DESRUShadowLabel && ((Label) sender).Text == WINDOW_TITLE)) && !_mouseDown) {
                    _mouseDown = true;
                    _lastLocation = e.Location;
                }
                Log.Verbose("Not in assignment mode. Exiting function.");
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
                    "hkResToggle0" => 3,
                    "hkResToggle1" => 4,
                    "hkResToggle2" => 5,
                    "hkResToggle3" => 6,
                    _ => int.TryParse(tag.Replace("hkFps", ""), out int id) ? id + 7 : -1,
                };
                if(type != -1) HotkeyHandler.ChangeHotkeys(pressedKey, type);
            }
            UpdateHotkeyAndInputFields();
        }
        /*
        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            var keyState = GetAsyncKeyState(Keys.LButton);
            var keyBit = keyState & 0x01;
            Log.Information("Selected hotkey field. tag={Tag} m1={Mouse1} GetAsyncKeyState={State}|{Bit}", (string) ((Label) sender).Tag, _mouse1Pressed, keyState, keyBit);
            if(_mouse1Pressed) {
                Log.Information("MOUSE1 was pressed. Canceling hotkey field selection.");
                _mouse1Pressed = false;
                return;
            }

            if(keyBit == 1) {
                _selectedHKField = (Label) sender;
                _selectedHKField.Text = "Press a key";
                _selectedHKField.BackColor = Color.WhiteSmoke;
                _selectedHKField.ForeColor = Color.Black;
                this.ActiveControl = null;

                _hkAssignmentMode = true;
                Log.Information("Hotkey Assignment Mode active");
            }
        }
        */
        private void HotkeyAssignment_MouseFieldSelected(object sender, MouseEventArgs e) {
            Log.Information("Selected hotkey field. tag={Tag} m1={Mouse1} MouseButton={Button}", (string) ((Label) sender).Tag, _mouse1Pressed, e.Button);
            if(_mouse1Pressed) {
                Log.Information("MOUSE1 was pressed. Canceling hotkey field selection.");
                _mouse1Pressed = false;
                return;
            }

            if(e.Button == MouseButtons.Left) {
                _selectedHKField = (Label) sender;
                _selectedHKField.Text = "Press a key";
                _selectedHKField.BackColor = Color.WhiteSmoke;
                _selectedHKField.ForeColor = Color.Black;
                this.ActiveControl = null;

                _hkAssignmentMode = true;
                Log.Information("Hotkey Assignment Mode active");
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

        private void DragWindow_MouseUp(object sender, MouseEventArgs e) {
            _mouseDown = false;
            SnapFormToScreen();
            ModifyWindowForSmallDisplays();
        }

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

            if(!int.TryParse(text, out int fps)) fps = -1;

            if(fps > 250) {
                if(tag == "maxfps" && !enforce250FPSCheckbox.Checked) {
                    if(fps > 1000) fps = 1000;
                } else {
                    fps = 250;
                }
            }
            if(tag == "maxfps" && fps <= 0) fps = 250;
            if(fps != -1) {
                if(fps == 0) fps = 1;
                ((TextBox) sender).Text = fps.ToString();
            } else {
                ((TextBox) sender).Text = "";
            }

            switch(tag) {
                case "maxfps":
                    _fpsDefault = fps;
                    break;
                default:
                    HotkeyHandler.Instance.FPSHotkeys.ChangeLimit(int.TryParse(tag.Replace("fpscap", ""), out int id) ? id : -1, fps);
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
                Memory.SetMinRes(resPercent / 100f);
            }
        }
        private void ResToggle_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;
            string tag = (string) ((Control) sender).Tag;

            if(!int.TryParse(text, out int resPercent)) resPercent = -1;

            if(resPercent > 100) resPercent = 100;
            if(resPercent != -1) {
                if(resPercent == 0) resPercent = 1;
                ((TextBox) sender).Text = resPercent.ToString();
            } else {
                ((TextBox) sender).Text = "";
            }

            switch(tag) {
                case "resToggle0":
                    _resPercent0 = resPercent;
                    break;
                case "resToggle1":
                    _resPercent1 = resPercent;
                    break;
                case "resToggle2":
                    _resPercent2 = resPercent;
                    break;
                case "resToggle3":
                    _resPercent3 = resPercent;
                    break;
            }
        }
        private void TargetFPS_KeyUp(object sender, KeyEventArgs e) {
            var text = ((TextBox) sender).Text;

            if(!int.TryParse(text, out int target)) target = 1000;

            if(target < 1) target = 60;
            if(target > 1000) target = 1000;
            _targetFPS = target >= 60 ? target : 60;
            ((TextBox) sender).Text = target.ToString();
        }
        #endregion

        #region Checkboxes
        private void AutoStartMacro_CheckChanged(object sender, EventArgs e) => _enableMacro = ((CheckBox) sender).Checked;
        private void EnableHotkeys_CheckChanged(object sender, EventArgs e) {
            bool val = ((CheckBox) sender).Checked;
            if(val) {
                if(Hooked) HotkeyHandler.Instance.EnableHotkeys();
            } else {
                HotkeyHandler.Instance.DisableHotkeys();
            }
        }
        private void AutoDynamic_CheckChanged(object sender, EventArgs e) {
            if(((CheckBox) sender).Checked) {
                if(Hooked && !Memory.ReadDynamicRes()) Memory.EnableDynamicScaling(_targetFPS);
            }
        }
        private void MaxFPS_CheckChanged(object sender, EventArgs e) {
            if(!((CheckBox) sender).Checked && !_justLaunched) {
                Log.Warning("Max FPS Limiter disabled.");
                defaultFPS.MaxLength = 4;
                if(_fpsDefault == 250) {
                    _fpsDefault = 1000;
                    defaultFPS.Text = "1000";
                }
                if(Hooked) {
                    Memory.SetMaxHz(_fpsDefault);
                    Memory.SetFlag(false, "limiter");
                }
                launchRTSSCheckbox.Enabled = true;
                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show("Disabling this option means you MUST limit your framerate to 250 or lower through external means if you set an FPS value greater than 250.\n\n" +
                    "Common options include Rivatuner Statistics Server (RTSS), NVIDIA Control Panel, etc.\n\n" +
                    "If you use a 3rd party program like RTSS, ensure that it is running at all times during your run.\n" +
                    "If you use RTSS specifically, DESRU can launch it for you if you enable the \"Launch RTSS with DESRU\" option.", "External FPS Limit Required");
            } else {
                Log.Information("Max FPS Limiter enabled.");
                if(_fpsDefault > 250 && !_justLaunched) {
                    _fpsDefault = 250;
                    defaultFPS.Text = "250";
                }
                defaultFPS.MaxLength = 3;
                if(Hooked) {
                    Memory.SetMaxHz(_fpsDefault);
                    Memory.SetFlag(true, "limiter");
                }
                launchRTSSCheckbox.Enabled = false;
            }
        }
        private void LaunchRTSS_CheckChanged(object sender, EventArgs e) {
            var checkbox = (CheckBox) sender;
            if(checkbox.Checked && checkbox.Visible) {
                if(_rtssExecutable == string.Empty) {
                    if(!DetectRTSSExecutable()) {
                        System.Media.SystemSounds.Asterisk.Play();
                        MessageBox.Show("Could not detect Rivatuner Statistics Server on this system.\n" +
                            "Please select your RTSS.exe installation.", "RTSS Not Detected");
                        using OpenFileDialog dialog = new();
                        dialog.InitialDirectory = @"C:\";
                        dialog.Filter = "Executable Files (*.exe)|*.exe";
                        dialog.RestoreDirectory = true;
                        bool first = true;
                        string path = "";
                        while(!path.EndsWith("RTSS.exe")) {
                            if(!first) {
                                System.Media.SystemSounds.Asterisk.Play();
                                MessageBox.Show("Incorrect file. Please select RTSS.exe or hit cancel if you don't have RTSS installed.", "File Mismatch");
                                dialog.InitialDirectory = dialog.FileName[..dialog.FileName.LastIndexOf('\\')];
                            }
                            if(dialog.ShowDialog() == DialogResult.OK) {
                                Debug.WriteLine(dialog.FileName);
                                path = dialog.FileName;
                            } else {
                                checkbox.Checked = false;
                                return;
                            }
                            first = false;
                        }
                        _rtssExecutable = path;
                    }
                }
                if(Process.GetProcesses().ToList().FindAll(p => p.ProcessName.ToLower().Contains("rtss")).Count == 0) {
                    LaunchRTSS();
                }
            }
        }
        private void AA_CheckChanged(object sender, EventArgs e) {
            if(Hooked) Memory.SetCVAR(!((CheckBox) sender).Checked, "antialiasing");
        }
        private void UNDelay_CheckChanged(object sender, EventArgs e) {
            if(Hooked) Memory.SetCVAR(!((CheckBox) sender).Checked, "undelay");
        }
        private void AutoContinue_CheckChanged(object sender, EventArgs e) {
            if(Hooked) Memory.SetCVAR(((CheckBox) sender).Checked, "autocontinue");
        }
        private void MinimalOSD_CheckChanged(object sender, EventArgs e) {
            if(Hooked) Memory.SetFlag(((CheckBox) sender).Checked, "minimal");
        }
        private void DisableOSD_CheckChanged(object sender, EventArgs e) {
            var check = ((CheckBox) sender).Checked;
            if(Hooked) Memory.EnableOSD = check;
            minimalOSDCheckbox.Enabled = check;
            if(!check && !_justLaunched) {
                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show("Speedruns submitted to the leaderboards will not be valid if the DESRU On-Screen Display is not visible in-game.\n\n" +
                    "If you plan on submitting, make sure to re-enable this option before starting your runs.", "Disabling On-Screen Display");
            }
        }
        private void ScalingMethod_CheckChanged(object sender, EventArgs e) {
            if(Hooked) {
                Memory.UseDynamicScaling = dynScalingRadioButton.Checked;
                if(Memory.ReadDynamicRes() || Memory.ReadForceRes() > 0f) Memory.EnableResolutionScaling();
            }
        }
        #endregion

        #region Buttons
        private void ExitButton_Click(object sender, EventArgs e) => this.Close();
        private void HelpButton_Click(object sender, EventArgs e) {
            helpButton.Enabled = false;
            new HelpPage().ShowDialog(this);
        }
        private void SettingsButton_Click(object sender, EventArgs e) {
            settingsButton.Enabled = false;
            SettingsWindow = new SettingsPage();
            SettingsWindow.ShowDialog(this);
        }
        private void UnlockRes_Click(object sender, EventArgs e) {
            if(Hooked) Memory.ScheduleResUnlock(autoScalingCheckbox.Checked, _targetFPS);
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
            try {
                Directory.Move(_gameDirectory, _gameDirectory + " " + current);
            } catch(IOException ioe) {
                Log.Error(ioe, "Failed to change game versions.");
                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show("Failed to swap game versions.\n" +
                    "If this problem persists, you may need to restart your system to fix it.\n" +
                    "If you currently cannot restart your system, you can still manually rename the folders to swap versions.", "Failed to Swap Versions");
                return;
            }
            Directory.Move(_gameDirectory + " " + desired, _gameDirectory);
            Log.Information("Game Version changed to [{Version}]", desired);
            if(_steamID3 != string.Empty && replaceProfileCheckbox.Checked) {
                var dir = _steamInstallation + "\\userdata\\" + _steamID3 + PATH_PROFILE_DIR;
                try {
                    if(desired == "3.1") {
                        File.Move(dir + PATH_PROFILE_FILE, dir + "\\main.bin");
                        if(File.Exists(dir + "\\3.1.bin")) File.Move(dir + "\\3.1.bin", dir + PATH_PROFILE_FILE);
                    } else {
                        if(current == "3.1") {
                            if(File.Exists(dir + "\\main.bin")) {
                                if(File.Exists(dir + PATH_PROFILE_FILE)) File.Move(dir + PATH_PROFILE_FILE, dir + "\\3.1.bin");
                                File.Move(dir + "\\main.bin", dir + PATH_PROFILE_FILE);
                            } else {
                                File.Copy(dir + PATH_PROFILE_FILE, dir + "\\3.1.bin");
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
                versionChangedLabel.ForeColor = COLOR_PANEL_BACK;
            });
        }
        private void FirewallToggle_Click(object sender, EventArgs e) {
            if(_fwRuleExists) {
                FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", true);
                foreach(string dir in _extraGameDirectories) FirewallHandler.CheckForFirewallRule(dir + "\\DOOMEternalx64vk.exe", true);
                firewallToggleButton.Text = "Create Firewall Rule";
                if(_fwRestart) _fwRestart = false;
                _fwRuleExists = false;
                if(Hooked) {
                    Memory.SetFlag(_fwRuleExists, "firewall");
                }
                firewallRestartLabel.ForeColor = COLOR_PANEL_BACK;
            } else {
                if(!FirewallHandler.CheckForFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", false))
                    FirewallHandler.CreateFirewallRule(_gameDirectory + "\\DOOMEternalx64vk.exe", 0);
                for(int i = 0; i < _extraGameDirectories.Count; i++) {
                    if(!FirewallHandler.CheckForFirewallRule(_extraGameDirectories[i] + "\\DOOMEternalx64vk.exe", false))
                        FirewallHandler.CreateFirewallRule(_extraGameDirectories[i] + "\\DOOMEternalx64vk.exe", i + 1);
                }
                firewallToggleButton.Text = "Remove Firewall Rule";
                _fwRestart = true;
                _fwRuleExists = true;
                if(Hooked) firewallRestartLabel.ForeColor = COLOR_TEXT_FORE;
            }
        }
        private void CheatsToggle_Click(object sender, EventArgs e) {
            if(Hooked) {
                Memory.EnableCheats = true;
                cheatsToggleButton.Enabled = false;
                cheatsToggleButton.Text = "Restart Game to Disable Cheats";
                return;
            }
            if(_mhExists) {
                UninstallMeathook();
            } else {
                InstallMeathook();
            }
        }
        private void MoreKeysButton_Click(object sender, EventArgs e) {
            if(extraFPSHotkeysPanel.Visible) {
                if(!_smallDisplay) {
                    this.Height -= WINDOW_EXTRAHEIGHT_MOREKEYS;
                    collapsiblePanel.Height -= PANEL_EXTRAHEIGHT_MOREKEYS;
                }
                extraFPSHotkeysPanel.Visible = false;
                _moreHotkeysLabel.Visible = false;
                showMoreKeysButton.Text = "Show More Hotkeys";
            } else {
                if(!_smallDisplay) {
                    this.Height += WINDOW_EXTRAHEIGHT_MOREKEYS;
                    collapsiblePanel.Height += PANEL_EXTRAHEIGHT_MOREKEYS;
                }
                extraFPSHotkeysPanel.Visible = true;
                _moreHotkeysLabel.Visible = true;
                showMoreKeysButton.Text = "Hide Extra Hotkeys";
            }
            ModifyWindowForSmallDisplays();
        }
        #endregion

        private void DropDown_IndexChanged(object sender, EventArgs e) {
            if(!Hooked) changeVersionButton.Enabled = ((ComboBox) sender).Text != GetCurrentVersion();
        }

        // Event method that runs upon loading of the MainWindow form.
        private void MainWindow_Load(object sender, EventArgs e) {
            MemoryHandler.OffsetList = JsonConvert.DeserializeObject<List<KnownOffsets>>(System.Text.Encoding.UTF8.GetString(Properties.Resources.offsets));
            if(File.Exists(@".\scannedOffsets.json")) MemoryHandler.ScannedOffsetList = JsonConvert.DeserializeObject<List<KnownOffsets>>(File.ReadAllText(@".\scannedOffsets.json"));
            if(File.Exists(@".\cheatOffsets.json")) MemoryHandler.CheatOffsetList = JsonConvert.DeserializeObject<List<CheatOffsets>>(File.ReadAllText(@".\cheatOffsets.json"));

            InitializeFonts();
            _moreHotkeysLabel = new DESRUShadowLabel(moreHotkeysTitle.Font, "EXTRA HOTKEYS", moreHotkeysTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK);
            _moreHotkeysLabel.Visible = false;
            _speedometer = new Speedometer(EternalUIRegular32, EternalUIRegular20, speedometerPanel.Location, speedometerPanel.Size, COLOR_TEXT_FORE, COLOR_PANEL_BACK);
            _speedometer.Visible = false;
            trainerPanel.Controls.Add(_speedometer);
            var titleBar = new DESRUShadowLabel(windowTitle.Font, WINDOW_TITLE, windowTitle.Location, Color.FromArgb(190, 34, 34), COLOR_FORM_BACK);
            titleBar.MouseMove += new MouseEventHandler(DragWindow_MouseMove);
            titleBar.MouseUp += new MouseEventHandler(DragWindow_MouseUp);
            this.Controls.Add(titleBar);
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(hotkeysTitle.Font, "KEYBINDS", hotkeysTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(optionsTitle.Font, "OPTIONS", optionsTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(versionTitle.Font, "CHANGE VERSION", versionTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(infoPanelTitle.Font, "INFO PANEL", infoPanelTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(resTitle.Font, "RESOLUTION SCALING", resTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(new DESRUShadowLabel(trainerTitle.Font, "TRAINER", trainerTitle.Location, COLOR_TEXT_FORE, COLOR_FORM_BACK));
            collapsiblePanel.Controls.Add(_moreHotkeysLabel);

            minimalOSDCheckbox.Enabled = true;

            // User Settings
            var fpsJson = "";
            if(File.Exists(PATH_FPSKEYS_JSON)) fpsJson = File.ReadAllText(PATH_FPSKEYS_JSON);
            _ = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
            _ = new HotkeyHandler((Keys) Properties.Settings.Default.ResScaleKey, (Keys) Properties.Settings.Default.ResToggleKey0,
                (Keys) Properties.Settings.Default.ResToggleKey1, (Keys) Properties.Settings.Default.ResToggleKey2,
                (Keys) Properties.Settings.Default.ResToggleKey3, (Keys) Properties.Settings.Default.ResetRunHotkey, fpsJson);
            MemoryHandler.DevConsoleKey = (KeyCode) Properties.Settings.Default.DevConsoleKey;
            _fpsDefault = Properties.Settings.Default.DefaultFPSCap;
            autorunMacroCheckbox.Checked = Properties.Settings.Default.MacroEnabled;
            _enableMacro = Properties.Settings.Default.MacroEnabled;
            enableHotkeysCheckbox.Checked = Properties.Settings.Default.HotkeysEnabled;
            _gameDirectory = Properties.Settings.Default.GameLocation;
            unlockOnStartupCheckbox.Checked = Properties.Settings.Default.StartupUnlock;
            autoScalingCheckbox.Checked = Properties.Settings.Default.AutoDynamic;
            _minResPercent = Properties.Settings.Default.MinResPercent;
            _targetFPS = Properties.Settings.Default.TargetFPSScaling;
            _steamInstallation = Properties.Settings.Default.SteamInstallation;
            _steamID3 = Properties.Settings.Default.SteamID3;
            replaceProfileCheckbox.Checked = Properties.Settings.Default.ReplaceProfile;
            enforce250FPSCheckbox.Checked = Properties.Settings.Default.EnableMaxFPSLimit;
            minimalOSDCheckbox.Checked = Properties.Settings.Default.MinimalOSD;
            trainerOSDCheckbox.Checked = Properties.Settings.Default.TrainerOSD;
            speedometerCheckbox.Checked = Properties.Settings.Default.ShowSpeedometer;
            speedometerPrecisionCheckbox.Checked = Properties.Settings.Default.IncreasedPrecision;
            hideDuringLoadsCheckbox.Checked = Properties.Settings.Default.HideSpeedometerDuringLoads;
            _resPercent0 = Properties.Settings.Default.ResTogglePercent0;
            _resPercent1 = Properties.Settings.Default.ResTogglePercent1;
            _resPercent2 = Properties.Settings.Default.ResTogglePercent2;
            _resPercent3 = Properties.Settings.Default.ResTogglePercent3;
            if(Properties.Settings.Default.UseDynamicScaling)
                dynScalingRadioButton.Checked = true;
            else
                staticScalingRadioButton.Checked = true;
            rightAlignCheckbox.Checked = Properties.Settings.Default.RightAlignSpeedometer;
            switch(Properties.Settings.Default.SecondaryVelocity) {
                case 0:
                    velocityRadioNone.Checked = true;
                    break;
                case 1:
                default:
                    velocityRadioTotal.Checked = true;
                    break;
                case 2:
                    velocityRadioVertical.Checked = true;
                    break;

            }
            _rtssExecutable = Properties.Settings.Default.RTSSPath;
            if(_rtssExecutable == string.Empty) DetectRTSSExecutable();
            launchRTSSCheckbox.Enabled = !Properties.Settings.Default.EnableMaxFPSLimit;
            launchRTSSCheckbox.Checked = Properties.Settings.Default.LaunchRTSS;
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
            this.Height = WINDOW_HEIGHT_DEFAULT;
            collapsiblePanel.Height = PANEL_HEIGHT_DEFAULT;
            if(!IsFormOnScreen() || loc == Point.Empty) Location = defaultLocation;
            ModifyWindowForSmallDisplays();
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

            File.WriteAllText(PATH_FPSKEYS_JSON, HotkeyHandler.Instance.FPSHotkeys.SerializeIntoJSON());

            Log.Information("User settings saved");

            HotkeyHandler.Instance.DisableHotkeys();
            FreescrollMacro.Instance.Stop(false);
            Memory?.ClosingDESRU();

            RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }

        private void MainWindow_Shown(object sender, EventArgs e) {
            if(Properties.Settings.Default.ShowFPSLimitWarning) {
                var dialog = new FPSLimitWarning(this.Location, this.Width, this.Height);
                Log.Information("Displayed FPS Limiter Warning to user.");
                if(dialog.ShowDialog() == DialogResult.Yes) {
                    _justLaunched = true;
                    enforce250FPSCheckbox.Checked = false;
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
