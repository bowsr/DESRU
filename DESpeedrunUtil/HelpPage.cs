namespace DESpeedrunUtil {
    public partial class HelpPage: Form {
        public HelpPage() {
            InitializeComponent();
            closeButton.Font = MainWindow.EternalUIBold;
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void HelpPage_Load(object sender, EventArgs e) {
            this.Width = 800;
            this.Height = 700;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
