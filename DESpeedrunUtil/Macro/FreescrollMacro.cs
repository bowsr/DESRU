using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil.Macro {
    internal class FreescrollMacro {
        private readonly string bindingsFile = @".\macro\bindings.txt";

        private readonly string downOnlyFormat = "0x{0:X2}";
        private readonly string upOnlyFormat = "0x{0:X2} Up";
        private readonly string downAndUpFormat = "0x{0:X2} 0x{1:X2}";

        private readonly ProcessStartInfo macroStartInfo;
        private Process macroProcess = null;

        private Timer timer;

        public bool IsRunning { get; private set; }
        public Keys DownScrollKey { get; private set; }
        public Keys UpScrollKey { get; private set; }

        public FreescrollMacro(Keys downScroll, Keys upScroll) {
            macroStartInfo = new ProcessStartInfo(@".\DOOMEternalMacro.exe");
            macroStartInfo.WorkingDirectory = @".\macro";
            macroStartInfo.UseShellExecute = true;
            macroStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            IsRunning = false;

            // Timer that runs every five seconds to prevent unmanaged macro processes
            timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += new EventHandler(MacroCheck);

            DownScrollKey = downScroll;
            UpScrollKey = upScroll;
            CreateBindingsFile();
        }

        public bool CanStart() => macroProcess == null && (DownScrollKey != Keys.None || UpScrollKey != Keys.None);
        public void Start() {
            if(!CanStart()) return;
            TerminateUnmanagedMacros(); // One final check before running our own macro instance
            macroProcess = Process.Start(macroStartInfo);
            IsRunning = true;
            if(timer.Enabled) timer.Stop();
        }
        public void Stop() {
            if(macroProcess == null) return;
            macroProcess.Kill();
            macroProcess = null;
            IsRunning = false;
            timer.Start();
        }
        private void StopNoTimer() {
            if(macroProcess == null) return;
            macroProcess.Kill();
            macroProcess = null;
            IsRunning = false;
        }
        public void Restart() {
            StopNoTimer();
            Start();
        }

        public void ChangeHotkey(Keys newKey, bool downKey) {
            if(downKey) DownScrollKey = newKey;
            else UpScrollKey = newKey;

            CreateBindingsFile();
            if(IsRunning) Restart(); // Macro is restarted for binding changes to take place
        }

        private void CreateBindingsFile() {
            string binds;

            if(DownScrollKey == Keys.None && UpScrollKey != Keys.None) binds = string.Format(upOnlyFormat, (int) UpScrollKey);
            else if(DownScrollKey != Keys.None && UpScrollKey == Keys.None) binds = string.Format(downOnlyFormat, (int) DownScrollKey);
            else binds = string.Format(downAndUpFormat, (int) DownScrollKey, (int) UpScrollKey);

            File.WriteAllText(bindingsFile, binds);
        }
        /// <summary>
        /// Timer method that periodically terminates any Macro processes.
        /// </summary>
        private void MacroCheck(object sender, EventArgs e) {
            if(macroProcess == null) TerminateUnmanagedMacros(); // Prevents the user from running the macro outside the scope of this utility
        }
        /// <summary>
        /// Terminates any DOOMEternalMacro.exe processes that are running outside the scope of this utility.
        /// </summary>
        public static void TerminateUnmanagedMacros() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalMacro"));
            if(procList.Count == 0) return;
            foreach(Process proc in procList) proc.Kill();
        }
    }
}
