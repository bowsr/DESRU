namespace DESpeedrunUtil.Define {
    internal static class Structs {

        internal struct ScrollPattern {
            public int ScrollCount { get; set; }
            public long DeltaTotal { get; set; }

            public long Average() => (ScrollCount > 1) ? DeltaTotal / (ScrollCount - 1) : 0;
            public void Reset() {
                ScrollCount = 0;
                DeltaTotal = 0;
            }
        }

        /// <summary>
        /// Struct that stores both the <see cref="Keys"/> and Limit together
        /// </summary>
        internal readonly struct FPSKey {
            public Keys Key { get; init; }
            public int Limit { get; init; }

            public FPSKey(Keys k, int fps) {
                Key = k;
                Limit = fps;
            }
            public FPSKey() : this(Keys.None, -1) { }
        }

        internal readonly struct KnownOffsets {
            public string Version { get; init; }

            public int Row1 { get; init; }
            public int Row6 { get; init; }

            public int GPUVendor { get; init; }
            public int GPUName { get; init; }
            public int CPU { get; init; }

            public int Metrics { get; init; }
            public int MaxHz { get; init; }
            public int MinRes { get; init; }
            public int DynamicRes { get; init; }
            public int RaiseMS { get; init; }
            public int DropMS { get; init; }

            public int ResScales { get; init; }

            public KnownOffsets(string v, int r1, int r6,
                                int gpuv, int gpu, int cpu,
                                int perf, int hz, int min, int dyn, int msR, int msD, int res) {
                Version = v;
                Row1 = r1;
                Row6 = r6;
                GPUVendor = gpuv;
                GPUName = gpu;
                CPU = cpu;
                Metrics = perf;
                MaxHz = hz;
                MinRes = min;
                DynamicRes = dyn;
                RaiseMS = msR;
                DropMS = msD;
                ResScales = res;
            }
        }
    }
}
