namespace DESpeedrunUtil {
    public partial class UpdateDialog: Form {
        public UpdateDialog(string newVersion) {
            InitializeComponent();
            versionLabel.Text = string.Format("{0} -> {1}", Program.APP_VERSION, newVersion);
        }
    }
}
