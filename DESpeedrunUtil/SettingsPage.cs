using DESpeedrunUtil.Memory;

namespace DESpeedrunUtil {
    internal partial class SettingsPage: Form {

        MemoryHandler? _memory;

        private bool _enableEvents = false;

        public SettingsPage(MemoryHandler? memory) {
            InitializeComponent();
            _memory = memory;
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

        private void SettingsPage_Load(object sender, EventArgs e) {
            this.Location = new Point(MainWindow.Instance.Location.X + 100, MainWindow.Instance.Location.Y + 150);
        }

        private void SettingsPage_FormClosing(object sender, FormClosingEventArgs e) {
            MainWindow.Instance.ToggleButtonStates("settings", true);
            MainWindow.Instance.SettingsWindow = null;
        }

        public void UpdateMemoryHandler(MemoryHandler memory) => _memory = memory;

        private void UpdateSettings() {
            Properties.Settings.Default.AntiAliasing = !settingsAACheckbox.Checked;
            Properties.Settings.Default.UNDelay = !settingsUNCheckbox.Checked;
            Properties.Settings.Default.AutoContinue = settingsAutoContinueCheckbox.Checked;
        }

        private void SetFonts() {
            settingsCVARLabel.Font = MainWindow.EternalUIBold20;
            settingsAACheckbox.Font = MainWindow.EternalUIRegular;
            settingsUNCheckbox.Font = MainWindow.EternalUIRegular;
            settingsAutoContinueCheckbox.Font = MainWindow.EternalUIRegular;
            settingsCloseButton.Font = MainWindow.EternalUIBold;
            settingsAADescription.Font = MainWindow.EternalUIRegular10;
            settingsUNDescription.Font = MainWindow.EternalUIRegular10;
            settingsAutoContinueDescription.Font = MainWindow.EternalUIRegular10;
        }
    }
}
