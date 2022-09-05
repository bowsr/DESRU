using System.Diagnostics;

namespace DESpeedrunUtil {
    public partial class UserdataDialog: Form {

        public string ID3 { get; private set; }

        public UserdataDialog(List<string> profileIDs) {
            InitializeComponent();

            profileSelector.Items.Clear();
            profileSelector.Items.AddRange(profileIDs.ToArray());
            profileSelector.SelectedIndex = 0;
            ID3 = profileSelector.Items[0].ToString();
        }

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(new ProcessStartInfo("https://steamid.io") { UseShellExecute = true });
        }

        private void profileSelector_SelectedIndexChanged(object sender, EventArgs e) {
            ID3 = ((ComboBox) sender).SelectedItem.ToString();
            Debug.WriteLine(ID3);
        }

        private void UserdataDialog_Load(object sender, EventArgs e) {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
