using System.ComponentModel;
using System.IO.Compression;

namespace Updater {
    public partial class ProgressWindow: Form {

        private const string GITHUB_DL_URI = "https://github.com/bowsr/DESRU/releases/download/{0}/DESRU.zip";
        private const string UPDATE_DIR = @".\updateFiles";

        private Uri _downloadURI;
        private string _zipName;

        private int _totalFiles = 0;
        private int _installedFiles = 0;

        public ProgressWindow() {
            InitializeComponent();

            var v = Program.NewVersion;
            _downloadURI = new Uri(string.Format(GITHUB_DL_URI, v));
            _zipName = string.Format(@".\desru_{0}.zip", v);
            progressLabel.Text = string.Format("Downloading DESRU {0}", v);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            if(Directory.Exists(UPDATE_DIR)) Directory.Delete(UPDATE_DIR, true);
            InvokeLabelChangeText("Extracting files");
            ZipFile.ExtractToDirectory(_zipName, UPDATE_DIR);
            backgroundWorker.ReportProgress(updateProgressBar.Value + updateProgressBar.Step);
            _totalFiles = CountFiles(UPDATE_DIR);
            MoveFiles(UPDATE_DIR);
        }

        private void BackgroundWorker_ProgessChanged(object sender, ProgressChangedEventArgs e) {
            updateProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            progressLabel.Text = "Installation Complete";
            openDESRUButton.Enabled = true;
            File.Delete(_zipName);
        }

        private void LaunchDESRU_Click(object sender, EventArgs e) {
            Program.LaunchOnClose = true;
            this.Close();
        }

        private void ProgressWindow_Load(object sender, EventArgs e) {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }

        private void ProgressWindow_Shown(object sender, EventArgs e) {
            Download();
        }

        private async void Download() {
            try {
                var response = await new HttpClient().GetAsync(_downloadURI);
                using var fs = new FileStream(_zipName, FileMode.Create);
                if(response.IsSuccessStatusCode) {
                    await response.Content.CopyToAsync(fs);
                    updateProgressBar.Style = ProgressBarStyle.Continuous;
                    backgroundWorker.RunWorkerAsync();
                } else {
                    progressLabel.Text = "Update Failed to Download";
                }
            } catch(Exception) {
                progressLabel.Text = "An error occurred when attempting to download update";
            }
        }

        private void CreateDirectories(string path) {
            foreach(string dir in Directory.GetDirectories(path)) {
                Directory.CreateDirectory(dir.Replace("updateFiles\\", ""));
                CreateDirectories(dir);
            }
        }

        private void MoveFiles(string path) {
            CreateDirectories(path);
            foreach(string dir in Directory.GetDirectories(path)) {
                MoveFiles(dir);
                Directory.Delete(dir);
            }
            foreach(string file in Directory.GetFiles(path)) {
                if(file[(file.LastIndexOf('\\') + 1)..].StartsWith("Updater.")) continue;
                InvokeLabelChangeText(string.Format("Installing {0}", file[(file.LastIndexOf('\\') + 1)..]));
                File.Move(file, file.Replace("updateFiles\\", ""), true);
                _installedFiles++;
                backgroundWorker.ReportProgress((_installedFiles * 100) / _totalFiles);
            }

        }

        private int CountFiles(string path) {
            var count = 0;
            foreach(string dir in Directory.GetDirectories(path)) {
                count += CountFiles(dir);
            }
            foreach(string file in Directory.GetFiles(path)) {
                if(file[(file.LastIndexOf('\\') + 1)..].StartsWith("Updater.")) continue;
                count++;
            }
            return count;
        }

        private void InvokeLabelChangeText(string text) {
            progressLabel.Invoke((MethodInvoker) delegate {
                progressLabel.Text = text;
            });
        }
    }
}