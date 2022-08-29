namespace DESpeedrunUtil {
    public partial class HelpPage: Form {
        public HelpPage() {
            InitializeComponent();
            hpTextBox0.Font = MainWindow.EternalUIRegular;
            hpTextBox1.Font = MainWindow.EternalUIRegular;
            hpTextBox2.Font = MainWindow.EternalUIRegular;
            hpTextBox3.Font = MainWindow.EternalUIRegular;
            instructionsLabel.Font = MainWindow.EternalLogoBold;
            closeButton.Font = MainWindow.EternalUIBold;
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void HelpPage_Load(object sender, EventArgs e) {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
        }
    }
}
