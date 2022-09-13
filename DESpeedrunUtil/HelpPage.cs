using Serilog;

namespace DESpeedrunUtil {
    public partial class HelpPage: Form {

        List<Panel> _infoPanels;
        List<Button> _buttons;

        public HelpPage() {
            InitializeComponent();
            _infoPanels = new() {
                macroPanel,
                keybindPanel,
                resPanel,
                versionPanel,
                optionPanel,
                infoPanel,
                osdPanel
            };
            _buttons = new() {
                helpTabButton0,
                helpTabButton1,
                helpTabButton2,
                helpTabButton3,
                helpTabButton4,
                helpTabButton5,
                helpTabButton6,
            };

            SetFonts();
        }

        private void SetFonts() {
            foreach(Button b in _buttons) b.Font = MainWindow.EternalUIBold;
            closeButton.Font = MainWindow.EternalUIBold;
            helpText0.Font = MainWindow.EternalUIRegular;
            helpText1.Font = MainWindow.EternalUIRegular;
            helpText2.Font = MainWindow.EternalUIRegular;
            helpText3.Font = MainWindow.EternalUIRegular;
            helpText4.Font = MainWindow.EternalUIRegular;
            helpText5.Font = MainWindow.EternalUIRegular;
            helpText6.Font = MainWindow.EternalUIRegular;
            titleText.Font = MainWindow.EternalUIRegular;
        }

        private void CloseButton_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void TabButton_Click(object sender, EventArgs e) {
            var button = (Button) sender;
            string tag = (string) button.Tag;
            blankPanel.Visible = false;
            button.Enabled = false;

            foreach(Button b in _buttons)
                if((string) b.Tag != tag)
                    b.Enabled = true;

            foreach(Panel p in _infoPanels)
                p.Visible = (string) p.Tag == tag;
        }

        private void HelpPage_Load(object sender, EventArgs e) {
            var loc = blankPanel.Location;
            foreach(Button b in _buttons) 
                b.Enabled = true;
            foreach(Panel p in _infoPanels) {
                p.Location = loc;
                p.Visible = false;
            }
            blankPanel.Visible = true;
            this.Width = 800;
            this.Height = 700;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Left + (Screen.PrimaryScreen.WorkingArea.Width / 2 - (this.Width / 2)),
                Screen.PrimaryScreen.WorkingArea.Top + (Screen.PrimaryScreen.WorkingArea.Height / 2) - (this.Height / 2));
            Log.Information("Loaded Help Page");
        }
    }
}
