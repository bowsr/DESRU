using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Macro;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil {
    public partial class MainWindow: Form {
        Process GameProcess;
        public bool Hooked = false;

        FreescrollMacro MacroProcess;

        Timer FormTimer;

        bool HKAssignmentMode = false, Mouse1Pressed = false;
        Label SelectedHKField = null;
        string PrevHKFieldText = "";

        Keys[] InvalidKeys = { Keys.Escape, Keys.Oemtilde, Keys.LButton, Keys.RButton};

        public MainWindow() {
            InitializeComponent();

            FormTimer = new Timer();
            FormTimer.Interval = 8;
            FormTimer.Tick += new EventHandler(UpdateTick);

            this.KeyDown += new KeyEventHandler(HotkeyAssignment_KeyDown);
            this.MouseDown += new MouseEventHandler(HotkeyAssignment_MouseDown);
            this.FormClosing += new FormClosingEventHandler(MainWindow_Closing);
            this.Load += new EventHandler(MainWindow_Load);
            hotkeyField1.Click += new EventHandler(HotkeyAssignment_FieldSelected);
            hotkeyField2.Click += new EventHandler(HotkeyAssignment_FieldSelected);

            AddMouseIntercepts(this);
        }

        private void UpdateTick(object sender, EventArgs e) {
            if(GameProcess == null || GameProcess.HasExited) {
                GameProcess = null;
                Hooked = false;
                MacroProcess.Stop();
            }

            macroStatusText.Text = (MacroProcess.IsRunning) ? "Macro Enabled" : "Macro Disabled";

            if(!Hooked) Hooked = Hook();
            if(!Hooked) return;

            MacroProcess.Start();
        }

        private void HotkeyAssignment_KeyDown(object sender, KeyEventArgs e) {
            if(!HKAssignmentMode) return;
            Keys pressedKey;

            if(e.Control) pressedKey = HotkeyHandler.ModKeySelector(0);
            else if(e.Shift) pressedKey = HotkeyHandler.ModKeySelector(1);
            else if(e.Alt) pressedKey = HotkeyHandler.ModKeySelector(2);
            else pressedKey = e.KeyCode;
            bool isValid = !InvalidKeys.Contains(pressedKey);

            if(SelectedHKField != null) {
                SelectedHKField.Text = (isValid) ? HotkeyHandler.TranslateKeyNames(pressedKey) : PrevHKFieldText;
                SelectedHKField.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            string tag = (string) SelectedHKField.Tag;
            HKAssignmentMode = false;
            SelectedHKField = null;
            if(isValid) {
                switch(tag) {
                    case "macroDown":
                        MacroProcess.ChangeHotkey(pressedKey, true);
                        break;
                    case "macroUp":
                        MacroProcess.ChangeHotkey(pressedKey, false);
                        break;
                }
            }
            e.Handled = true;
        }
        private void HotkeyAssignment_MouseDown(object sender, MouseEventArgs e) {
            if(!HKAssignmentMode) return;

            Keys pressedKey = HotkeyHandler.ConvertMouseButton(e.Button);
            bool isValid = !InvalidKeys.Contains(pressedKey);

            if(SelectedHKField != null) {
                SelectedHKField.Text = (isValid) ? HotkeyHandler.TranslateKeyNames(pressedKey) : PrevHKFieldText;
                SelectedHKField.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            string tag = (string) SelectedHKField.Tag;
            HKAssignmentMode = false;
            if(pressedKey == Keys.LButton && sender.Equals(SelectedHKField)) Mouse1Pressed = true;
            SelectedHKField = null;
            if(isValid) {
                switch(tag) {
                    case "macroDown":
                        MacroProcess.ChangeHotkey(pressedKey, true);
                        break;
                    case "macroUp":
                        MacroProcess.ChangeHotkey(pressedKey, false);
                        break;
                }
            }
        }

        private void HotkeyAssignment_FieldSelected(object sender, EventArgs e) {
            if(Mouse1Pressed) {
                Mouse1Pressed = false;
                return;
            }
            PrevHKFieldText = ((Label) sender).Text;
            ((Label) sender).Text = "Press a key";
            ((Label) sender).BackColor = Color.Gold;

            HKAssignmentMode = true;
            SelectedHKField = (Label) sender;
        }
        /// <summary>
        /// Event method that runs upon loading of the <c>MainWindow</c> form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e) {
            MacroProcess = new FreescrollMacro((Keys) Properties.Settings.Default.DownScrollKey, (Keys) Properties.Settings.Default.UpScrollKey);
            hotkeyField1.Text = HotkeyHandler.TranslateKeyNames(MacroProcess.DownScrollKey);
            hotkeyField2.Text = HotkeyHandler.TranslateKeyNames(MacroProcess.UpScrollKey);

            Point loc = Properties.Settings.Default.Location;
            if(loc != Point.Empty) Location = loc;

            FormTimer.Start();
        }
        /// <summary>
        /// Event method that runs upon closing of the <c>MainWindow</c> form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, FormClosingEventArgs e) {
            MacroProcess.Stop();

            Properties.Settings.Default.DownScrollKey = (int) MacroProcess.DownScrollKey;
            Properties.Settings.Default.UpScrollKey = (int) MacroProcess.UpScrollKey;
            if(WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = Location;
            else if(WindowState == FormWindowState.Minimized) Properties.Settings.Default.Location = RestoreBounds.Location;

            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Adds a MouseDown event to every control in the form, recursively.
        /// </summary>
        /// <param name="control"></param>
        private void AddMouseIntercepts(Control control) {
            foreach(Control c in control.Controls) {
                c.MouseDown += new MouseEventHandler(HotkeyAssignment_MouseDown);
                if(c.Controls.Count > 0) AddMouseIntercepts(c);
            }
        }
        /// <summary>
        /// Hooks into the DOOMEternalx64vk.exe process, then sets up pointers for memory reading/writing.
        /// </summary>
        /// <returns>Returns true if the hook is sucessful, otherwise returns false.</returns>
        public bool Hook() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalx64vk"));
            if(procList.Count == 0) {
                GameProcess = null;
                return false;
            }
            GameProcess = procList[0];

            if(GameProcess.HasExited) return false;

            try {
                int mainModuleSize = GameProcess.MainModule.ModuleMemorySize;
                // Pointers
                return true;
            }catch (Win32Exception ex) {
                Console.WriteLine(ex.ErrorCode);
                return false;
            }
        }
    }
}