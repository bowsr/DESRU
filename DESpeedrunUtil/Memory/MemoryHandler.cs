using System.Diagnostics;
using System.Text;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {
        private readonly string SigScanFPS = "2569204650530000252E32666D7300004672616D65203A202575";
        private readonly string SigScanDLSS = "444C5353203A2025730000000000000056756C6B616E202573";

        DeepPointer MaxHzDP, PerfMetricsDP,
                    GPUVendorDP, GPUNameDP, CPUDP;

        IntPtr      MaxHzPtr, PerfMetricsPtr,
                    MetricsRow1Ptr, MetricsRow2Ptr, MetricsRow3Ptr, MetricsRow4Ptr,
                    MetricsRow5Ptr, MetricsRow6Ptr, MetricsRow7Ptr, MetricsRow8Ptr, MetricsRow9Ptr,
                    GPUVendorPtr, GPUNamePtr, CPUPtr;

        Process Game;
        int ModuleSize;
        /// <summary>
        /// Determines what Medium Perf Metrics looks like. 0: Base, 1: + CPU, 2: + RT/DLSS
        /// </summary>
        int PerfMetricsType = 0;
        public bool VersionIsSupported { get; private set; }

        public MemoryHandler(Process game) {
            Game = game;
            ModuleSize = game.MainModule.ModuleMemorySize;
            
            SetUpPointers();
        }

        public void TestRows() {
            Game.VirtualProtect(MetricsRow1Ptr, 1024, MemPageProtect.PAGE_READWRITE);
            Game.WriteBytes(MetricsRow1Ptr, ToByteArray("Row 1", 8));
            Game.WriteBytes(MetricsRow2Ptr, ToByteArray("Row 2", 79));
            Game.WriteBytes(MetricsRow3Ptr, ToByteArray("Row 3", 19));
            Game.WriteBytes(MetricsRow4Ptr, ToByteArray("Row 4", 7));
            Game.WriteBytes(MetricsRow5Ptr, ToByteArray("Row 5", 34));
            Game.WriteBytes(MetricsRow6Ptr, ToByteArray("Row 6", 34));
            Game.WriteBytes(MetricsRow7Ptr, ToByteArray("Row 7", 34));
            if(PerfMetricsType >= 2) {
                Game.WriteBytes(MetricsRow8Ptr, ToByteArray("Row 8", 34));
                Game.WriteBytes(MetricsRow9Ptr, ToByteArray("Row 9", 34));
            }
        }

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            if(!VersionIsSupported) return;
            if(MaxHzDP != null) MaxHzDP.DerefOffsets(Game, out MaxHzPtr);
            if(PerfMetricsDP != null) PerfMetricsDP.DerefOffsets(Game, out PerfMetricsPtr);
            if(GPUVendorDP != null) GPUVendorDP.DerefOffsets(Game, out GPUVendorPtr);
            if(GPUNameDP != null) GPUNameDP.DerefOffsets(Game, out GPUNamePtr);
            if(CPUDP != null && PerfMetricsType >= 1) CPUDP.DerefOffsets(Game, out CPUPtr);
        }

        public byte[] ToByteArray(string text, int length) {
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
        public string VersionString() {
            switch(ModuleSize) {
                case 507191296:
                case 515133440:
                case 510681088:
                    return "Release (1.0)";
                case 482037760:
                    return "May Patch Steam";
                case 546783232:
                    return "May Hotfix Steam";
                case 492113920:
                    return "1.1";
                case 490299392:
                    return "2.0";
                case 505344000:
                    return "2.1";
                case 475557888:
                    return "3.0";
                case 504107008:
                    return "3.1";
                case 478056448:
                    return "4.0";
                case 472821760:
                    return "4.1";
                case 475787264:
                    return "5.0";
                case 459132928:
                    return "5.1";
                case 481435648:
                    return "6.0";
                case 465915904:
                    return "6.1";
                case 464543744:
                    return "6.2";
                case 483786752:
                    return "6.3";
                case 494395392:
                    return "6.4";
                case 508350464:
                    return "6.66";
                case 478367744:
                    return "6.66 Rev 1";
                case 475570176:
                    return "6.66 Rev 1.1";
                case 510251008:
                    return "6.66 Rev 2";
                default:
                    return "Unknown (" + ModuleSize.ToString() + ")";
            }
        }

        private void SetUpPointers() {
            switch(ModuleSize) {
                case 507191296:
                case 515133440:
                case 510681088: // Release (1.0)
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B83BC0);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B83244);
                    CPUDP = null;
                    PerfMetricsDP = new DeepPointer("DoomEternalx64vk.exe", 0x3D11B20);
                    MaxHzDP = null;
                    VersionIsSupported = true;
                    PerfMetricsType = 0;
                    break;
                case 492113920: // 1.1
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B754D0);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B74B54);
                    CPUDP = null;
                    PerfMetricsDP = new DeepPointer("DOOMEternalx64vk.exe", 0x3D83420);
                    MaxHzDP = new DeepPointer("DOOMEternalx64vk.exe", 0x3D85760);
                    VersionIsSupported = true;
                    PerfMetricsType = 0;
                    break;
                case 490299392: // 2.0
                    VersionIsSupported = false;
                    PerfMetricsType = 0;
                    break;
                case 505344000: // 2.1
                    VersionIsSupported = false;
                    PerfMetricsType = 0;
                    break;
                case 475557888: // 3.0
                    VersionIsSupported = false;
                    PerfMetricsType = 0;
                    break;
                case 504107008: // 3.1
                    VersionIsSupported = false;
                    PerfMetricsType = 0;
                    break;
                case 478056448: // 4.0
                    VersionIsSupported = false;
                    PerfMetricsType = 1;
                    break;
                case 472821760: // 4.1
                    VersionIsSupported = false;
                    PerfMetricsType = 1;
                    break;
                case 475787264: // 5.0
                    VersionIsSupported = false;
                    PerfMetricsType = 1;
                    break;
                case 459132928: // 5.1
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5E44BE0);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5E44224);
                    CPUDP = new DeepPointer("DOOMEternalx64vk.exe", 0x489A1A0, 0x0);
                    PerfMetricsDP = new DeepPointer("DOOMEternalx64vk.exe", 0x4088BE0);
                    MaxHzDP = new DeepPointer("DOOMEternalx64vk.exe", 0x408AFA0);
                    VersionIsSupported = true;
                    PerfMetricsType = 1;
                    break;
                case 481435648: // 6.0
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 465915904: // 6.1
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 464543744: // 6.2
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 483786752: // 6.3
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 494395392: // 6.4
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x6226398);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x6225874);
                    CPUDP = new DeepPointer("DOOMEternalx64vk.exe", 0x499EFA0, 0x0);
                    PerfMetricsDP = new DeepPointer("DOOMEternalx64vk.exe", 0x42537B0);
                    MaxHzDP = null;
                    VersionIsSupported = true;
                    PerfMetricsType = 2;
                    break;
                case 508350464: // 6.66
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 478367744: // 6.66 Rev 1
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 475570176: // 6.66 Rev 1.1
                    VersionIsSupported = false;
                    PerfMetricsType = 2;
                    break;
                case 510251008: // 6.66 Rev 2
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x63FC378);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x63FB854);
                    CPUDP = new DeepPointer("DOOMEternalx64vk.exe", 0x4B7F018, 0x0);
                    PerfMetricsDP = new DeepPointer("DOOMEternalx64vk.exe", 0x44333D0);
                    MaxHzDP = new DeepPointer("DOOMEternalx64vk.exe", 0x4435790);
                    PerfMetricsType = 2;
                    break;
                default:
                    VersionIsSupported = false;
                    return;
            }
            SigScanRows();
        }

        private void SigScanRows() {
            SigScanTarget fpsTarget = new SigScanTarget(SigScanFPS);
            SigScanTarget dlssTarget = new SigScanTarget(SigScanDLSS);
            SignatureScanner scanner = new SignatureScanner(Game, Game.MainModule.BaseAddress, Game.MainModule.ModuleMemorySize);
            MetricsRow1Ptr = scanner.Scan(fpsTarget);
            if(MetricsRow1Ptr.ToInt64() != 0) {
                MetricsRow2Ptr = MetricsRow1Ptr + 0x8;
                MetricsRow3Ptr = MetricsRow1Ptr + 0x58;
                MetricsRow4Ptr = MetricsRow1Ptr + 0x70;
                MetricsRow5Ptr = MetricsRow1Ptr + 0x78;
                if(PerfMetricsType <= 1) {
                    MetricsRow6Ptr = MetricsRow1Ptr + 0x98;
                    MetricsRow7Ptr = MetricsRow1Ptr + 0xA8;
                }else if(PerfMetricsType == 2) {
                    MetricsRow6Ptr = scanner.Scan(dlssTarget);
                    if(MetricsRow6Ptr.ToInt64() != 0) {
                        MetricsRow7Ptr = MetricsRow6Ptr + 0x10;
                        MetricsRow8Ptr = MetricsRow6Ptr + 0x30;
                        MetricsRow9Ptr = MetricsRow6Ptr + 0x40;
                    }
                }
            }
        }
    }
}
