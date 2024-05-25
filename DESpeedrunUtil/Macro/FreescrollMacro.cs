using DESpeedrunUtil.Util;
using Serilog;
using System.Diagnostics;
using static DESpeedrunUtil.Define.Constants;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil.Macro {
    internal class FreescrollMacro {
        private const string MD5_CHECKSUM = "C0843017C93A17DA48F23C77B0DFF1DE";

        public static FreescrollMacro Instance { get; private set; }

        private readonly ProcessStartInfo MACRO_START_INFO;
        private Process _macroProcess;

        private Timer _timer;

        private Keys _downScrollKey { get; set; }
        private Keys _upScrollKey { get; set; }

        private bool _incorrectMacroVersion = false;
        private bool _processStarted = false;

        private int _recurseCount = 0;

        public FreescrollMacro(Keys downScroll, Keys upScroll) {
            MACRO_START_INFO = new ProcessStartInfo(@".\macro\DOOMEternalMacro.exe") {
                WorkingDirectory = @".\macro",
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            // Timer that runs every five seconds to prevent unmanaged macro processes
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Tick += new EventHandler(MacroCheck);
            _timer.Start();

            _downScrollKey = downScroll;
            _upScrollKey = upScroll;
            CreateBindingsFile();

            _macroProcess = new();

            Instance = this;

            Log.Information("Initialized FreescrollMacro");
        }

        /// <summary>
        /// Checks if the macro can start.
        /// </summary>
        /// <returns><see langword="true"/> if the macro process doesn't exist and at least one bind is enabled.</returns>
        public bool CanStart() => !IsRunning() && HasKeyBound() && !_incorrectMacroVersion;

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
            if(_timer.Enabled) _timer.Stop();
            TerminateUnmanagedMacros(); // One final check before running our own macro instance
            try {
                _macroProcess = new();
                _macroProcess.ErrorDataReceived += (s, e) => { LogMacroOutput(e.Data); };
                _macroProcess.StartInfo = MACRO_START_INFO;
                _processStarted = _macroProcess.Start();
                _macroProcess.BeginErrorReadLine();
            } catch(Exception e) {
                Log.Error(e, "Failed to start Freescroll Macro.");
                return;
            }
            Log.Information("Starting macro process...");
            var crash = false;
            try {
                if(_processStarted) {
                    Thread.Sleep(1000);
                    if(!CheckMacroVersion(out var output)) {
                        Stop(true);
                        _incorrectMacroVersion = true;
                        Log.Warning("Macro version mismatch. Please redownload DESRU.\n[Expected hash: {Expected}, got {Actual}]", MD5_CHECKSUM.ToLower(), output.ToLower());
                        System.Media.SystemSounds.Asterisk.Play();
                        MessageBox.Show("The version of the macro currently installed does not match what is expected.\n" +
                            "Please redownload and reinstall DESRU to make sure your files are up to date.", "Macro Executable Mismatch");
                    }
                } else {
                    crash = true;
                }
            } catch(Exception e) {
                Log.Error(e, "Something went wrong when checking the macro md5 file hash.");
                crash = true;
            } finally {
                _recurseCount++;
                if(crash) {
                    Restart();
                } else {
                    Log.Information("Freescroll Macro is running.");
                }
                _recurseCount = 0;
            }
        }

        /// <summary>
        /// Stops the macro process and starts a timer periodically terminating any other macro processes.
        /// </summary>
        /// <param name="startTimer"></param>
        public void Stop(bool startTimer) {
            if(!IsRunning()) return;
            _macroProcess.Kill();
            _processStarted = false;
            Log.Information("Freescroll macro stopped.");
            if(startTimer) _timer.Start();
        }

        /// <summary>
        /// Restarts the macro process.
        /// </summary>
        public void Restart() {
            if(_recurseCount > 5) {
                Log.Error("Failed to restart macro.");
                Stop(true);
                if(!_timer.Enabled) _timer.Start();
                _recurseCount = 0;
                return;
            }
            if(_recurseCount > 0)
                Log.Information("Attempting to restart Freescroll macro. ({Count})", _recurseCount);
            else
                Log.Information("Restarting Freescroll macro.");
            Stop(false);
            Start();
        }

        public bool IsRunning() => _processStarted && !_macroProcess.HasExited;

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
            if(IsRunning()) Restart(); // Macro is restarted for binding changes to take place
        }
        /// <summary>
        /// Checks if the installed version of the macro matches what is expected.
        /// </summary>
        /// <returns><see langword="true"/> if the file hash matches</returns>
        public bool CheckMacroVersion(out string md5) {
                md5 = Checksums.GetMD5ChecksumFromFile(_macroProcess.MainModule.FileName);
            return Checksums.Compare(md5, MD5_CHECKSUM);
        }

        // Overwrites the bindings.txt file for the DOOMEternalMacro
        private void CreateBindingsFile() {
            string binds;

            if(_downScrollKey == Keys.None && _upScrollKey != Keys.None) binds = string.Format(UP_ONLY_FORMAT, (int) _upScrollKey);
            else if(_downScrollKey != Keys.None && _upScrollKey == Keys.None) binds = string.Format(DOWN_ONLY_FORMAT, (int) _downScrollKey);
            else binds = string.Format(DOWN_UP_FORMAT, (int) _downScrollKey, (int) _upScrollKey);

            File.WriteAllText(BINDINGS_FILE, binds);
            Log.Information("Updated Macro bindings.txt file with binds: {Binds}", binds);
        }

        // Timer method that periodically terminates any Macro processes.
        private void MacroCheck(object? sender, EventArgs e) {
            if(!IsRunning()) TerminateUnmanagedMacros(); // Prevents the user from running the macro outside the scope of this utility
        }
        /// <summary>
        /// Terminates any DOOMEternalMacro.exe processes that are running outside the scope of this utility.
        /// </summary>
        public static void TerminateUnmanagedMacros() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DOOMEternalMacro"));
            if(procList.Count == 0) return;
            Log.Information("Found {Count} macro processes. Terminating...", procList.Count);
            var c = 0;
            foreach(Process proc in procList) {
                proc.Kill();
                c++;
            }
            Log.Verbose("Terminated {Count} macro processes.", c);
        }

        private static void LogMacroOutput(string? data) {
            if(string.IsNullOrEmpty(data)) return;
            var output = "[Freescroll Macro] " + data;

            Log.Error(output);
        }
    }
}
