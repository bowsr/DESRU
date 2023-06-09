using DESpeedrunUtil.Hotkeys;
using DESpeedrunUtil.Util;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using static DESpeedrunUtil.Define.Constants;
using static DESpeedrunUtil.Define.Structs;
using static DESpeedrunUtil.Interop.DLLImports;
using Timer = System.Windows.Forms.Timer;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {

        public static List<KnownOffsets> OffsetList = new(), ScannedOffsetList = new();
        KnownOffsets _currentOffsets;

        public static List<CheatOffsets> CheatOffsetList = new();
        CheatOffsets _currentCheatOffsets;

        DeepPointer? _maxHzDP, _metricsDP, _rampJumpDP, _minResDP, _skipIntroDP, _aliasingDP, _unDelayDP, _continueDP,
                    _row1DP, _row6DP,
                    _gpuVendorDP, _gpuNameDP, _cpuDP,
                    _dynamicResDP, _resScalesDP, _raiseMSDP, _dropMSDP, _forceResDP, _currentResScaleDP,
                    _velocityDP, _positionDP, _rotationDP,
                    _isLoadingDP, _isLoading2DP, _isInGameDP,
                    _cheatsConsoleDP, _cheatsBindsDP;

        IntPtr _maxHzPtr, _metricsPtr, _rampJumpPtr, _minResPtr, _skipIntroPtr, _aliasingPtr, _unDelayPtr, _continuePtr,
               _row1Ptr, _row6Ptr,
               _gpuVendorPtr, _gpuNamePtr, _cpuPtr,
               _dynamicResPtr, _resScalesPtr, _raiseMSPtr, _dropMSPtr, _forceResPtr, _currentResScalePtr,
               _velocityPtr, _positionPtr, _rotationPtr,
               _isLoadingPtr, _isLoading2Ptr, _isInGamePtr,
               _cheatsConsolePtr, _cheatsBindsPtr;

        Process _game, _trainer;
        public Timer MemoryTimer { get; init; }
        public bool Reset { get; init; }
        public bool TrainerSupported { get; private set; } = true;
        public bool FirstRun { get; set; } = true;
        public bool EnableOSD { get; set; } = false;
        public bool EnableCheats { get; set; } = false;
        System.Timers.Timer _restartCheatsTimer;
        int _moduleSize;
        public GameVersion Version { get; init; }
        public float CurrentResScaling { get; private set; } = 0f;
        public bool UseDynamicScaling { get; set; } = true;

        bool _osdFlagMeath00k = false, _osdFlagMacro = false, _osdFlagFirewall = false, _osdFlagSlopeboost = false, _osdFlagReshade = false,
             _osdFlagOutOfDate = false, _osdFlagRestartGame = false, _osdFlagLimiter = true, _osdFlagModded = false,
             _unlockResFlag = false, _autoScaling = false, _resUnlocked = false,
             _trainerFlag = false, _externalTrainerFlag = false, _scheduleScaling = false,
             _antiAliasing = true, _unDelay = true, _autoContinue = false,
             _minimalOSD = false;
        string _row1, _row2, _row3, _row4, _row5, _row6, _row7, _row8, _row9, _cpu, _gpuV, _gpuN;
        string _cheatString = "CHEATS ENABLED", _scrollString = "";
        int _fpsLimit = 250, _targetFPS = 1000;
        float _minRes = 0.01f;

        bool _windowFocused = false, _dynTimer = false, _osdReset = false;
        long _focusedTime, _dynTime, _scalingTime;
        float _prevScaling = 0f;

        float _velocityX = 0f, _velocityY = 0f, _velocityZ = 0f, _velocityHorizontal = 0f, _velocityTotal = 0f,
              _positionX = 0f, _positionY = 0f, _positionZ = 0f,
              _yaw = 0f, _pitch = 0f;

        public MemoryHandler(Process game) {
            _game = game ?? throw new ArgumentNullException(nameof(game), "Game process is null. How?");
            _trainer = null;
            try {
                _moduleSize = _game.MainModule.ModuleMemorySize;
            } catch(Exception e) {
                Log.Error(e, "An error occured when retrieving the game's moduleSize");
                Reset = true;
                return;
            }
            Version = GameVersion.GetVersionByModuleSize(_moduleSize);
            _scalingTime = DateTime.Now.Ticks;

            MemoryTimer = new Timer();
            MemoryTimer.Interval = Program.TimerInterval;
            MemoryTimer.Tick += (sender, e) => { MemoryTick(); };

            _restartCheatsTimer = new System.Timers.Timer(2500);
            _restartCheatsTimer.Elapsed += (sender, e) => { RestartTick(); };
            _restartCheatsTimer.Start();

            Reset = false;
            Initialize();
            Log.Information("Initialized MemoryHandler. Game Version: {Version} [{ModuleSize}]", Version.Name, _moduleSize);
        }


        private void MemoryTick() {
            if(!_restartCheatsTimer.Enabled) _restartCheatsTimer.Start();

            DerefPointers();

            if(Version.Name == "1.0 (Release)") SetFlag(!_game.ReadValue<bool>(_rampJumpPtr), "slopeboost");

            TrainerSupported = _positionPtr != IntPtr.Zero;
            if(TrainerSupported) ReadTrainerValues();

            if(_cheatsConsolePtr != IntPtr.Zero && _game.ReadValue<bool>(_cheatsConsolePtr) != !EnableCheats) {
                //_game.VirtualProtect(_cheatsConsolePtr, 128, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_cheatsConsolePtr, new byte[1] { Convert.ToByte(!EnableCheats) });
            }

            if(_cheatsBindsPtr != IntPtr.Zero && _game.ReadValue<bool>(_cheatsBindsPtr) != !EnableCheats) {
                //_game.VirtualProtect(_cheatsBindsPtr, 128, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_cheatsBindsPtr, new byte[1] { Convert.ToByte(!EnableCheats) });
            }

            // Read current resolution scaling percentage
            if(_currentResScalePtr != IntPtr.Zero) CurrentResScaling = _game.ReadValue<float>(_currentResScalePtr);
            if(CurrentResScaling != _prevScaling) {
                _prevScaling = CurrentResScaling;
                _scalingTime = DateTime.Now.Ticks;
            }

            // com_skipIntroVideo
            if(_skipIntroPtr != IntPtr.Zero && !_game.ReadValue<bool>(_skipIntroPtr))
                _game.WriteBytes(_skipIntroPtr, new byte[1] { 1 });

            // r_antialiasing
            if(_aliasingPtr != IntPtr.Zero && _game.ReadValue<byte>(_aliasingPtr) < 2 && _game.ReadValue<bool>(_aliasingPtr) != _antiAliasing)
                _game.WriteBytes(_aliasingPtr, new byte[1] { Convert.ToByte(_antiAliasing) });

            // pauseMenu_delayUNPrompt
            if(_unDelayPtr != IntPtr.Zero && _game.ReadValue<bool>(_unDelayPtr) != _unDelay)
                _game.WriteBytes(_unDelayPtr, new byte[1] { Convert.ToByte(_unDelay) });

            // com_skipKeyPressOnLoadScreens
            if(_continuePtr != IntPtr.Zero && _game.ReadValue<bool>(_continuePtr) != _autoContinue)
                _game.WriteBytes(_continuePtr, new byte[1] { Convert.ToByte(_autoContinue) });

            _row1 = _row2 = _row3 = _row4 = _row5 = _row6 = _row7 = _row8 = _row9 = _cpu = _gpuV = _gpuN = "";
            _row1 = METRICS_FPS_TEXT + ((_osdFlagOutOfDate) ? "*" : "");

            if(!EnableOSD && !_osdReset) {
                _row1 = METRICS_FPS_TEXT;
                _row2 = METRICS_FRAMETIME_TEXT;
                _row3 = METRICS_RESOLUTION_TEXT;
                if(_row6Ptr == IntPtr.Zero) {
                    _row4 = METRICS_HDR_TEXT;
                    _row5 = METRICS_VULKAN_TEXT;
                    _row6 = METRICS_VRAM_TEXT;
                    //_row7 = METRICS_DRIVER_TEXT;
                } else {
                    _row4 = METRICS_RAYTRACING_TEXT;
                    _row5 = METRICS_HDR_TEXT;
                    _row6 = METRICS_DLSS_TEXT;
                    _row7 = METRICS_VULKAN_TEXT;
                    _row8 = METRICS_VRAM_TEXT;
                    //_row9 = METRICS_DRIVER_TEXT;
                }
                ModifyMetricRows();
                _osdReset = true;
            }

            if(!_externalTrainerFlag && EnableOSD) {
                if(!_trainerFlag || (!_osdFlagMeath00k && !EnableCheats)) {
                    if(_osdFlagRestartGame) _cheatString = "RESTART GAME";
                    if(EnableCheats) 
                        _cheatString = "CHEATS ENABLED";
                    else if(_osdFlagMeath00k)
                        _cheatString = "MEATH00K";

                    StringBuilder builderR2 = new(Version.Name.Replace(" Rev ", "r").Replace(" (Gamepass)", ""));
                    builderR2.Replace("1.0 (Release)", "Release");
                    if(_osdFlagMacro || _osdFlagFirewall || _osdFlagSlopeboost || _osdFlagReshade || !_osdFlagLimiter) {
                        builderR2.Append(" (");
                        if(_osdFlagMacro) builderR2.Append('M');
                        if(_osdFlagFirewall) builderR2.Append('F');
                        if(_osdFlagReshade) builderR2.Append('R');
                        if(_osdFlagSlopeboost) builderR2.Append('S');
                        if(!_osdFlagLimiter) builderR2.Append('L');
                        builderR2.Append(')');
                    }
                    var cheats = (_osdFlagMeath00k || EnableCheats || _osdFlagRestartGame) ? _cheatString : string.Empty;
                    if(_osdFlagModded) {
                        if(cheats == string.Empty) {
                            cheats = "MODDED CLIENT";
                        } else {
                            if(!_osdFlagRestartGame)
                                cheats += " (MOD)";
                        }
                    }
                    var scaling = string.Empty;
                    if(((DateTime.Now.Ticks - _scalingTime) / 10000) <= 3000) {
                        if(ReadDynamicRes() || ReadForceRes() > 0f) {
                            scaling = string.Format("{0:0.00}x [{1}]", CurrentResScaling, ReadDynamicRes() ? GetTargetFPS() : "S");
                        } else {
                            scaling = "rs-off";
                        }
                    }
                    if(_minimalOSD) {
                        var modded = cheats == "MODDED CLIENT";
                        if(modded) cheats = string.Empty;
                        var row2Mod = (_scrollString != string.Empty) ? '[' + _scrollString + ']' : cheats;
                        _row1 += ' ';
                        builderR2.Replace("(", "");
                        builderR2.Insert(0, "MOD-");
                        if(!_osdFlagOutOfDate) {
                            _row1 += '(';
                        } else {
                            builderR2.Insert(0, '(');
                        }
                        if(_osdFlagModded) {
                            builderR2.Replace("CHEATS ENABLED", "CHEATS (MOD)");
                            builderR2.Replace("MEATH00K", "MEATH00K (MOD)");
                        }
                        _row2 = builderR2.ToString();
                        if(!_row2.Contains(')')) _row2 += ')';
                        if(row2Mod != string.Empty) {
                            _row1 = _row1[..7] + (_row1.Contains('*') ? ' ' : row2Mod[0]);
                            _row2 = _row1.Contains('*') ? row2Mod : row2Mod[1..];
                        } else {
                            if(scaling != string.Empty) {
                                _row2 = _row2.Replace(")", " [" + scaling.Replace("[", "") + ')');
                            }
                        }
                    } else {
                        _row2 = builderR2.ToString();
                        if(cheats == string.Empty) cheats = scaling;
                        if(_cpuPtr == IntPtr.Zero) {
                            _row3 = (_scrollString != string.Empty) ? _scrollString : cheats;
                            _cpu = "";
                        } else {
                            _cpu = (_scrollString != string.Empty) ? _scrollString : cheats;
                            _row3 = "";
                        }
                    }
                    if(_metricsPtr != IntPtr.Zero) SetMetrics((byte) (_minimalOSD ? 1 : 2));
                    ModifyMetricRows();
                    _osdReset = false;
                } else {
                    string velocity = string.Format("vel: {0:0.00}", _velocityTotal),
                           position = string.Format("x: {0:0.00} y: {1:0.00} z: {2:0.00}", _positionX, _positionY, _positionZ),
                           hzVelocity = string.Format("h: {0:0.00} v: {1:0.00}", _velocityHorizontal, _velocityZ),
                           yaw = string.Format("yaw: {0:0.0}", _yaw),
                           pitch = string.Format("pitch: {0:0.0}", _pitch);

                    _row2 = velocity;
                    _gpuN = position;
                    if(_cpuPtr != IntPtr.Zero) {
                        _cpu = hzVelocity;
                    } else {
                        _row3 = hzVelocity;
                        _cpu = "";
                    }
                    if(_row6Ptr != IntPtr.Zero) {
                        _row8 = yaw;
                        _row9 = pitch;
                    } else {
                        _row6 = yaw;
                        _row7 = pitch;
                    }
                    if(_metricsPtr != IntPtr.Zero) SetMetrics(2);
                    ModifyMetricRows();
                    _osdReset = false;
                }
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
                        } else {
                            if(((DateTime.Now.Ticks - _focusedTime) / 10000) >= 2750) {
                                var unlocked = UnlockResScale();
                                if(unlocked) SendKeys.Send("%(~)");
                                _unlockResFlag = false;
                                _windowFocused = false;
                                _resUnlocked = unlocked;
                                HotkeyHandler.Instance.ToggleResScaleKeys(true);
                                if(unlocked) SendKeys.Send("%(~)");
                            }
                        }
                    }
                }
            }
            if(_scheduleScaling && ReadyToUnlockRes()) {
                if(!_dynTimer) {
                    _dynTime = DateTime.Now.Ticks;
                    _dynTimer = true;
                }
                if(_dynTimer && ((DateTime.Now.Ticks - _dynTime) / 10000) >= 2750) {
                    EnableResolutionScaling();
                    _scheduleScaling = false;
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
                    if(!_osdFlagMeath00k) SetFlag(true, "restart");
                    _externalTrainerFlag = true;
                    Log.Information("Trainer process found running.");
                } else {
                    _trainer = null;
                    _externalTrainerFlag = false;
                }
            } else {
                if(_trainer.HasExited) {
                    _trainer = null;
                    _externalTrainerFlag = false;
                }
            }
            if(!_osdFlagRestartGame) {
                if(!_osdFlagMeath00k && processes.FindAll(x => x.ProcessName.ToLower().Contains("cheatengine")).Count > 0) {
                    SetFlag(true, "restart");
                    _cheatString = "RESTART GAME";
                    Log.Warning("CheatEngine process found running.");
                }
            }
        }

        private void ReadTrainerValues() {
            _velocityX = _game.ReadValue<float>(_velocityPtr);
            _velocityY = _game.ReadValue<float>(_velocityPtr + 4);
            _velocityZ = _game.ReadValue<float>(_velocityPtr + 8);
            _velocityHorizontal = (float) Math.Sqrt((_velocityX * _velocityX) + (_velocityY * _velocityY));
            _velocityTotal = (float) Math.Sqrt((_velocityX * _velocityX) + (_velocityY * _velocityY) + (_velocityZ * _velocityZ));

            if(!_osdFlagMeath00k && !EnableCheats) return;
            
            _positionX = _game.ReadValue<float>(_positionPtr);
            _positionY = _game.ReadValue<float>(_positionPtr + 4);
            _positionZ = _game.ReadValue<float>(_positionPtr + 8);

            _pitch = (360f + _game.ReadValue<float>(_rotationPtr)) % 360f;
            _yaw = (360f + _game.ReadValue<float>(_rotationPtr + 4)) % 360f;
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
            } catch(Exception e) {
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
            if(_minimalOSD) {
                _row1 = _row1.Substring(0, 7) + (_row1.Contains('*') ? " " : _row3[0]);
                _row2 = _row1.Contains('*') ? _row3 : _row3[1..];
                _game.WriteBytes(_row1Ptr, ToByteArray(_row1, 20));
                _game.WriteBytes(_row1Ptr + OFFSET_ROW2, ToByteArray(_row2, 18));
            } else {
                if(_cpuPtr == IntPtr.Zero)
                    _game.WriteBytes(_row1Ptr + OFFSET_ROW3, ToByteArray(_row3, 19));
                else
                    _game.WriteBytes(_cpuPtr, ToByteArray(_row3, 64));
            }
        }

        private void SetMetrics(byte val) {
            if(val > 6) return;
            _game.WriteBytes(_metricsPtr, new byte[] { val });
        }

        private bool UnlockResScale() {
            var result = SetResScales();
            if(_minResPtr != IntPtr.Zero) _game.WriteBytes(_minResPtr, FloatToBytes(_minRes));
            if(_autoScaling) {
                EnableResolutionScaling();
                _autoScaling = false;
            }
            return result;
        }
        /// <summary>
        /// Schedules dynamic scaling to be turned on when the game is loaded
        /// </summary>
        /// <param name="targetFPS">FPS Target the scaling will try to hit</param>
        public void ScheduleResolutionScaling(int targetFPS) {
            if(Version.Name.Contains("Unknown")) return;
            _targetFPS = targetFPS;
            _scheduleScaling = true;
        }
        /// <summary>
        /// Enables dynamic resolution scaling with a specified target FPS.<br/>
        /// Also disables static scaling
        /// </summary>
        /// <param name="targetFPS">FPS Target the scaling will try to hit</param>
        public void EnableDynamicScaling(int targetFPS) {
            DisableStaticScaling();
            if(_dynamicResPtr != IntPtr.Zero) _game.WriteBytes(_dynamicResPtr, new byte[] { 1 });
            if(_raiseMSPtr != IntPtr.Zero) _game.WriteBytes(_raiseMSPtr, FloatToBytes((1000f / targetFPS) * 0.95f));
            if(_dropMSPtr != IntPtr.Zero) _game.WriteBytes(_dropMSPtr, FloatToBytes((1000f / targetFPS) * 0.99f));
        }
        /// <summary>
        /// Disables dynamic resolution scaling
        /// </summary>
        public void DisableDynamicScaling() {
            if(_dynamicResPtr != IntPtr.Zero) _game.WriteBytes(_dynamicResPtr, new byte[] { 0 });
        }
        /// <summary>
        /// Enables static resolution scaling.<br/>
        /// Also disables dynamic scaling
        /// </summary>
        public void EnableStaticScaling() {
            DisableDynamicScaling();
            // rs_forceResolution takes the set value and sets the res scaling to the corresponding item of the scales array (compared to the default array)
            //  So since 0.5f is the smallest value in the default array, 0.5f and anything below it will select the final value of the array, which is the lowest scaling value
            //  Other values will take the next highest corresponding value. e.g. 0.55f would select 0.56f normally, which is the 29th value of the array, so the 29th value of the new array is selected
            // Because DESRU generates a new array depending on what minimum res scale value the user selected, the forced res will be set to 0.5f to keep that min value selected
            float scale = Version.Name.Contains("6.66 Rev 2") ? 0.5f : _minRes;
            if(_forceResPtr != IntPtr.Zero) _game.WriteValue(_forceResPtr, scale);
        }
        /// <summary>
        /// Disables static resolution scaling
        /// </summary>
        public void DisableStaticScaling() {
            if(_forceResPtr != IntPtr.Zero) _game.WriteValue(_forceResPtr, 0f);
        }
        /// <summary>
        /// Toggles Dynamic Resolution Scaling
        /// </summary>
        public void ToggleDynamicScaling() {
            if(_dynamicResPtr == IntPtr.Zero) return;
            if(ReadDynamicRes()) DisableDynamicScaling();
            else EnableDynamicScaling(_targetFPS);
        }
        public void ToggleStaticScaling() {
            if(_forceResPtr == IntPtr.Zero) return;
            if(ReadForceRes() > 0f) DisableStaticScaling();
            else EnableStaticScaling();
        }
        public void ToggleResolutionScaling() {
            if(UseDynamicScaling) ToggleDynamicScaling();
            else ToggleStaticScaling();
        }
        public void EnableResolutionScaling() {
            if(UseDynamicScaling) EnableDynamicScaling(_targetFPS);
            else EnableStaticScaling();
        }
        private static byte[] FloatToBytes(float f) {
            byte[] output = new byte[4];
            float[] fArray = new float[1] { f };
            Buffer.BlockCopy(fArray, 0, output, 0, 4);
            return output;
        }

        private bool SetResScales() {
            if(!Version.Name.Contains("6.66 Rev 2") && _minRes >= 0.5f) return false; // No need to change res scales since 50% is the default minimum in game
            float[] scales = ONEPERCENT_RES_SCALES;
            if(Version.Name.Contains("6.66 Rev 2") && _minRes >= 0.02f) {
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
            return true;
        }

        /// <summary>
        /// Sets the state of the flags that will be shown next to the FPS counter.
        /// </summary>
        /// <param name="flag">State of the flag</param>
        /// <param name="flagName">Name of the flag being set</param>
        public void SetFlag(bool flag, string flagName) {
            switch(flagName) {
                case "meath00k":
                    _osdFlagMeath00k = flag;
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
                case "minimal":
                    _minimalOSD = flag;
                    break;
                case "trainer":
                    _trainerFlag = flag;
                    break;
                case "modded":
                    _osdFlagModded = flag;
                    break;
            }
        }

        public void SetCVAR(bool cvar, string cvarName) {
            switch(cvarName) {
                case "antialiasing":
                    _antiAliasing = cvar;
                    break;
                case "undelay":
                    _unDelay = cvar;
                    break;
                case "autocontinue":
                    _autoContinue = cvar;
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
                "meath00k" => _osdFlagMeath00k,
                "firewall" => _osdFlagFirewall,
                "limiter" => _osdFlagLimiter,
                "macro" => _osdFlagMacro,
                "outofdate" => _osdFlagOutOfDate,
                "reshade" => _osdFlagReshade,
                "restart" => _osdFlagRestartGame,
                "slopeboost" => _osdFlagSlopeboost,
                "resunlocked" => _resUnlocked,
                "unlockscheduled" => _unlockResFlag,
                "modded" => _osdFlagModded,
                _ => false
            };
        }
        /// <summary>
        /// Schedules the resolution scaling unlock for the next time the game is in focus
        /// </summary>
        /// <param name="auto">Enable scaling at the same time</param>
        /// <param name="targetFPS">Target FPS for dynamic scaling</param>
        public void ScheduleResUnlock(bool auto, int targetFPS) {
            if(Version.Name.Contains("Unknown")) return;
            _targetFPS = targetFPS;
            if(_resUnlocked && !Version.Name.Contains("6.66 Rev 2")) {
                if(_minResPtr != IntPtr.Zero) _game.WriteBytes(_minResPtr, FloatToBytes(_minRes));
                return;
            }
            _unlockResFlag = true;
            _autoScaling = auto;
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

        public (float, float, float, float, float) GetPlayerPosition() => (_positionX, _positionY, _positionZ, _yaw, _pitch);
        public (float, float, float, float, float) GetPlayerVelocity() => (_velocityX, _velocityY, _velocityZ, _velocityHorizontal, _velocityTotal);

        /// <summary>
        /// Reads the current value of rs_enable from memory
        /// </summary>
        /// <returns><see langword="true"/> if rs_enable is set to 1. <see langword="false"/> otherwise</returns>
        public bool ReadDynamicRes() {
            if(_dynamicResPtr != IntPtr.Zero) return _game.ReadValue<bool>(_dynamicResPtr);
            return false;
        }
        /// <summary>
        /// Reads the current value of rs_forceResolution from memory
        /// </summary>
        /// <returns><see cref="float"/> value of rs_forceResolution</returns>
        public float ReadForceRes() {
            if(_forceResPtr != IntPtr.Zero) return _game.ReadValue<float>(_forceResPtr);
            return 0f;
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
        /// Reads the current value of rs_raiseMilliseconds from memory
        /// </summary>
        /// <returns>A <see cref="float"/> representing minimum frametime for scaling. <c>-1f</c> if it cannot be read from memory</returns>
        public float ReadRaiseMillis() {
            if(_raiseMSPtr != IntPtr.Zero) return _game.ReadValue<float>(_raiseMSPtr);
            return -1f;
        }
        /// <summary>
        /// Fetches the current dynamic resolution scaling target framerate
        /// </summary>
        /// <returns>An <see cref="int"/> representing the target framerate</returns>
        public int GetTargetFPS() {
            var ms = ReadRaiseMillis();
            if(ms < 0f) return -1;
            return (int) (1000 / (ms / 0.95f));
        }

        /// <summary>
        /// Checks if the game is currently loading a level or in the main menu
        /// </summary>
        /// <returns><see langword="true"/> if loading or in menu</returns>
        public bool IsLoadingOrInMenu() => _game.ReadValue<bool>(_isLoadingPtr) || _game.ReadValue<byte>(_isLoading2Ptr) > 0 || !_game.ReadValue<bool>(_isInGamePtr);

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            try {
                if(_row1DP != null) {
                    _row1DP.DerefOffsets(_game, out _row1Ptr);
                    _row6DP?.DerefOffsets(_game, out _row6Ptr);
                }
                _gpuVendorDP?.DerefOffsets(_game, out _gpuVendorPtr);
                _gpuNameDP?.DerefOffsets(_game, out _gpuNamePtr);
                _cpuDP?.DerefOffsets(_game, out _cpuPtr);

                _metricsDP?.DerefOffsets(_game, out _metricsPtr);
                _maxHzDP?.DerefOffsets(_game, out _maxHzPtr);
                _skipIntroDP?.DerefOffsets(_game, out _skipIntroPtr);
                _rampJumpDP?.DerefOffsets(_game, out _rampJumpPtr);
                _aliasingDP?.DerefOffsets(_game, out _aliasingPtr);
                _unDelayDP?.DerefOffsets(_game, out _unDelayPtr);
                _continueDP?.DerefOffsets(_game, out _continuePtr);

                _minResDP?.DerefOffsets(_game, out _minResPtr);
                _dynamicResDP?.DerefOffsets(_game, out _dynamicResPtr);
                _resScalesDP?.DerefOffsets(_game, out _resScalesPtr);
                _raiseMSDP?.DerefOffsets(_game, out _raiseMSPtr);
                _dropMSDP?.DerefOffsets(_game, out _dropMSPtr);
                _forceResDP?.DerefOffsets(_game, out _forceResPtr);
                _currentResScaleDP?.DerefOffsets(_game, out _currentResScalePtr);

                _velocityDP?.DerefOffsets(_game, out _velocityPtr);
                _positionDP?.DerefOffsets(_game, out _positionPtr);
                _rotationDP?.DerefOffsets(_game, out _rotationPtr);

                _isLoadingDP?.DerefOffsets(_game, out _isLoadingPtr);
                _isLoading2DP?.DerefOffsets(_game, out _isLoading2Ptr);
                _isInGameDP?.DerefOffsets(_game, out _isInGamePtr);

                _cheatsConsoleDP?.DerefOffsets(_game, out _cheatsConsolePtr);
                _cheatsBindsDP?.DerefOffsets(_game, out _cheatsBindsPtr);
            } catch(Win32Exception e) {
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

        private void Initialize() {
            _row1DP = _row6DP = _gpuVendorDP = _gpuNameDP = _cpuDP = null;
            _metricsDP = _maxHzDP = _rampJumpDP = _resScalesDP = _minResDP = _dynamicResDP = _raiseMSDP = _dropMSDP = _skipIntroDP = null;
            _aliasingDP = _unDelayDP = _continueDP = null;

            if(!SetCurrentCheatOffsets()) {
                ScanForCheatsPointers();
            }

            // Launch Param Cheats are disabled ASAP to prevent any command/cvar changes coming from the user's set launch parameters
            var paramDP = CreateDP(_currentCheatOffsets.LaunchParams);
            paramDP.DerefOffsets(_game, out IntPtr paramPtr);
            //_game.VirtualProtect(paramPtr, 1024, MemPageProtect.PAGE_READWRITE);
            _game.WriteBytes(paramPtr, StringUtil.ConvertHexStringToBytes(LAUNCHPARAMS_GLOBAL_R));

            _cheatsConsoleDP = CreateDP(_currentCheatOffsets.Console);
            _cheatsBindsDP = CreateDP(_currentCheatOffsets.Binds);

            if(!SetCurrentKnownOffsets()) {
                SigScans();
            }
            _row1DP = CreateDP(_currentOffsets.Row1);
            _row6DP = CreateDP(_currentOffsets.Row6);

            _resScalesDP = CreateDP(_currentOffsets.ResScales);

            _gpuVendorDP = CreateDP(_currentOffsets.GPUVendor);
            _gpuNameDP = CreateDP(_currentOffsets.GPUName);
            _cpuDP = CreateDP(_currentOffsets.CPU, 0x0);

            _metricsDP = CreateDP(_currentOffsets.Metrics);
            if(_currentOffsets.Metrics == 0) SetFlag(true, "minimal");
            _maxHzDP = CreateDP(_currentOffsets.MaxHz);
            _skipIntroDP = CreateDP(_currentOffsets.SkipIntro);
            _aliasingDP = CreateDP(_currentOffsets.AA);
            _unDelayDP = CreateDP(_currentOffsets.UNDelay);
            _continueDP = CreateDP(_currentOffsets.AutoContinue);

            _minResDP = CreateDP(_currentOffsets.MinRes);
            _dynamicResDP = CreateDP(_currentOffsets.DynamicRes);
            _raiseMSDP = CreateDP(_currentOffsets.RaiseMS);
            _dropMSDP = CreateDP(_currentOffsets.DropMS);
            _forceResDP = CreateDP(_currentOffsets.ForceRes);
            _currentResScaleDP = CreateDP(_currentOffsets.CurrentScaling);
            if(Version.Name == "1.0 (Release)") _rampJumpDP = CreateDP(0x6126430);

            if(int.TryParse(Version.Name[0..1], out int versionMajor)) {
                _velocityDP = CreateDP(_currentOffsets.Velocity, versionMajor < 3 ? VEL_OFFSETS_OLD : VEL_OFFSETS_CURRENT);
            } else {
                _velocityDP = CreateDP(_currentOffsets.Velocity, VEL_OFFSETS_OLD);
            }

            _positionDP = CreateDP(_currentOffsets.Position);
            _rotationDP = CreateDP(_currentOffsets.Rotation);

            _isLoadingDP = CreateDP(_currentOffsets.IsLoading);
            _isLoading2DP = CreateDP(_currentOffsets.IsLoading2);
            _isInGameDP = CreateDP(_currentOffsets.IsInGame);
        }

        private static DeepPointer? CreateDP(int baseOffset, params int[] offsets) => baseOffset != 0 ? new("DOOMEternalx64vk.exe", baseOffset, offsets) : null;

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
            } else {
                Log.Error("Could not find the perf metrics rows in memory. Is this even DOOM Eternal?");
                return;
            }
            Log.Information("Scanning for resolution scale values.");
            res = scanner.Scan(resTarget);
            if(res != IntPtr.Zero) Log.Information("Found resolution scale values.");
            KnownOffsets ko = new(Version.Name, GetOffset(r1), GetOffset(r6), GetOffset(res));
            ScannedOffsetList.Add(ko);
            _currentOffsets = ko;

            string jsonString = JsonConvert.SerializeObject(ScannedOffsetList, Formatting.Indented);
            File.WriteAllText(@".\scannedOffsets.json", jsonString);
            Log.Information("Added Unknown Version ({ModuleSize}) to known offset list.", _moduleSize);
        }

        private void ScanForCheatsPointers() {
            SigScanTarget paramsTargetU  = new(LAUNCHPARAMS_GLOBAL_U),
                          paramsTargetR  = new(LAUNCHPARAMS_GLOBAL_R),
                          consoleTargetU = new(CONSOLE_GLOBAL_U),
                          consoleTargetR = new(CONSOLE_GLOBAL_R),
                          bindsTargetU,
                          bindsTargetR;

            if(int.TryParse(Version.Name[0..1], out int versionMajor)) {
                if(versionMajor >= 6) {
                    bindsTargetU = new(BINDS_PATCH6_U);
                    bindsTargetR = new(BINDS_PATCH6_R);
                } else if(versionMajor == 5) {
                    bindsTargetU = new(BINDS_PATCH5_U);
                    bindsTargetR = new(BINDS_PATCH5_R);
                } else {
                    bindsTargetU = new(BINDS_GLOBAL_U);
                    bindsTargetR = new(BINDS_GLOBAL_R);
                }
            } else {
                bindsTargetU = new(BINDS_GLOBAL_U);
                bindsTargetR = new(BINDS_GLOBAL_R);
            }

            SignatureScanner scanner = new(_game, _game.MainModule.BaseAddress, _game.MainModule.ModuleMemorySize);

            int console, binds, param;

            var scannedPtr = scanner.Scan(paramsTargetU);
            if(scannedPtr == IntPtr.Zero) scannedPtr = scanner.Scan(paramsTargetR);
            param = GetOffset(scannedPtr);

            scannedPtr = scanner.Scan(consoleTargetU);
            if(scannedPtr == IntPtr.Zero) scannedPtr = scanner.Scan(consoleTargetR);
            console = GetOffset(scannedPtr) + 0x5;

            scannedPtr = scanner.Scan(bindsTargetU);
            if(scannedPtr == IntPtr.Zero) scannedPtr = scanner.Scan(bindsTargetR);
            binds = GetOffset(scannedPtr) + 0x5;

            CheatOffsets co = new(Version.Name, console, binds, param);
            CheatOffsetList.Add(co);
            _currentCheatOffsets = co;

            string jsonString = JsonConvert.SerializeObject(CheatOffsetList, Formatting.Indented);
            File.WriteAllText(@".\cheatOffsets.json", jsonString);
            Log.Information("Added Version {Version} ({ModuleSize}) to cheat offset list.", Version.Name, _moduleSize);
        }
        private int GetOffset(IntPtr pointer) => (pointer != IntPtr.Zero) ? (int) (pointer.ToInt64() - _game.MainModule.BaseAddress.ToInt64()) : 0;

        private bool SetCurrentKnownOffsets() {
            foreach(KnownOffsets k in OffsetList) {
                if(k.Version == Version.Name) {
                    _currentOffsets = k;
                    return true;
                }
            }
            foreach(KnownOffsets k in ScannedOffsetList) {
                if(k.Version == Version.Name) {
                    _currentOffsets = k;
                    Log.Information("Version {Version} is not officially supported. Using previously scanned offsets.", Version.Name);
                    return true;
                }
            }
            Log.Warning("Offset Lists do not contain {Version}.", Version.Name);
            return false;
        }
        private bool SetCurrentCheatOffsets() {
            foreach(CheatOffsets c in CheatOffsetList) {
                if(c.Version == Version.Name) {
                    _currentCheatOffsets = c;
                    return true;
                }
            }
            Log.Warning("Cheat Offset List does not contain {Version}.", Version.Name);
            return false;
        }
    }
}
