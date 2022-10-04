namespace DESpeedrunUtil {
    public partial class FPSLimitWarning: Form {

        private Point _windowLocation;

        public FPSLimitWarning(Point location, int w, int h) {
            InitializeComponent();
            _windowLocation = new Point(
                location.X + ((w / 2) - this.Width / 2),
                location.Y + ((h / 2) - this.Height / 2));
        }

        private void FPSLimitWarning_Load(object sender, EventArgs e) {
            this.Location = _windowLocation;
        }
    }
}
