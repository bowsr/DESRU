using DESpeedrunUtil.Hotkeys;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Timer = System.Windows.Forms.Timer;
using static DESpeedrunUtil.Define.Structs;
using static DESpeedrunUtil.Interop.DLLImports;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {

        private const int OFFSET_ROW2 = 0x8;
        private const int OFFSET_ROW3 = 0x58;
        private const int OFFSET_ROW4 = 0x70;
        private const int OFFSET_ROW5 = 0x78;
        private const int OFFSET_ROW6 = 0x98;
        private const int OFFSET_ROW7 = 0x10;
        private const int OFFSET_ROW8 = 0x30;
        private const int OFFSET_ROW9 = 0x40;

        private readonly float[] ONEPERCENT_RES_SCALES = new float[32] 
                { 1.0f, 0.968f, 0.936f, 0.904f, 0.872f, 0.84f, 0.808f, 0.776f, 
                0.744f, 0.712f, 0.68f, 0.648f, 0.616f, 0.584f, 0.552f, 0.52f, 
                0.488f, 0.456f, 0.424f, 0.392f, 0.36f, 0.328f, 0.296f, 0.264f, 
                0.232f, 0.2f, 0.168f, 0.136f, 0.104f, 0.072f, 0.04f, 0.01f };

        public static List<KnownOffsets> OffsetList = new(), ScannedOffsetList = new();
        KnownOffsets _currentOffsets;

        DeepPointer? _maxHzDP, _metricsDP, _rampJumpDP, _minResDP, _dynamicResDP, _resScalesDP,
                    _row1DP, _row6DP,
                    _gpuVendorDP, _gpuNameDP, _cpuDP,
                    _raiseMSDP, _dropMSDP;

        IntPtr _maxHzPtr, _metricsPtr, _rampJumpPtr, _minResPtr, _dynamicResPtr, _resScalesPtr,
               _row1Ptr, _row6Ptr,
               _gpuVendorPtr, _gpuNamePtr, _cpuPtr,
               _raiseMSPtr, _dropMSPtr;

        Process _game, _trainer;
        HotkeyHandler _hotkeys;
        public Timer MemoryTimer { get; init; }
        public bool Reset { get; init; }
        System.Timers.Timer _restartCheatsTimer;
        int _moduleSize;
        public string Version { get; init; }

        bool _osdFlagCheats = false, _osdFlagMacro = false, _osdFlagFirewall = false, _osdFlagSlopeboost = false, _osdFlagReshade = false,
             _unlockResFlag = false, _autoDynamic = false, _resUnlocked = false, _osdFlagOutOfDate = false, _osdFlagRestartGame = false,
             _trainerFlag = false, _scheduleDynamic = false, _osdFlagLimiter = true;
        string _row1, _row2, _row3, _row4, _row5, _row6, _row7, _row8, _row9, _cpu, _gpuV, _gpuN;
        string _cheatString = "CHEATS ENABLED", _scrollString = "";
        int _fpsLimit = 250, _targetFPS = 1000;
        float _minRes = 0.01f;

        bool _windowFocused = false, _dynTimer = false;
        long _focusedTime, _dynTime;

        public MemoryHandler(Process game, HotkeyHandler hotkeys) {
            _game = game ?? throw new ArgumentNullException(nameof(game), "Game process is null. How?");
            _trainer = null;
            _hotkeys = hotkeys;
            try {
                _moduleSize = _game.MainModule.ModuleMemorySize;
            }catch(Exception e) {
                Log.Error(e, "An error occured when retrieving the game's moduleSize");
                Reset = true;
                return;
            }
            Version = TranslateModuleSize();
            _row1 = _row2 = _row3 = _row4 = _row5 = _row6 = _row7 = _row8 = _row9 = _cpu = _gpuV = _gpuN = "";

            MemoryTimer = new Timer();
            MemoryTimer.Interval = Program.TimerInterval;
            MemoryTimer.Tick += (sender, e) => { MemoryTick(); };

            _restartCheatsTimer = new System.Timers.Timer(2500);
            _restartCheatsTimer.Elapsed += (sender, e) => { RestartTick(); };
            _restartCheatsTimer.Start();

            Reset = false;
            Initialize();
            Log.Information("Initialized MemoryHandler. Game Version: {Version} [{ModuleSize}]", Version, _moduleSize);
        }


        private void MemoryTick() {
            if(!_restartCheatsTimer.Enabled) _restartCheatsTimer.Start();

            DerefPointers();

            if(!_trainerFlag) {
                if(_osdFlagRestartGame) _cheatString = (_osdFlagCheats) ? "CHEATS ENABLED" : "RESTART GAME";
                if(Version == "1.0 (Release)") SetFlag(!_game.ReadValue<bool>(_rampJumpPtr), "slopeboost");
                _row1 = "%i FPS" + ((_osdFlagOutOfDate) ? "*" : "");
                _row2 = _currentOffsets.Version.Replace(" Rev ", "r").Replace(" (Gamepass)", "");
                if(_row2 == "1.0 (Release)") _row2 = "Release";
                if(_metricsPtr == IntPtr.Zero) _row2 = string.Empty;
                if(_osdFlagMacro || _osdFlagFirewall || _osdFlagSlopeboost || _osdFlagReshade || !_osdFlagLimiter) {
                    if(_row2 != string.Empty) {
                        _row2 += " (";
                    }else {
                        _row1 += " ";
                        if(!_osdFlagOutOfDate) {
                            _row1 += "(";
                        }else {
                            _row2 += "(";
                        }
                    }
                    _row2 += (_osdFlagMacro ? "M" : "") + (_osdFlagFirewall ? "F" : "") + (_osdFlagReshade ? "R" : "") + (_osdFlagSlopeboost ? "S" : "") + (!_osdFlagLimiter ? "L" : "") + ")";
                }
                var cheats = (_osdFlagCheats || _osdFlagRestartGame) ? _cheatString : "";
                if(_currentOffsets.Version.Contains("Unknown") || _metricsPtr == IntPtr.Zero) {
                    if(cheats != string.Empty) {
                        _row1 = _row1.Substring(0, 7) + (_row1.Contains('*') ? " " : cheats[0]);
                        _row2 = _row1.Contains('*') ? cheats : cheats[1..];
                    }
                    if(_cpuPtr == IntPtr.Zero) {
                        _row3 = _scrollString;
                        _cpu = string.Empty;
                    }else {
                        _cpu = _scrollString;
                        _row3 = string.Empty;
                    }
                }else {
                    if(_cpuPtr == IntPtr.Zero) {
                        _row3 = (_scrollString != string.Empty) ? _scrollString : cheats;
                        _cpu = "";
                    }else {
                        _cpu = (_scrollString != string.Empty) ? _scrollString : cheats;
                        _row3 = "";
                    }
                    SetMetrics(2);
                }
                ModifyMetricRows();
            }

            if(_minResPtr != IntPtr.Zero && ReadyToUnlockRes()) _game.WriteBytes(_minResPtr, FloatToBytes(_minRes));
            if(CanCapFPS() && _fpsLimit != ReadMaxHz()) _game.WriteBytes(_maxHzPtr, BitConverter.GetBytes((short) _fpsLimit));
            if(_unlockResFlag) {
                if(ReadyToUnlockRes()) {
                    if(!_windowFocused && CheckIfGameIsFocused()) {
                        _focusedTime = DateTime.Now.Ticks;
                        _windowFocused = true;
                    }
                    if(_windowFocused) {
                        if(!CheckIfGameIsFocused()) {
                            _windowFocused = false;
                        }else {
                            if(((DateTime.Now.Ticks - _focusedTime) / 10000) >= 2500) {
                                UnlockResScale(_targetFPS);
                                SendKeys.Send("%(~)");
                                SendKeys.Send("%(~)");
                                _unlockResFlag = false;
                                _windowFocused = false;
                                _resUnlocked = true;
                                _hotkeys.ToggleResScaleKey(true);
                            }
                        }
                    }
                }
            }
            if(_scheduleDynamic && ReadyToUnlockRes()) {
                if(!_dynTimer) {
                    _dynTime = DateTime.Now.Ticks;
                    _dynTimer = true;
                }
                if(_dynTimer && ((DateTime.Now.Ticks - _dynTime) / 10000) >= 2500) {
                    EnableDynamicScaling(_targetFPS);
                    _scheduleDynamic = false;
                    _dynTimer = false;
                }
            }
        }
        private void RestartTick() {
            // Checks if the trainer is running
            // If the trainer is detected, metric rows are no longer modified, and the restart game flag is set to true
            // This is done here since this process takes upwards of 3ms and this timer has a longer interval.
            // This will eventually be removed once the trainer is integrated directly into DESRU
            List<Process> processes = Process.GetProcesses().ToList();
            if(_trainer == null) {
                var trainers = processes.FindAll(x => x.ProcessName.Contains("DoomEternalTrainer"));
                if(trainers.Count > 0) {
                    _trainer = trainers[0];
                    if(!_osdFlagCheats) SetFlag(true, "restart");
                    _trainerFlag = true;
                    Log.Information("Trainer process found running.");
                }else {
                    _trainer = null;
                    _trainerFlag = false;
                }
            }else {
                if(_trainer.HasExited) {
                    _trainer = null;
                    _trainerFlag = false;
                }
            }
            if(!_osdFlagRestartGame) {
                if(!_osdFlagCheats && processes.FindAll(x => x.ProcessName.ToLower().Contains("cheatengine")).Count > 0) {
                    SetFlag(true, "restart");
                    _cheatString = "RESTART GAME";
                    Log.Warning("CheatEngine process found running.");
                }
            }
        }

        private void ModifyMetricRows() {
            if(_row1Ptr != IntPtr.Zero) {
                var r6 = (_row6Ptr != IntPtr.Zero) ? _row6Ptr : _row1Ptr + OFFSET_ROW6;
                _game.VirtualProtect(_row1Ptr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_row1Ptr, ToByteArray(_row1, 20));
                _game.WriteBytes(_row1Ptr + OFFSET_ROW2, ToByteArray(_row2, 18));
                _game.WriteBytes(_row1Ptr + OFFSET_ROW3, ToByteArray(_row3, 19));
                _game.WriteBytes(_row1Ptr + OFFSET_ROW4, ToByteArray(_row4, 7));
                _game.WriteBytes(_row1Ptr + OFFSET_ROW5, ToByteArray(_row5, 34));
                _game.WriteBytes(r6, ToByteArray(_row6, 34));
                _game.WriteBytes(r6 + OFFSET_ROW7, ToByteArray(_row7, 34));
            }
            if(_row6Ptr != IntPtr.Zero) _game.WriteBytes(_row6Ptr + OFFSET_ROW8, ToByteArray(_row8, 34));
            if(_row6Ptr != IntPtr.Zero) _game.WriteBytes(_row6Ptr + OFFSET_ROW9, ToByteArray(_row9, 34));
            if(_cpuPtr != IntPtr.Zero) {
                _game.VirtualProtect(_cpuPtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_cpuPtr, ToByteArray(_cpu, 64));
            }
            if(_gpuVendorPtr != IntPtr.Zero) {
                _game.VirtualProtect(_gpuVendorPtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_gpuVendorPtr, ToByteArray(_gpuV, 64));
            }
            if(_gpuNamePtr != IntPtr.Zero) {
                _game.VirtualProtect(_gpuNamePtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_gpuNamePtr, ToByteArray(_gpuN, 64));
            }
        }

        private bool CheckIfGameIsFocused() {
            try {
                return _game.MainWindowHandle == GetForegroundWindow();
            }catch(Exception e) {
                Log.Error(e, "An error occured when checking if the game was in focus.");
                return false;
            }
        }
        // Once the main menu is pretty much loaded, rs_raiseMilliseconds gets set to 15.833332ms
        //  or lower, depending on the user's settings.
        // Used to make sure ALT+ENTER isn't dropped due to the game loading.
        private bool ReadyToUnlockRes() {
            var ms = ReadRaiseMillis();
            return ms > 0f && ms < 16f;
        }

        /// <summary>
        /// Sets the scroll pattern string to display on screen
        /// </summary>
        /// <param name="scrollString"><see cref="string"/> displayed on screen</param>
        public void SetScrollPatternString(string scrollString) => _scrollString = scrollString;

        /// <summary>
        /// Stops timers and sets 3rd metrics row to a <see cref="string"/> indicating DESRU was closed
        /// </summary>
        public void ClosingDESRU() {
            _restartCheatsTimer.Stop();
            MemoryTimer.Stop();
            _row3 = "DESRU CLOSED";
            if(_currentOffsets.Version.Contains("Unknown")) {
                _game.WriteBytes(_row1Ptr + 0x7 + (_row1.Contains('*') ? 0x1 : 0x0), ToByteArray(_row3, 18));
                return;
            }
            if(_cpuPtr == IntPtr.Zero)
                _game.WriteBytes(_row1Ptr + OFFSET_ROW3, ToByteArray(_row3, 19));
            else
                _game.WriteBytes(_cpuPtr, ToByteArray(_row3, 64));
        }

        private void SetMetrics(byte val) {
            if(val > 6) return;
            _game.WriteBytes(_metricsPtr, new byte[] { val });
        }

        private void UnlockResScale(int targetFPS) {
            SetResScales();
            if(_minResPtr != IntPtr.Zero) _game.WriteBytes(_minResPtr, FloatToBytes(_minRes));
            if(_autoDynamic) {
                EnableDynamicScaling(targetFPS);
                _autoDynamic = false;
            }
        }
        /// <summary>
        /// Schedules dynamic scaling to be turned on when the game is loaded
        /// </summary>
        /// <param name="targetFPS">FPS Target the scaling will try to hit</param>
        public void ScheduleDynamicScaling(int targetFPS) {
            if(Version.Contains("Unknown")) return;
            _targetFPS = targetFPS;
            _scheduleDynamic = true;
        }
        /// <summary>
        /// Enables dynamic resolution scaling with a specified target FPS
        /// </summary>
        /// <param name="targetFPS">FPS Target the scaling will try to hit</param>
        public void EnableDynamicScaling(int targetFPS) {
            if(_dynamicResPtr != IntPtr.Zero) _game.WriteBytes(_dynamicResPtr, new byte[] { 1 });
            if(_raiseMSPtr != IntPtr.Zero) _game.WriteBytes(_raiseMSPtr, FloatToBytes((1000f / targetFPS) * 0.95f));
            if(_dropMSPtr != IntPtr.Zero) _game.WriteBytes(_dropMSPtr, FloatToBytes((1000f / targetFPS) * 0.99f));
        }
        /// <summary>
        /// Toggles Dynamic Resolution Scaling
        /// </summary>
        public void ToggleDynamicScaling() {
            if(_dynamicResPtr == IntPtr.Zero) return;
            if(ReadDynamicRes()) _game.WriteBytes(_dynamicResPtr, new byte[] { 0 });
            else _game.WriteBytes(_dynamicResPtr, new byte[] { 1 });
        }
        private static byte[] FloatToBytes(float f) {
            byte[] output = new byte[4];
            float[] fArray = new float[1] { f };
            Buffer.BlockCopy(fArray, 0, output, 0, 4);
            return output;
        }

        private void SetResScales() {
            if(Version != "6.66 Rev 2" && _minRes >= 0.5f) return; // No need to change res scales since 50% is the default minimum in game
            float[] scales = ONEPERCENT_RES_SCALES;
            if(Version == "6.66 Rev 2") {
                // rs_minimumResolutionScale does not exist on 6.66 Rev 2. It is dynamically inferred based off the 128 byte set of res scale values.
                // Because of this, we cannot set the min res scale directly, so a new set of res scale values must be generated,
                //   based off of the desired minimum resolution set by the user.
                float min = _minRes;
                scales = new float[32];
                for(int i = 0; i < scales.Length; i++) {
                    scales[i] = (1.0f - (((1.0f - min) / 31) * i));
                }
            }
            byte[] resBytes = new byte[32 * 4];
            Buffer.BlockCopy(scales, 0, resBytes, 0, resBytes.Length);
            if(_resScalesPtr != IntPtr.Zero) {
                _game.VirtualProtect(_resScalesPtr, 32 * 4, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_resScalesPtr, resBytes);
            }
        }

        /// <summary>
        /// Sets the state of the flags that will be shown next to the FPS counter.
        /// </summary>
        /// <param name="flag">State of the flag</param>
        /// <param name="flagName">Name of the flag being set</param>
        public void SetFlag(bool flag, string flagName) {
            switch(flagName) {
                case "cheats":
                    _osdFlagCheats = flag;
                    break;
                case "firewall":
                    _osdFlagFirewall = flag;
                    break;
                case "limiter":
                    _osdFlagLimiter = flag;
                    break;
                case "macro":
                    _osdFlagMacro = flag;
                    break;
                case "outofdate":
                    _osdFlagOutOfDate = flag;
                    break;
                case "reshade":
                    _osdFlagReshade = flag;
                    break;
                case "restart":
                    _osdFlagRestartGame = flag;
                    break;
                case "slopeboost":
                    _osdFlagSlopeboost = flag;
                    break;
            }
        }

        /// <summary>
        /// Retrieves the state of a specified flag
        /// </summary>
        /// <param name="flagName"><see langword="string"/> representing which flag to retrieve</param>
        /// <returns>State of the specified <see langword="bool"/> flag</returns>
        public bool GetFlag(string flagName) {
            return flagName switch {
                "cheats" => _osdFlagCheats,
                "firewall" => _osdFlagFirewall,
                "limiter" => _osdFlagLimiter,
                "macro" => _osdFlagMacro,
                "outofdate" => _osdFlagOutOfDate,
                "reshade" => _osdFlagReshade,
                "restart" => _osdFlagRestartGame,
                "slopeboost" => _osdFlagSlopeboost,
                "resunlocked" => _resUnlocked,
                "unlockscheduled" => _unlockResFlag,
                _ => false
            };
        }
        /// <summary>
        /// Schedules the resolution scaling unlock for the next time the game is in focus
        /// </summary>
        /// <param name="auto">Enable dynamic scaling at the same time</param>
        /// <param name="targetFPS">Target FPS for dynamic scaling</param>
        public void ScheduleResUnlock(bool auto, int targetFPS) {
            if(Version.Contains("Unknown")) return;
            if(_resUnlocked && Version != "6.66 Rev 2") {
                if(_minResPtr != IntPtr.Zero) _game.WriteBytes(_minResPtr, FloatToBytes(_minRes));
                return;
            }
            _unlockResFlag = true;
            _autoDynamic = auto;
            _targetFPS = targetFPS;
        }

        /// <summary>
        /// Checks if the current game version can cap FPS
        /// </summary>
        /// <returns><see langword="true"/> if the MaxHzPtr exists</returns>
        public bool CanCapFPS() => _maxHzPtr != IntPtr.Zero;

        /// <summary>
        /// Sets the stored fps limit and also modifies the value in DOOMEternal's memory for an instant response
        /// </summary>
        /// <param name="fps">FPS Limit</param>
        public void SetMaxHz(int fps) {
            _fpsLimit = fps;
            if(CanCapFPS() && _fpsLimit != ReadMaxHz()) _game.WriteBytes(_maxHzPtr, BitConverter.GetBytes((short) _fpsLimit));
        }
        public void SetMinRes(float min) => _minRes = min;
        public float GetMinRes() => _minRes;
        /// <summary>
        /// Reads the current value of rs_enable from memory
        /// </summary>
        /// <returns><see langword="true"/> if rs_enable is set to 1. <see langword="false"/> otherwise</returns>
        public bool ReadDynamicRes() {
            if(_dynamicResPtr != IntPtr.Zero) return _game.ReadValue<bool>(_dynamicResPtr);
            return false;
        }
        /// <summary>
        /// Reads the current value of com_adaptiveTickMaxHz from memory
        /// </summary>
        /// <returns>An <see cref="int"/> representing max Hz. <c>-1</c> if it cannot be read from memory</returns>
        public int ReadMaxHz() {
            int cap = -1;
            if(CanCapFPS()) _game.ReadValue(_maxHzPtr, out cap);
            return cap;
        }
        /// <summary>
        /// Reads the curreent value of rs_minimumResolution from memory
        /// </summary>
        /// <returns>A <see cref="float"/> representing minimum resolution scale. <c>-1f</c> if it cannot be read from memory</returns>
        public float ReadMinRes() {
            if(_minResPtr != IntPtr.Zero) return _game.ReadValue<float>(_minResPtr);
            return -1f;
        }
        /// <summary>
        /// Reads the current value of rs_raiseMilliseconds from memory
        /// </summary>
        /// <returns>A <see cref="float"/> representing minimum frametime for scaling. <c>-1f</c> if it cannot be read from memory</returns>
        public float ReadRaiseMillis() {
            if(_raiseMSPtr != IntPtr.Zero) return _game.ReadValue<float>(_raiseMSPtr);
            return -1f;
        }

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            try {
                if(_row1DP != null) {
                    _row1DP.DerefOffsets(_game, out _row1Ptr);
                    if(_row6DP != null) _row6DP.DerefOffsets(_game, out _row6Ptr);
                }
                if(_gpuVendorDP != null) _gpuVendorDP.DerefOffsets(_game, out _gpuVendorPtr);
                if(_gpuNameDP != null) _gpuNameDP.DerefOffsets(_game, out _gpuNamePtr);
                if(_metricsDP != null) _metricsDP.DerefOffsets(_game, out _metricsPtr);
                if(_maxHzDP != null) _maxHzDP.DerefOffsets(_game, out _maxHzPtr);
                if(_cpuDP != null) _cpuDP.DerefOffsets(_game, out _cpuPtr);
                if(_rampJumpDP != null) _rampJumpDP.DerefOffsets(_game, out _rampJumpPtr);
                if(_minResDP != null) _minResDP.DerefOffsets(_game, out _minResPtr);
                if(_dynamicResDP != null) _dynamicResDP.DerefOffsets(_game, out _dynamicResPtr);
                if(_resScalesDP != null) _resScalesDP.DerefOffsets(_game, out _resScalesPtr);
                if(_raiseMSDP != null) _raiseMSDP.DerefOffsets(_game, out _raiseMSPtr);
                if(_dropMSDP != null) _dropMSDP.DerefOffsets(_game, out _dropMSPtr);
            }catch(Win32Exception e) {
                Debug.WriteLine(e.StackTrace);
                return;
            }
        }

        /// <summary>
        /// Converts a <see cref="string"/> of text into a byte array of a specified length
        /// </summary>
        /// <param name="text">Text to convert</param>
        /// <param name="length">Length of byte array</param>
        /// <returns>A byte array representing a string</returns>
        public static byte[] ToByteArray(string text, int length) {
            byte[] output = new byte[length];
            byte[] textArray = Encoding.ASCII.GetBytes(text);
            for(int i = 0; i < length; i++) {
                if(i >= textArray.Length)
                    break;
                output[i] = textArray[i];
            }
            return output;
        }

        /// <summary>
        /// Translates the game module size into a human readable version string.
        /// </summary>
        /// <returns>Returns a <see langword="string"/> representing the game's version.</returns>
        public string TranslateModuleSize() {
            return _moduleSize switch {
                507191296 or 515133440 or 510681088 => "1.0 (Release)",
                482037760 => "May Patch Steam",
                546783232 => "May Hotfix Steam",
                492113920 => "1.1",
                490299392 => "2.0",
                505344000 => "2.1",
                475557888 => "3.0",
                504107008 => "3.1",
                478056448 => "4.0",
                472821760 => "4.1",
                475787264 => "5.0",
                459132928 => "5.1",
                481435648 => "6.0",
                465915904 => "6.1",
                464543744 => "6.2",
                483786752 => "6.3",
                494395392 => "6.4",
                508350464 => "6.66",
                478367744 => "6.66 Rev 1",
                475570176 => "6.66 Rev 1.1",
                510251008 => "6.66 Rev 2",
                445820928 => "6.66 Rev 2 (Gamepass)",
                _ => "Unknown (" + _moduleSize.ToString() + ")",
            };
        }

        private void Initialize() {
            _row1DP = /*_row2DP = _row3DP = _row4DP = _row5DP =*/ _row6DP = /*_row7DP = _row8DP = _row9DP =*/ null;
            _gpuVendorDP = _gpuNameDP = _metricsDP = _maxHzDP = _cpuDP = _rampJumpDP = _resScalesDP = _minResDP = _dynamicResDP = _raiseMSDP = _dropMSDP = null;
            if(!SetCurrentKnownOffsets()) {
                SigScans();
            }
            if(_currentOffsets.Row1 != 0) _row1DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row1);
            if(_currentOffsets.Row6 != 0) _row6DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row6);

            if(_currentOffsets.ResScales != 0) _resScalesDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.ResScales);

            if(_currentOffsets.GPUVendor != 0) _gpuVendorDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.GPUVendor);
            if(_currentOffsets.GPUName != 0) _gpuNameDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.GPUName);
            if(_currentOffsets.CPU != 0) _cpuDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.CPU, 0x0);

            if(_currentOffsets.Metrics != 0) _metricsDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Metrics);
            if(_currentOffsets.MaxHz != 0) _maxHzDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.MaxHz);
            if(_currentOffsets.MinRes != 0) _minResDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.MinRes);
            if(_currentOffsets.DynamicRes != 0) _dynamicResDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.DynamicRes);
            if(_currentOffsets.RaiseMS != 0) _raiseMSDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.RaiseMS);
            if(_currentOffsets.DropMS != 0) _dropMSDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.DropMS);
            if(Version == "1.0 (Release)") _rampJumpDP = new DeepPointer("DOOMEternalx64vk.exe", 0x6126430);
        }

        private void SigScans() {
            const string SIGSCAN_FPS = "2569204650530000252E32666D7300004672616D65203A202575";
            // DLSS does not show up if you don't have a capable gpu (NVIDIA RTX) BUT it still exists in memory in the exact same spot.
            const string SIGSCAN_DLSS = "444C5353203A2025730000000000000056756C6B616E202573";
            const string SIGSCAN_RES_SCALES = 
            "0000803FA4707D3F48E17A3FEC51783F8FC2753F3333733FD7A3703F7B146E3F" +
            "1F856B3FC3F5683F6666663F0AD7633FAE47613FF6285C3F3D0A573F85EB513F" +
            "CDCC4C3F14AE473F5C8F423FA4703D3FEC51383F3333333F7B142E3FC3F5283F" +
            "0AD7233F52B81E3F9A99193FE17A143F295C0F3F713D0A3FB81E053F0000003F";

            // This only needs to be done on the first hook of the game. Offsets can be saved since they're not pointer chains.
            // Despite knowing the offsets, they will still need to be placed in a DeepPointer to prevent massive memory usage and a possible leak
            Log.Information("Signature Scans initiated. moduleSize: {ModuleSize}", _moduleSize);
            IntPtr r1, r6, res;
            r1 = r6 = res = IntPtr.Zero;
            SigScanTarget fpsTarget = new(SIGSCAN_FPS);
            SigScanTarget dlssTarget = new(SIGSCAN_DLSS);
            SigScanTarget resTarget = new(SIGSCAN_RES_SCALES);
            SignatureScanner scanner = new(_game, _game.MainModule.BaseAddress, _game.MainModule.ModuleMemorySize);
            Log.Information("Scanning for FPS counter.");
            r1 = scanner.Scan(fpsTarget);
            if(r1 != IntPtr.Zero) {
                Log.Information("Found FPS counter.");
                r6 = IntPtr.Zero;
                Log.Information("Scanning for DLSS string.");
                IntPtr dlss = scanner.Scan(dlssTarget);
                if(dlss.ToInt64() != 0) {
                    Log.Information("Found DLSS string.");
                    r6 = dlss;
                }
            }else {
                Log.Error("Could not find the perf metrics rows in memory. Is this even DOOM Eternal?");
                return;
            }
            Log.Information("Scanning for resolution scale values.");
            res = scanner.Scan(resTarget);
            if(res != IntPtr.Zero) Log.Information("Found resolution scale values.");
            KnownOffsets ko = new(Version, GetOffset(r1), GetOffset(r6), GetOffset(res));
            ScannedOffsetList.Add(ko);
            _currentOffsets = ko;

            string jsonString = JsonConvert.SerializeObject(ScannedOffsetList, Formatting.Indented);
            File.WriteAllText(@".\scannedOffsets.json", jsonString);
            Log.Information("Added Unknown Version ({ModuleSize}) to known offset list.", _moduleSize);
        }
        private int GetOffset(IntPtr pointer) => (pointer != IntPtr.Zero) ? (int) (pointer.ToInt64() - _game.MainModule.BaseAddress.ToInt64()) : 0;
        public static bool IsValidVersionString(string version) {
            return version switch {
                "1.0 (Release)" => true,
                "May Patch Steam" => true,
                "May Hotfix Steam" => true,
                "1.1" => true,
                "2.0" => true,
                "2.1" => true,
                "3.0" => true,
                "3.1" => true,
                "4.0" => true,
                "4.1" => true,
                "5.0" => true,
                "5.1" => true,
                "6.0" => true,
                "6.1" => true,
                "6.2" => true,
                "6.3" => true,
                "6.4" => true,
                "6.66" => true,
                "6.66 Rev 1" => true,
                "6.66 Rev 1.1" => true,
                "6.66 Rev 2" => true,
                "6.66 Rev 2 (Gamepass)" => true,
                _ => false,
            };
        }

        private bool SetCurrentKnownOffsets() {
            foreach(KnownOffsets k in OffsetList) {
                if(k.Version == Version) {
                    _currentOffsets = k;
                    return true;
                }
            }
            foreach(KnownOffsets k in ScannedOffsetList) {
                if(k.Version == Version) {
                    _currentOffsets = k;
                    Log.Information("Version {Version} is not officially supported. Using previously scanned offsets.", Version);
                    return true;
                }
            }
            Log.Warning("Offset Lists do not contain {Version}.", Version);
            return false;
        }
    }
}
