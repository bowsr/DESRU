using Markdig;

namespace DESpeedrunUtil {
    public partial class UpdateDialog: Form {
        public UpdateDialog(string newVersion, string changelog) {
            InitializeComponent();
            changelogWebViewer.EnsureCoreWebView2Async(null, null);
            changelogWebViewer.CoreWebView2InitializationCompleted += (sender, e) => {
                changelogWebViewer.CoreWebView2.NavigateToString(string.Format("<font face=\"Segoe UI\">{0}</font>", Markdown.ToHtml(changelog)));
            };
            versionLabel.Text = string.Format("{0} -> {1}", Program.APP_VERSION, newVersion);
        }

        private void UpdateDialog_Load(object sender, EventArgs e) {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
