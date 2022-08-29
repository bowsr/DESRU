namespace DESpeedrunUtil {
    public partial class UpdateDialog: Form {
        public UpdateDialog(string newVersion) {
            InitializeComponent();
            versionLabel.Text = string.Format("{0} -> {1}", Program.APP_VERSION, newVersion);
        }

        private void UpdateDialog_Load(object sender, EventArgs e) {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
