﻿using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Memory;
using System.Buffers;
using WindowsInput.Events;
using static DESpeedrunUtil.Define.Constants;
using static DESpeedrunUtil.Interop.DLLImports;

namespace DESpeedrunUtil
{
    internal partial class SettingsPage: Form
    {

        bool _enableEvents = false;
        bool _hkAssignmentMode = false;

        Label? _selectedHKField;

        public SettingsPage() {
            InitializeComponent();
            settingsAACheckbox.Checked = !Properties.Settings.Default.AntiAliasing;
            settingsUNCheckbox.Checked = !Properties.Settings.Default.UNDelay;
            settingsAutoContinueCheckbox.Checked = Properties.Settings.Default.AutoContinue;
            settingsResetBindCheckbox.Checked = Properties.Settings.Default.EnableResetRunHotkey;
            settingsFontSizeCheckbox.Checked = Properties.Settings.Default.EnableFontSizeChange;
            settingsFontSlider.Value = Properties.Settings.Default.OSDFontSize;
            settingsFontSliderText.Text = (5 + (Properties.Settings.Default.OSDFontSize * 0.5)).ToString();
            settingsConsoleHotkeyCheckbox.Checked = Properties.Settings.Default.AdvancedKeypress;
            settingsManualAltTabCheckbox.Checked = Properties.Settings.Default.ManualAltTab;

            settingsFontSlider.Enabled = settingsFontSizeCheckbox.Checked;
            settingsManualAltTabCheckbox.Enabled = settingsResetBindCheckbox.Checked;
            UpdateOSDFontSize();
            UpdateHotkeyFields();
            SetFonts();
            _enableEvents = true;
        }

        #region EVENTS
        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            if((GetAsyncKeyState(Keys.LButton) & 0x01) == 1) {
                _selectedHKField = (Label) sender;
                _selectedHKField.Text = "Press a key";
                _selectedHKField.BackColor = Color.WhiteSmoke;
                _selectedHKField.ForeColor = Color.Black;
                this.ActiveControl = null;

                _hkAssignmentMode = true;
            }
        }
        private void HotkeyAssignment_KeyDown(object sender, KeyEventArgs e) {
            if(!_hkAssignmentMode) return;
            Keys pressedKey;

            if(e.Control) pressedKey = HotkeyHandler.ModKeySelector(0);
            else if(e.Shift) pressedKey = HotkeyHandler.ModKeySelector(1);
            else if(e.Alt) pressedKey = HotkeyHandler.ModKeySelector(2);
            else pressedKey = e.KeyCode;
            if(pressedKey == Keys.Escape) pressedKey = Keys.None;

            if(_selectedHKField.Tag.ToString().ToLower().Contains("hkconsole")) {
                MemoryHandler.DevConsoleKey = (KeyCode) pressedKey;
            } else {
                bool isValid = !INVALID_KEYS.Contains(pressedKey);

                if(isValid) {
                    HotkeyHandler.ChangeHotkeys(pressedKey, 7);
                }
                MainWindow.Instance.UpdateHotkeyAndInputFields();
            }

            UpdateHotkeyFields();

            _hkAssignmentMode = false;
            _selectedHKField = null;
            e.Handled = true;
        }

        private void CVARCheckbox_CheckedChanged(object sender, EventArgs e) {
            if(!_enableEvents) return;
            var cb = (CheckBox) sender;
            string tag = cb.Tag.ToString() ?? "null";
            bool state = tag switch {
                "antialiasing" or "undelay" => !cb.Checked,
                "autocontinue" => cb.Checked,
                _ => false
            };
            MainWindow.Instance.Memory?.SetCVAR(state, tag);
            UpdateSettings();
        }

        private void ResetRunCheckbox_CheckChanged(object sender, EventArgs e) {
            if(!_enableEvents) return;
            settingsManualAltTabCheckbox.Enabled = ((CheckBox) sender).Checked;
            UpdateSettings();
        }

        private void GenericCheckbox_CheckChanged(object sender, EventArgs eventArgs) {
            if(!_enableEvents) return;
            UpdateSettings();
        }

        private void FontSizeCheckbox_CheckChanged(object sender, EventArgs e) {
            if(!_enableEvents) return;
            if(settingsFontSizeCheckbox.Checked) {
                System.Media.SystemSounds.Asterisk.Play();
                MessageBox.Show("Make sure the On-Screen Display text is still fully legible in your recorded videos.\n" +
                    "If the text is not readable at any point, your run cannot be verified for the leaderboards", "Font Size Warning");
                UpdateOSDFontSize();
            } else {
                ResetOSDFontSize();
            }
            settingsFontSlider.Enabled = settingsFontSizeCheckbox.Checked;
            UpdateSettings();
        }

        private void FontSlider_Changed(object sender, EventArgs e) {
            var val = 5 + (settingsFontSlider.Value * 0.5f);
            settingsFontSliderText.Text = val switch {
                10 => "10",
                _ => val.ToString("0.0")
            };
            UpdateOSDFontSize();
            UpdateSettings();
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            UpdateSettings();
            this.Close();
        }

        private void SettingsPage_Load(object sender, EventArgs e) { }

        private void SettingsPage_FormClosing(object sender, FormClosingEventArgs e) {
            MainWindow.Instance.ToggleButtonStates("settings", true);
            MainWindow.Instance.SettingsWindow = null;
        }
        #endregion

        private void UpdateSettings() {
            var props = Properties.Settings.Default;
            props.AntiAliasing = !settingsAACheckbox.Checked;
            props.UNDelay = !settingsUNCheckbox.Checked;
            props.AutoContinue = settingsAutoContinueCheckbox.Checked;
            props.EnableResetRunHotkey = settingsResetBindCheckbox.Checked;
            props.EnableFontSizeChange = settingsFontSizeCheckbox.Checked;
            props.OSDFontSize = settingsFontSlider.Value;
            props.AdvancedKeypress = settingsConsoleHotkeyCheckbox.Checked;
            props.ManualAltTab = settingsManualAltTabCheckbox.Checked;
        }

        private void UpdateHotkeyFields() {
            settingsResetRunHotkeyField.Text = HotkeyHandler.TranslateKeyNames(HotkeyHandler.Instance.ResetRunHotkey);
            settingsResetRunConsoleKey.Text = HotkeyHandler.TranslateKeyNames((Keys) MemoryHandler.DevConsoleKey);
            settingsResetRunHotkeyField.ForeColor = COLOR_TEXT_FORE;
            settingsResetRunHotkeyField.BackColor = COLOR_TEXT_BACK;
            settingsResetRunConsoleKey.ForeColor = COLOR_TEXT_FORE;
            settingsResetRunConsoleKey.BackColor = COLOR_TEXT_BACK;
        }

        private void UpdateOSDFontSize() {
            if(settingsFontSizeCheckbox.Checked)
                MainWindow.Instance.Memory?.SetOSDFontSize(5 + (settingsFontSlider.Value * 0.5f));
            else
                ResetOSDFontSize();
        }

        private void ResetOSDFontSize() => MainWindow.Instance.Memory?.ResetOSDFontSize();

        private void SetFonts() {
            settingsCVARLabel.Font = MainWindow.EternalUIBold20;
            settingsHotkeyLabel.Font = MainWindow.EternalUIBold20;
            settingsFontSliderText.Font = MainWindow.EternalUIBold20;

            settingsAACheckbox.Font = MainWindow.EternalUIRegular;
            settingsUNCheckbox.Font = MainWindow.EternalUIRegular;
            settingsAutoContinueCheckbox.Font = MainWindow.EternalUIRegular;
            settingsResetBindCheckbox.Font = MainWindow.EternalUIRegular;
            settingsFontSizeCheckbox.Font = MainWindow.EternalUIRegular;
            settingsConsoleHotkeyCheckbox.Font = MainWindow.EternalUIRegular;
            settingsResetRunHotkeyField.Font = MainWindow.EternalUIRegular;
            settingsResetRunConsoleKey.Font = MainWindow.EternalUIRegular;
            settingsManualAltTabCheckbox.Font = MainWindow.EternalUIRegular;

            settingsCloseButton.Font = MainWindow.EternalUIBold;

            settingsAADescription.Font = MainWindow.EternalUIRegular10;
            settingsUNDescription.Font = MainWindow.EternalUIRegular10;
            settingsAutoContinueDescription.Font = MainWindow.EternalUIRegular10;
            settingsResetBindDescription.Font = MainWindow.EternalUIRegular10;
            settingsFontSizeDescription.Font = MainWindow.EternalUIRegular10;
            settingsConsoleHotkeyDescription.Font = MainWindow.EternalUIRegular10;
            settingsDevConsoleKeyDescription.Font = MainWindow.EternalUIRegular10;
        }
    }
}
