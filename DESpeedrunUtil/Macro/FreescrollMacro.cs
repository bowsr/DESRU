using System.Diagnostics;

namespace DESpeedrunUtil.Macro {
    internal class FreescrollMacro {
        private readonly string bindingsFile = @".\macro\bindings.txt";

        private readonly string downOnlyFormat = "0x{0:X2}";
        private readonly string upOnlyFormat = "0x{0:X2} Up";
        private readonly string downAndUpFormat = "0x{0:X2} 0x{1:X2}";

        private readonly ProcessStartInfo macroStartInfo;
        private Process macroProcess = null;

        public bool IsRunning { get; private set; }
        public Keys DownScrollKey { get; private set; }
        public Keys UpScrollKey { get; private set; }

        public FreescrollMacro(Keys downScroll, Keys upScroll) {
            macroStartInfo = new ProcessStartInfo(@".\DOOMEternalMacro.exe");
            macroStartInfo.WorkingDirectory = @".\macro";
            macroStartInfo.UseShellExecute = true;
            macroStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            IsRunning = false;

            DownScrollKey = downScroll;
            UpScrollKey = upScroll;
            CreateBindingsFile();
        }

        public bool CanStart() => macroProcess == null && (DownScrollKey != Keys.None || UpScrollKey != Keys.None);
        public void Start() {
            if(!CanStart()) return;
            macroProcess = Process.Start(macroStartInfo);
            IsRunning = true;
        }
        public void Stop() {
            if(macroProcess == null) return;
            macroProcess.Kill();
            macroProcess = null;
            IsRunning = false;
        }
        public void Restart() {
            Stop();
            Start();
        }

        public void ChangeHotkey(Keys newKey, bool downKey) {
            if(downKey) DownScrollKey = newKey;
            else UpScrollKey = newKey;

            CreateBindingsFile();
            if(IsRunning) Restart();
        }

        private void CreateBindingsFile() {
            string binds;

            if(DownScrollKey == Keys.None && UpScrollKey != Keys.None) binds = string.Format(upOnlyFormat, (int) UpScrollKey);
            else if(DownScrollKey != Keys.None && UpScrollKey == Keys.None) binds = string.Format(downOnlyFormat, (int) DownScrollKey);
            else binds = string.Format(downAndUpFormat, (int) DownScrollKey, (int) UpScrollKey);

            File.WriteAllText(bindingsFile, binds);
        }
    }
}
