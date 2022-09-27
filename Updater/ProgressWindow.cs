namespace Updater {
    public partial class ProgressWindow: Form {

        private const string GITHUB_DL_URI = "https://github.com/bowsr/DESRU/releases/download/{0}/DESRU.zip";

        private string _downloadURI;

        public ProgressWindow() {
            InitializeComponent();

            _downloadURI = string.Format(GITHUB_DL_URI, Program.NewVersion);
        }
    }
}