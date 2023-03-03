using Serilog;

namespace DESpeedrunUtil {
    public partial class HelpPage: Form {

        List<Button> _buttons;

        public HelpPage() {
            InitializeComponent();
            _buttons = new() {
                helpTabButton0,
                helpTabButton1,
                helpTabButton2,
                helpTabButton3,
                helpTabButton4,
                helpTabButton5,
                helpTabButton6,
                helpTabButton7,
            };

            SetFonts();
        }

        private void SetFonts() {
            foreach(Button b in _buttons) b.Font = MainWindow.EternalUIBold;
            closeButton.Font = MainWindow.EternalUIBold;
            helpTextbox.Font = MainWindow.EternalUIRegular;
            titleText.Font = MainWindow.EternalUIRegular;
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void TabButton_Click(object sender, EventArgs e) {
            var button = (Button) sender;
            string tag = (string) button.Tag;
            button.Enabled = false;

            helpTextbox.Text = tag switch {
                "macro" => Properties.Resources.HelpPage_Macro,
                "hk" => Properties.Resources.HelpPage_Keybinds,
                "res" => Properties.Resources.HelpPage_ResScaling,
                "ver" => Properties.Resources.HelpPage_VersionChanger,
                "option" => Properties.Resources.HelpPage_Options,
                "info" => Properties.Resources.HelpPage_InfoPanel,
                "osd" => Properties.Resources.HelpPage_OSD,
                "trainer" => Properties.Resources.HelpPage_Trainer,
                _ => ""
            };

            helpTextbox.ScrollBars = (tag == "option" || tag == "trainer") ? ScrollBars.Vertical : ScrollBars.None;

            helpPageVersionImage.Visible = tag == "ver";
            helpPageOSDImage.Visible = tag == "osd";

            foreach(Button b in _buttons)
                if((string) b.Tag != tag)
                    b.Enabled = true;
        }

        private void HelpPage_Load(object sender, EventArgs e) {
            foreach(Button b in _buttons)
                b.Enabled = true;
            blankPanel.Visible = true;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
            Log.Information("Loaded Help Page");
        }
    }
}
