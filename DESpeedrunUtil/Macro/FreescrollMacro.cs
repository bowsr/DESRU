using Serilog;
using System.Diagnostics;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil.Macro {
    internal class FreescrollMacro {
        private const string BINDINGS_FILE = @".\macro\bindings.txt";

        private const string DOWN_ONLY_FORMAT = "0x{0:X2}";
        private const string UP_ONLY_FORMAT = "0x{0:X2} Up";
        private const string DOWN_UP_FORMAT = "0x{0:X2} 0x{1:X2}";

        private readonly ProcessStartInfo MACRO_START_INFO;
        private Process _macroProcess = null;

        private Timer _timer;

        public bool IsRunning { get; private set; }
        private Keys _downScrollKey { get; set; }
        private Keys _upScrollKey { get; set; }

        private bool _incorrectMacroVersion = false;

        public FreescrollMacro(Keys downScroll, Keys upScroll) {
            MACRO_START_INFO = new ProcessStartInfo(@".\DOOMEternalMacro.exe");
            MACRO_START_INFO.WorkingDirectory = @".\macro";
            MACRO_START_INFO.UseShellExecute = true;
            MACRO_START_INFO.WindowStyle = ProcessWindowStyle.Hidden;
            IsRunning = false;

            // Timer that runs every five seconds to prevent unmanaged macro processes
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Tick += new EventHandler(MacroCheck);
            _timer.Start();

            _downScrollKey = downScroll;
            _upScrollKey = upScroll;
            CreateBindingsFile();
        }

        /// <summary>
        /// Checks if the macro can start.
        /// </summary>
        /// <returns><see langword="true"/> if the macro process doesn't exist and at least one bind is enabled.</returns>
        public bool CanStart() => _macroProcess == null && HasKeyBound() && !_incorrectMacroVersion;

        /// <summary>
        /// Checks if any macro hotkeys are bound.
        /// </summary>
        /// <returns><see langword="true"/> if at least one bind is enabled.</returns>
        public bool HasKeyBound() => _downScrollKey != Keys.None || _upScrollKey != Keys.None;

        /// <summary>
        /// Starts the macro process.
        /// </summary>
        public void Start() {
            if(!CanStart()) return;
            TerminateUnmanagedMacros(); // One final check before running our own macro instance
            _macroProcess = Process.Start(MACRO_START_INFO);
            IsRunning = true;
            Log.Verbose("Freescroll macro started.");
            if(_timer.Enabled) _timer.Stop();
            try {
                if(!CheckModuleSize()) {
                    Stop(true);
                    _incorrectMacroVersion = true;
                    Log.Warning("Macro version mismatch. Please redownload DESRU.");
                    System.Media.SystemSounds.Asterisk.Play();
                    MessageBox.Show("The version of the macro currently installed does not match what is expected.\n" +
                        "Please redownload and reinstall DESRU to make sure your files are up to date.", "Macro Executable Mismatch");
                }
            }catch(NullReferenceException e) {
                Log.Error(e, "Macro process was somehow null when checking module size.");
            }catch(ArgumentNullException n) {
                Log.Error(n, "Macro process is null. Aborting module size check.");
            }finally {
                if(_macroProcess == null) {
                    Restart();
                }else {
                    if(_macroProcess.HasExited) Restart();
                }
            }
        }

        /// <summary>
        /// Stops the macro process and starts a timer periodically terminating any other macro processes.
        /// </summary>
        /// <param name="startTimer"></param>
        public void Stop(bool startTimer) {
            if(_macroProcess == null) return;
            _macroProcess.Kill();
            _macroProcess = null;
            IsRunning = false;
            Log.Verbose("Freescroll macro stopped.");
            if(startTimer) _timer.Start();
        }

        /// <summary>
        /// Restarts the macro process.
        /// </summary>
        public void Restart() {
            Log.Verbose("Restarting Freescroll macro.");
            Stop(false);
            Start();
        }

        /// <summary>
        /// Retrieves the hotkey specified by the bool parameter.
        /// </summary>
        /// <param name="downKey"></param>
        /// <returns>Returns <see cref="_downScrollKey"/> if <see langword="true"/>, <see cref="_upScrollKey"/> if <see langword="false"/>.</returns>
        public Keys GetHotkey(bool downKey) => downKey ? _downScrollKey : _upScrollKey;

        /// <summary>
        /// Changes the desired hotkey then refreshes the bindings file and restarts the macro process.
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="downKey"></param>
        public void ChangeHotkey(Keys newKey, bool downKey) {
            if(downKey) _downScrollKey = newKey;
            else _upScrollKey = newKey;

            CreateBindingsFile();
            if(IsRunning) Restart(); // Macro is restarted for binding changes to take place
        }
        /// <summary>
        /// Checks if the installed version of the macro matches what is expected.
        /// </summary>
        /// <returns><see langword="true"/> if the main module size matches</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool CheckModuleSize() {
            if(_macroProcess == null) {
                throw new ArgumentNullException("Macro Process is currently null.");
            }else {
                if(_macroProcess.HasExited) throw new ArgumentNullException("Macro Process may have crashed.");
            }
            return _macroProcess.MainModule.ModuleMemorySize == 49152;
        }

        // Overwrites the bindings.txt file for the DOOMEternalMacro
        private void CreateBindingsFile() {
            string binds;

            if(_downScrollKey == Keys.None && _upScrollKey != Keys.None) binds = string.Format(UP_ONLY_FORMAT, (int) _upScrollKey);
            else if(_downScrollKey != Keys.None && _upScrollKey == Keys.None) binds = string.Format(DOWN_ONLY_FORMAT, (int) _downScrollKey);
            else binds = string.Format(DOWN_UP_FORMAT, (int) _downScrollKey, (int) _upScrollKey);

            File.WriteAllText(BINDINGS_FILE, binds);
        }

        // Timer method that periodically terminates any Macro processes.
        private void MacroCheck(object sender, EventArgs e) {
            if(_macroProcess == null) TerminateUnmanagedMacros(); // Prevents the user from running the macro outside the scope of this utility
        }
        /// <summary>
        /// Terminates any DOOMEternalMacro.exe processes that are running outside the scope of this utility.
        /// </summary>
        public static void TerminateUnmanagedMacros() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalMacro"));
            if(procList.Count == 0) return;
            Log.Information("Found extra macro processes. Terminating...");
            var c = 0;
            foreach(Process proc in procList) {
                proc.Kill();
                c++;
            }
            Log.Verbose("Terminated {Count} macro processes.", c);
        }
    }
}
