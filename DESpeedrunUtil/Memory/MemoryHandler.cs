using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {
        private readonly string SIGSCAN_FPS = "2569204650530000252E32666D7300004672616D65203A202575";
        // DLSS does not show up if you don't have a capable gpu (NVIDIA RTX) BUT it still exists in memory in the exact same spot.
        private readonly string SIGSCAN_DLSS = "444C5353203A2025730000000000000056756C6B616E202573";

        public static List<KnownOffsets> OffsetList = new();
        KnownOffsets _currentOffsets;

        DeepPointer _maxHzDP, _metricsDP, _rampJumpDP,
                    _row1DP, _row2DP, _row3DP, _row4DP, _row5DP, _row6DP, _row7DP, _row8DP, _row9DP,
                    _gpuVendorDP, _gpuNameDP, _cpuDP;

        IntPtr _maxHzPtr, _metricsPtr, _rampJumpPtr,
               _row1Ptr, _row2Ptr, _row3Ptr, _row4Ptr, _row5Ptr, _row6Ptr, _row7Ptr, _row8Ptr, _row9Ptr,
               _gpuVendorPtr, _gpuNamePtr, _cpuPtr;

        Process _game;
        int _moduleSize;
        public string Version { get; init; }

        bool _cheatsFlag = false, _macroFlag = false, _firewallFlag = false, _slopeboostFlag = false, _reshadeFlag = false;

        public MemoryHandler(Process game) {
            _game = game;
            _moduleSize = game.MainModule.ModuleMemorySize;
            Version = TranslateModuleSize();

            Initialize();
        }

        public void ModifyMetricRows() {
            var row1 = "%i FPS";
            var row2 = _currentOffsets.Version.Replace(" Rev ", "r");
            if(row2 == "1.0 (Release)") row2 = "Release";
            if(_macroFlag || _firewallFlag || _slopeboostFlag) row2 += " (" + ((_macroFlag) ? "M" : "") + ((_firewallFlag) ? "F" : "") + ((_reshadeFlag) ? "R" : "") + ((_slopeboostFlag) ? "S" : "") + ")";
            var cpu = ""; var row3 = "";
            var cheatString = (_cheatsFlag) ? "CHEATS ENABLED" : "";
            if(_cpuPtr.ToInt64() == 0) {
                row3 = cheatString;
            }else {
                cpu = cheatString;
            }
            _game.VirtualProtect(_row1Ptr, 1024, MemPageProtect.PAGE_READWRITE);
            _game.WriteBytes(_row1Ptr, ToByteArray(row1, 20));
            _game.WriteBytes(_row2Ptr, ToByteArray(row2, 16));
            _game.WriteBytes(_row3Ptr, ToByteArray(row3, 19));
            _game.WriteBytes(_row4Ptr, ToByteArray("", 7));
            _game.WriteBytes(_row5Ptr, ToByteArray("", 34));
            _game.WriteBytes(_row6Ptr, ToByteArray("", 34));
            _game.WriteBytes(_row7Ptr, ToByteArray("", 34));
            if(_row8Ptr.ToInt64() != 0) _game.WriteBytes(_row8Ptr, ToByteArray("", 34));
            if(_row9Ptr.ToInt64() != 0) _game.WriteBytes(_row9Ptr, ToByteArray("", 34));
            if(_cpuPtr.ToInt64() != 0) {
                _game.VirtualProtect(_cpuPtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_cpuPtr, ToByteArray(cpu, 64));
            }
            if(_gpuVendorPtr.ToInt64() != 0) {
                _game.VirtualProtect(_gpuVendorPtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_gpuVendorPtr, ToByteArray("", 64));
            }
            if(_gpuNamePtr.ToInt64() != 0) {
                _game.VirtualProtect(_gpuNamePtr, 1024, MemPageProtect.PAGE_READWRITE);
                _game.WriteBytes(_gpuNamePtr, ToByteArray("", 64));
            }
        }

        public void SetMetrics(byte val) {
            if(val > 6) return;
            _game.WriteBytes(_metricsPtr, new byte[] { val });
        }

        /// <summary>
        /// Sets the state of the flags that will be shown next to the FPS counter.
        /// </summary>
        /// <param name="flag">State of the flag</param>
        /// <param name="flagName">Name of the flag being set</param>
        public void SetFlag(bool flag, string flagName) {
            switch(flagName) {
                case "cheats":
                    _cheatsFlag = flag;
                    break;
                case "macro":
                    _macroFlag = flag;
                    break;
                case "firewall":
                    _firewallFlag = flag;
                    break;
                case "reshade":
                    _reshadeFlag = flag;
                    break;
            }
        }
        public bool GetFlag(string flagName) {
            return flagName switch {
                "cheats" => _cheatsFlag,
                "macro" => _macroFlag,
                "firewall" => _firewallFlag,
                "reshade" => _reshadeFlag,
                _ => false
            };
        }

        public bool CanCapFPS() => _maxHzPtr.ToInt64() != 0;

        public void CapFPS(int cap) {
            if(CanCapFPS()) {
                _game.WriteBytes(_maxHzPtr, BitConverter.GetBytes((short) cap));
            }
        }

        public int ReadMaxHz() {
            int cap = -1;
            if(CanCapFPS()) _game.ReadValue(_maxHzPtr, out cap);
            return cap;
        }

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            try {
                if(_row1DP != null) _row1DP.DerefOffsets(_game, out _row1Ptr);
                if(_row2DP != null) _row2DP.DerefOffsets(_game, out _row2Ptr);
                if(_row3DP != null) _row3DP.DerefOffsets(_game, out _row3Ptr);
                if(_row4DP != null) _row4DP.DerefOffsets(_game, out _row4Ptr);
                if(_row5DP != null) _row5DP.DerefOffsets(_game, out _row5Ptr);
                if(_row6DP != null) _row6DP.DerefOffsets(_game, out _row6Ptr);
                if(_row7DP != null) _row7DP.DerefOffsets(_game, out _row7Ptr);
                if(_row8DP != null) _row8DP.DerefOffsets(_game, out _row8Ptr);
                if(_row9DP != null) _row9DP.DerefOffsets(_game, out _row9Ptr);
                if(_gpuVendorDP != null) _gpuVendorDP.DerefOffsets(_game, out _gpuVendorPtr);
                if(_gpuNameDP != null) _gpuNameDP.DerefOffsets(_game, out _gpuNamePtr);
                if(_metricsDP != null) _metricsDP.DerefOffsets(_game, out _metricsPtr);
                if(_maxHzDP != null) _maxHzDP.DerefOffsets(_game, out _maxHzPtr);
                if(_cpuDP != null) _cpuDP.DerefOffsets(_game, out _cpuPtr);
                if(_rampJumpDP != null) _rampJumpDP.DerefOffsets(_game, out _rampJumpPtr);
            } catch(Win32Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
            if(Version == "1.0 (Release)") _slopeboostFlag = _game.ReadBytes(_rampJumpPtr, 1)[0] == 0;
        }

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
                _ => "Unknown (" + _moduleSize.ToString() + ")",
            };
        }

        private void Initialize() {
            _row1DP = _row2DP = _row3DP = _row4DP = _row5DP = _row6DP = _row7DP = _row8DP = _row9DP = null;
            _gpuVendorDP = _gpuNameDP = _metricsDP = _maxHzDP = _cpuDP = null;
            if(!SetCurrentKnownOffsets(Version)) {
                SigScanRows();
            } else {
                if(_currentOffsets.Row1 != 0) _row1DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row1);
                if(_currentOffsets.Row2 != 0) _row2DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row2);
                if(_currentOffsets.Row3 != 0) _row3DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row3);
                if(_currentOffsets.Row4 != 0) _row4DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row4);
                if(_currentOffsets.Row5 != 0) _row5DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row5);
                if(_currentOffsets.Row6 != 0) _row6DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row6);
                if(_currentOffsets.Row7 != 0) _row7DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row7);
                if(_currentOffsets.Row8 != 0) _row8DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row8);
                if(_currentOffsets.Row9 != 0) _row9DP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Row9);
            }
            if(_currentOffsets.GPUVendor != 0) _gpuVendorDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.GPUVendor);
            if(_currentOffsets.GPUName != 0) _gpuNameDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.GPUName);
            if(_currentOffsets.Metrics != 0) _metricsDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.Metrics);
            if(_currentOffsets.MaxHz != 0) _maxHzDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.MaxHz);
            if(_currentOffsets.CPU != 0) _cpuDP = new DeepPointer("DOOMEternalx64vk.exe", _currentOffsets.CPU, 0x0);
            if(Version == "1.0 (Release)") _rampJumpDP = new DeepPointer("DOOMEternalx64vk.exe", 0x6126430);
        }

        private void SigScanRows() {
            // This only needs to be done on the first hook of the game. Offsets can be saved since they're not pointer chains.

            SigScanTarget fpsTarget = new SigScanTarget(SIGSCAN_FPS);
            SigScanTarget dlssTarget = new SigScanTarget(SIGSCAN_DLSS);
            SignatureScanner scanner = new SignatureScanner(_game, _game.MainModule.BaseAddress, _game.MainModule.ModuleMemorySize);
            _row1Ptr = scanner.Scan(fpsTarget);
            if(_row1Ptr.ToInt64() != 0) {
                _row2Ptr = _row1Ptr + 0x8;
                _row3Ptr = _row1Ptr + 0x58;
                _row4Ptr = _row1Ptr + 0x70;
                _row5Ptr = _row1Ptr + 0x78;
                _row6Ptr = _row1Ptr + 0x98;
                _row7Ptr = _row1Ptr + 0xA8;
                _row8Ptr = IntPtr.Zero;
                _row9Ptr = IntPtr.Zero;
                IntPtr dlss = scanner.Scan(dlssTarget);
                if(dlss.ToInt64() != 0) {
                    _row6Ptr = dlss;
                    _row7Ptr = dlss + 0x10;
                    _row8Ptr = dlss + 0x30;
                    _row9Ptr = dlss + 0x40;
                }
            } else {
                return;
            }
            KnownOffsets ko = new KnownOffsets(Version, GetOffset(_row1Ptr),
                GetOffset(_row2Ptr),
                GetOffset(_row3Ptr),
                GetOffset(_row4Ptr),
                GetOffset(_row5Ptr),
                GetOffset(_row6Ptr),
                GetOffset(_row7Ptr),
                GetOffset(_row8Ptr),
                GetOffset(_row9Ptr),
                0, 0, 0, 0, 0);
            OffsetList.Add(ko);
            _currentOffsets = ko;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(OffsetList, options);
            Debug.WriteLine(jsonString);
            File.WriteAllText(@".\offsets.json", jsonString);
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
                _ => false,
            };
        }

        private bool SetCurrentKnownOffsets(string ver) {
            foreach(KnownOffsets k in OffsetList) {
                if(k.Version == ver) {
                    _currentOffsets = k;
                    return true;
                }
            }
            Debug.WriteLine("Offset List does not contain " + ver);
            return false;
        }

        public struct KnownOffsets {
            public string Version { get; init; }

            public int Row1 { get; init; }
            public int Row2 { get; init; }
            public int Row3 { get; init; }
            public int Row4 { get; init; }
            public int Row5 { get; init; }
            public int Row6 { get; init; }
            public int Row7 { get; init; }
            public int Row8 { get; init; }
            public int Row9 { get; init; }

            public int GPUVendor { get; init; }
            public int GPUName { get; init; }
            public int CPU { get; init; }
            public int Metrics { get; init; }
            public int MaxHz { get; init; }

            public KnownOffsets(string v, int r1, int r2, int r3, int r4, int r5, int r6, int r7, int r8, int r9, int gpuv, int gpu, int cpu, int perf, int hz) {
                Version = v;
                Row1 = r1;
                Row2 = r2;
                Row3 = r3;
                Row4 = r4;
                Row5 = r5;
                Row6 = r6;
                Row7 = r7;
                Row8 = r8;
                Row9 = r9;
                GPUVendor = gpuv;
                GPUName = gpu;
                CPU = cpu;
                Metrics = perf;
                MaxHz = hz;
            }
        }
    }
}
