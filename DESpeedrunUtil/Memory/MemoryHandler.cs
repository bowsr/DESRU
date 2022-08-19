using System.Diagnostics;
using System.Text;

namespace DESpeedrunUtil.Memory {
    internal class MemoryHandler {
        // Unused for now. Will use if modifying FPS counter string is needed on unsupported versions.
        private readonly string SigScanFPS = "2569204650530000252E32666D7300004672616D65203A202575";
        private readonly string SigScanDLSS = "444C5353203A2025730000000000000056756C6B616E202573";

        DeepPointer MaxHzDP, PerfMetricsDP,
                    MetricsRow1DP, MetricsRow2DP, MetricsRow3DP, MetricsRow4DP,
                    MetricsRow5DP, MetricsRow6DP, MetricsRow7DP, MetricsRow8DP, MetricsRow9DP,
                    GPUVendorDP, GPUNameDP, CPUDP,
                    LevelNameDP;

        IntPtr      MaxHzPtr, PerfMetricsPtr,
                    MetricsRow1Ptr, MetricsRow2Ptr, MetricsRow3Ptr, MetricsRow4Ptr,
                    MetricsRow5Ptr, MetricsRow6Ptr, MetricsRow7Ptr, MetricsRow8Ptr, MetricsRow9Ptr,
                    GPUVendorPtr, GPUNamePtr, CPUPtr,
                    LevelNamePtr;

        Process Game;
        int ModuleSize;
        // PerfMetricsType -> Determines how the Medium Perf Metrics is displayed in game, as well as how it's set up in memory (for sig scans if necessary)
        // 0:  Baseline
        // 1:  + CPU Name
        // 2:  + RT/DLSS
        // 3+: Reserved if any other iterations are necessary
        int PerfMetricsType = 0;
        public bool VersionIsSupported { get; private set; }

        public MemoryHandler(Process game) {
            Game = game;
            ModuleSize = game.MainModule.ModuleMemorySize;
            
            //SetUpPointers();
        }

        /// <summary>
        /// Dereferences the <see cref="DeepPointer"/> addresses and offsets into an <see cref="IntPtr"/> that can be read from/written to.
        /// </summary>
        public void DerefPointers() {
            if(!VersionIsSupported) return;
            if(MaxHzDP != null) MaxHzDP.DerefOffsets(Game, out MaxHzPtr);
            if(PerfMetricsDP != null) PerfMetricsDP.DerefOffsets(Game, out PerfMetricsPtr);
            if(MetricsRow1DP != null) MetricsRow1DP.DerefOffsets(Game, out MetricsRow1Ptr);
            if(MetricsRow2DP != null) MetricsRow2DP.DerefOffsets(Game, out MetricsRow2Ptr);
            if(MetricsRow3DP != null) MetricsRow3DP.DerefOffsets(Game, out MetricsRow3Ptr);
            if(MetricsRow4DP != null) MetricsRow4DP.DerefOffsets(Game, out MetricsRow4Ptr);
            if(MetricsRow5DP != null) MetricsRow5DP.DerefOffsets(Game, out MetricsRow5Ptr);
            if(MetricsRow6DP != null) MetricsRow6DP.DerefOffsets(Game, out MetricsRow6Ptr);
            if(MetricsRow7DP != null) MetricsRow7DP.DerefOffsets(Game, out MetricsRow7Ptr);
            if(MetricsRow8DP != null && PerfMetricsType >= 2) MetricsRow8DP.DerefOffsets(Game, out MetricsRow8Ptr);
            if(MetricsRow9DP != null && PerfMetricsType >= 2) MetricsRow9DP.DerefOffsets(Game, out MetricsRow9Ptr);
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
                case 492113920: // 1.1
                    MetricsRow1DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338);         // FPS
                    MetricsRow2DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0x8);   // Frametime
                    MetricsRow3DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0x58);  // Resolution (scaling)
                    MetricsRow4DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0x70);  // HDR
                    MetricsRow5DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0x78);  // Vulkan
                    MetricsRow6DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0x98);  // VRAM
                    MetricsRow7DP = new DeepPointer("DOOMEternalx64vk.exe", 0x2608338 + 0xA8);  // Driver
                    MetricsRow8DP = null;
                    MetricsRow9DP = null;
                    GPUVendorDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B754D0);
                    GPUNameDP = new DeepPointer("DOOMEternalx64vk.exe", 0x5B74B54);
                    CPUDP = null;
                    PerfMetricsDP = new DeepPointer("DOOMEternalx64vk.exe", 0x3D83420);
                    MaxHzDP = new DeepPointer("DOOMEternalx64vk.exe", 0x3D85760);
                    VersionIsSupported = true;
                    PerfMetricsType = 0;
                    break;
                case 459132928: // 5.1
                    break;
                default:
                    VersionIsSupported = false;
                    return;
            }
        }
    }
}
