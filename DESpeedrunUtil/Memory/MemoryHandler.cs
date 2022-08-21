﻿using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {
        private readonly string SigScanFPS = "2569204650530000252E32666D7300004672616D65203A202575";
        // DLSS does not show up if you don't have a capable gpu (NVIDIA RTX) BUT it still exists in memory in the exact same spot.
        private readonly string SigScanDLSS = "444C5353203A2025730000000000000056756C6B616E202573";

        public static List<KnownOffsets> OffsetList = new();
        KnownOffsets CurrentOffsets;

        DeepPointer CPUDP;

        IntPtr MaxHzPtr, MetricsPtr,
               Row1Ptr, Row2Ptr, Row3Ptr, Row4Ptr, Row5Ptr, Row6Ptr, Row7Ptr, Row8Ptr, Row9Ptr,
               GPUVendorPtr, GPUNamePtr, CPUPtr;

        Process Game;
        int ModuleSize;
        public string Version { get; init; }

        public MemoryHandler(Process game) {
            Game = game;
            ModuleSize = game.MainModule.ModuleMemorySize;
            Version = TranslateModuleSize();
            
            Initialize();
        }

        public void TestRows() {
            Game.VirtualProtect(Row1Ptr, 1024, MemPageProtect.PAGE_READWRITE);
            Game.WriteBytes(Row1Ptr, ToByteArray("Row 1", 8));
            Game.WriteBytes(Row2Ptr, ToByteArray("Row 2", 79));
            Game.WriteBytes(Row3Ptr, ToByteArray("Row 3", 19));
            Game.WriteBytes(Row4Ptr, ToByteArray("Row 4", 7));
            Game.WriteBytes(Row5Ptr, ToByteArray("Row 5", 34));
            Game.WriteBytes(Row6Ptr, ToByteArray("Row 6", 34));
            Game.WriteBytes(Row7Ptr, ToByteArray("Row 7", 34));
            if(Row8Ptr.ToInt64() != 0) Game.WriteBytes(Row8Ptr, ToByteArray("Row 8", 34));
            if(Row9Ptr.ToInt64() != 0) Game.WriteBytes(Row9Ptr, ToByteArray("Row 9", 34));
            if(CPUPtr.ToInt64() != 0) {
                Game.VirtualProtect(CPUPtr, 1024, MemPageProtect.PAGE_READWRITE);
                Game.WriteBytes(CPUPtr, ToByteArray("CPU", 64));
            }
            if(GPUVendorPtr.ToInt64() != 0) {
                Game.VirtualProtect(GPUVendorPtr, 1024, MemPageProtect.PAGE_READWRITE);
                Game.WriteBytes(GPUVendorPtr, ToByteArray("GPU Vendor", 64));
            }
            if(GPUNamePtr.ToInt64() != 0) {
                Game.VirtualProtect(GPUNamePtr, 1024, MemPageProtect.PAGE_READWRITE);
                Game.WriteBytes(GPUNamePtr, ToByteArray("GPU Name", 64));
            }
        }

        public bool CanCapFPS() => MaxHzPtr.ToInt64() != 0;

        public void CapFPS(int cap) {
            if(CanCapFPS()) {
                Game.WriteBytes(MaxHzPtr, BitConverter.GetBytes((short) cap));
            }
        }

        public int ReadMaxHz() {
            int cap = -1;
            if(CanCapFPS()) Game.ReadValue(MaxHzPtr, out cap);
            return cap;
        }

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            if(CPUDP != null) CPUDP.DerefOffsets(Game, out CPUPtr);
            else CPUPtr = new IntPtr(0);
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
            return ModuleSize switch {
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
                _ => "Unknown (" + ModuleSize.ToString() + ")",
            };
        }

        private void Initialize() {
            var moduleBase = Game.MainModule.BaseAddress;
            if(!SetCurrentKnownOffsets(Version)) {
                SigScanRows();
            }else {
                Row1Ptr = (CurrentOffsets.Row1 != 0) ? moduleBase + CurrentOffsets.Row1 : IntPtr.Zero;
                Row2Ptr = (CurrentOffsets.Row2 != 0) ? moduleBase + CurrentOffsets.Row2 : IntPtr.Zero;
                Row3Ptr = (CurrentOffsets.Row3 != 0) ? moduleBase + CurrentOffsets.Row3 : IntPtr.Zero;
                Row4Ptr = (CurrentOffsets.Row4 != 0) ? moduleBase + CurrentOffsets.Row4 : IntPtr.Zero;
                Row5Ptr = (CurrentOffsets.Row5 != 0) ? moduleBase + CurrentOffsets.Row5 : IntPtr.Zero;
                Row6Ptr = (CurrentOffsets.Row6 != 0) ? moduleBase + CurrentOffsets.Row6 : IntPtr.Zero;
                Row7Ptr = (CurrentOffsets.Row7 != 0) ? moduleBase + CurrentOffsets.Row7 : IntPtr.Zero;
                Row8Ptr = (CurrentOffsets.Row8 != 0) ? moduleBase + CurrentOffsets.Row8 : IntPtr.Zero;
                Row9Ptr = (CurrentOffsets.Row9 != 0) ? moduleBase + CurrentOffsets.Row9 : IntPtr.Zero;
            }
            GPUVendorPtr = (CurrentOffsets.GPUVendor != 0) ? moduleBase + CurrentOffsets.GPUVendor : IntPtr.Zero;
            GPUNamePtr = (CurrentOffsets.GPUName != 0) ? moduleBase + CurrentOffsets.GPUName : IntPtr.Zero;
            MetricsPtr = (CurrentOffsets.Metrics != 0) ? moduleBase + CurrentOffsets.Metrics : IntPtr.Zero;
            MaxHzPtr = (CurrentOffsets.MaxHz != 0) ? moduleBase + CurrentOffsets.MaxHz : IntPtr.Zero;
            CPUDP = new DeepPointer("DOOMEternalx64vk.exe", CurrentOffsets.CPU, 0x0);
        }

        private void SigScanRows() {
            // This only needs to be done on the first hook of the game. Offsets can be saved since they're not pointer chains.

            SigScanTarget fpsTarget = new SigScanTarget(SigScanFPS);
            SigScanTarget dlssTarget = new SigScanTarget(SigScanDLSS);
            SignatureScanner scanner = new SignatureScanner(Game, Game.MainModule.BaseAddress, Game.MainModule.ModuleMemorySize);
            Row1Ptr = scanner.Scan(fpsTarget);
            if(Row1Ptr.ToInt64() != 0) {
                Row2Ptr = Row1Ptr + 0x8;
                Row3Ptr = Row1Ptr + 0x58;
                Row4Ptr = Row1Ptr + 0x70;
                Row5Ptr = Row1Ptr + 0x78;
                Row6Ptr = Row1Ptr + 0x98;
                Row7Ptr = Row1Ptr + 0xA8;
                Row8Ptr = IntPtr.Zero;
                Row9Ptr = IntPtr.Zero;
                IntPtr dlss = scanner.Scan(dlssTarget);
                if(dlss.ToInt64() != 0) {
                    Row6Ptr = dlss;
                    Row7Ptr = dlss + 0x10;
                    Row8Ptr = dlss + 0x30;
                    Row9Ptr = dlss + 0x40;
                }
            }else {
                return;
            }
            KnownOffsets ko = new KnownOffsets(Version, GetOffset(Row1Ptr),
                GetOffset(Row2Ptr),
                GetOffset(Row3Ptr),
                GetOffset(Row4Ptr),
                GetOffset(Row5Ptr),
                GetOffset(Row6Ptr),
                GetOffset(Row7Ptr),
                GetOffset(Row8Ptr),
                GetOffset(Row9Ptr),
                0, 0, 0, 0, 0);
            OffsetList.Add(ko);
            CurrentOffsets = ko;

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(OffsetList, options);
            Debug.WriteLine(jsonString);
            File.WriteAllText(@".\offsets.json", jsonString);
        }
        private int GetOffset(IntPtr pointer) => (pointer != IntPtr.Zero) ? (int) (pointer.ToInt64() - Game.MainModule.BaseAddress.ToInt64()) : 0;
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
                    CurrentOffsets = k;
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
