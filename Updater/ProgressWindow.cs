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
            Directory.CreateDirectory(@".\fonts");
            Directory.CreateDirectory(@".\macro");
            Directory.CreateDirectory(@".\meath00k");
            MoveFiles(UPDATE_DIR);
        }

        private void BackgroundWorker_ProgessChanged(object sender, ProgressChangedEventArgs e) {
            updateProgressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            progressLabel.Text = "Installation Complete";
            openDESRUButton.Enabled = true;
        }

        private void ProgressWindow_Shown(object sender, EventArgs e) {
            Download();
        }

        private async void Download() {
            var response = await new HttpClient().GetAsync(_downloadURI);
            using var fs = new FileStream(_zipName, FileMode.Create);
            await response.Content.CopyToAsync(fs);
            updateProgressBar.PerformStep();
            backgroundWorker.RunWorkerAsync();
        }

        private void MoveFiles(string path) {
            foreach(string dir in Directory.GetDirectories(path)) {
                MoveFiles(dir);
                Directory.Delete(dir);
            }
            foreach(string file in Directory.GetFiles(path)) {
                if(file[file.LastIndexOf('\\')..].StartsWith("Updater")) continue;
                InvokeLabelChangeText(string.Format("Installing {0}", file[file.LastIndexOf('\\')..]));
                File.Move(file, file.Replace("updateFiles\\", ""), true);
                _installedFiles++;
                backgroundWorker.ReportProgress(10 + ((_installedFiles * 90) / _totalFiles));
            }
            
        }

        private int CountFiles(string path) {
            var count = 0;
            foreach(string dir in Directory.GetDirectories(path)) {
                count += CountFiles(dir);
            }
            foreach(string file in Directory.GetFiles(path)) {
                if(!file[file.LastIndexOf('\\')..].StartsWith("Updater")) count++;
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