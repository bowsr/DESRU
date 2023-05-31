using DESpeedrunUtil.Memory;

namespace DESpeedrunUtil {
    internal partial class SettingsPage: Form {

        MemoryHandler? _memory;
        MainWindow _parent;

        private bool _enableEvents = false;

        public SettingsPage(MemoryHandler? memory, MainWindow parent) {
            InitializeComponent();
            _memory = memory;
            _parent = parent;
            settingsAACheckbox.Checked = !Properties.Settings.Default.AntiAliasing;
            settingsUNCheckbox.Checked = !Properties.Settings.Default.UNDelay;
            settingsAutoContinueCheckbox.Checked = Properties.Settings.Default.AutoContinue;
            SetFonts();
            _enableEvents = true;
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e) {
            if(!_enableEvents) return;
            var cb = (CheckBox) sender;
            string tag = cb.Tag.ToString();
            bool state = tag switch {
                "antialiasing" or "undelay" => !cb.Checked,
                "autocontinue" => cb.Checked,
                _ => false
            };
            _memory?.SetCVAR(state, tag);
            UpdateSettings();
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            UpdateSettings();
            this.Close();
        }

        private void UpdateSettings() {
            Properties.Settings.Default.AntiAliasing = !settingsAACheckbox.Checked;
            Properties.Settings.Default.UNDelay = !settingsUNCheckbox.Checked;
            Properties.Settings.Default.AutoContinue = settingsAutoContinueCheckbox.Checked;
        }

        private void SetFonts() {
            settingsCVARLabel.Font = MainWindow.EternalUIRegular20;
            settingsAACheckbox.Font = MainWindow.EternalUIRegular;
            settingsUNCheckbox.Font = MainWindow.EternalUIRegular;
            settingsAutoContinueCheckbox.Font = MainWindow.EternalUIRegular;
            settingsCloseButton.Font = MainWindow.EternalUIRegular;
            settingsAADescription.Font = MainWindow.EternalUIRegular10;
            settingsUNDescription.Font = MainWindow.EternalUIRegular10;
            settingsAutoContinueDescription.Font = MainWindow.EternalUIRegular10;
        }

        private void SettingsPage_Load(object sender, EventArgs e) {
            this.Location = new Point(_parent.Location.X + 15, _parent.Location.Y + 50);
        }

        private void SettingsPage_FormClosing(object sender, FormClosingEventArgs e) {
            _parent?.ToggleButtonStates("settings", true);
        }
    }
}
