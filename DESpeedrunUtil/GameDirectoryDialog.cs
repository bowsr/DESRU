using System.Diagnostics;

namespace DESpeedrunUtil {
    public partial class GameDirectoryDialog: Form {
        public string FileName { get; private set; }
        public GameDirectoryDialog() {
            InitializeComponent();
            this.Icon = Properties.Resources.DESRU;
        }

        private void SelectFolder_Click(object sender, EventArgs e) {
            var folder = string.Empty;
            using OpenFileDialog dialog = new();
            dialog.InitialDirectory = (pathTextBox.Text != string.Empty) ? pathTextBox.Text : @"C:\";
            dialog.Filter = "Executable Files (*.exe)|*.exe";
            dialog.RestoreDirectory = true;

            if(dialog.ShowDialog() == DialogResult.OK) {
                folder = dialog.FileName;
                Debug.WriteLine(folder);
                pathTextBox.Text = folder;
                if(folder.EndsWith(@"DOOMEternal\DOOMEternalx64vk.exe")) {
                    confirmButton.Enabled = true;
                    errorLabel.ForeColor = Color.FromKnownColor(KnownColor.Control);
                } else {
                    confirmButton.Enabled = false;
                    errorLabel.ForeColor = Color.LightCoral;
                }
            }
            FileName = folder;
        }

        private void GameDirectoryDialog_Load(object sender, EventArgs e) {
            System.Media.SystemSounds.Asterisk.Play();
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
