namespace DESpeedrunUtil.Define {
    internal static class Constants {

        internal static readonly Color COLOR_FORM_BACK = Color.FromArgb(35, 35, 35);
        internal static readonly Color COLOR_PANEL_BACK = Color.FromArgb(45, 45, 45);
        internal static readonly Color COLOR_TEXT_BACK = Color.FromArgb(70, 70, 70);
        internal static readonly Color COLOR_TEXT_FORE = Color.FromArgb(230, 230, 230);

        internal const string WINDOW_TITLE = "DOOM ETERNAL SPEEDRUN UTILITY";
        internal const string PATH_PROFILE_DIR = @"\782330\remote\PROFILE";
        internal const string PATH_PROFILE_FILE = @"\profile.bin";
        internal const string PATH_FPSKEYS_JSON = @".\fpskeys.json";
        internal const string TEXTBOX_POSITION_TEXT = "Position\r\nx: {0:0.00}\r\ny: {1:0.00}\r\nz: {2:0.00}\r\nyaw: {3:0.0}\r\npitch: {4:0.0}";
        internal const string TEXTBOX_VELOCITY_TEXT = "Velocity\r\nx: {0:0.00}\r\ny: {1:0.00}\r\nz: {2:0.00}\r\nhorizontal: {3:0.0}\r\ntotal: {4:0.0}";
        internal const string TEXTBOX_ALT_TEXT_POS = "{0:0.00} {1:0.00} {2:0.00} | {3:0.0} {4:0.0}";

        internal const string BINDINGS_FILE = @".\macro\bindings.txt";

        internal const string DOWN_ONLY_FORMAT = "0x{0:X2}";
        internal const string UP_ONLY_FORMAT = "0x{0:X2} Up";
        internal const string DOWN_UP_FORMAT = "0x{0:X2} 0x{1:X2}";

        internal const string METRICS_FPS_TEXT = "%i FPS";
        internal const string METRICS_FRAMETIME_TEXT = "%.2fms";
        internal const string METRICS_RESOLUTION_TEXT = "%d x %d (%s)";
        internal const string METRICS_HDR_TEXT = "HDR: %s";
        internal const string METRICS_VULKAN_TEXT = "Vulkan %s";
        internal const string METRICS_VRAM_TEXT = "VRAM %llu MB%s";
        internal const string METRICS_DRIVER_TEXT = "Driver %s";
        internal const string METRICS_RAYTRACING_TEXT = "RT: %s";
        internal const string METRICS_DLSS_TEXT = "DLSS: %s";

        internal const string TRAINER_NOCHEATS_WARNING = "Speedometer is allowed without cheats";
        internal const string TRAINER_UNSUPPORTED_WARNING = "The Trainer is not supported on this version";

        internal const string BINDS_PATCH6_R = "084C8B07BA01";
        internal const string BINDS_PATCH6_U = "084C8B07BA00";
        internal const string BINDS_PATCH5_R = "084C8B03BA01";
        internal const string BINDS_PATCH5_U = "084C8B03BA00";
        internal const string BINDS_GLOBAL_R = "084C8B0FBA01";
        internal const string BINDS_GLOBAL_U = "084C8B0FBA00";
        internal const string CONSOLE_GLOBAL_R = "084C8B0EBA01";
        internal const string CONSOLE_GLOBAL_U = "084C8B0EBA00";
        internal const string LAUNCHPARAMS_GLOBAL_R = "CCCCCCCCCCCCCCCC40534883EC50488B842488000000488B";
        internal const string LAUNCHPARAMS_GLOBAL_U = "4183C91053EB0390EBF64883EC50488B842488000000488B";

        internal const int MAX_SCROLL_DELTA = 100; // Max milliseconds between scroll inputs
        internal const int WINDOW_HEIGHT_DEFAULT = 805;
        internal const int WINDOW_WIDTH_DEFAULT = 653;
        internal const int PANEL_HEIGHT_DEFAULT = 760;
        internal const int WINDOW_EXTRAHEIGHT_MOREKEYS = 271; // Amount to add/subtract when showing/hiding extra fps hotkeys
        internal const int PANEL_EXTRAHEIGHT_MOREKEYS = 269;
        internal const int WINDOW_EXTRAWIDTH = 12;

        internal const int OFFSET_ROW2 = 0x8;
        internal const int OFFSET_ROW3 = 0x58;
        internal const int OFFSET_ROW4 = 0x70;
        internal const int OFFSET_ROW5 = 0x78;
        internal const int OFFSET_ROW6 = 0x98;
        internal const int OFFSET_ROW7 = 0x10;
        internal const int OFFSET_ROW8 = 0x30;
        internal const int OFFSET_ROW9 = 0x40;

        internal static readonly Keys[] INVALID_KEYS = { Keys.Oemtilde, Keys.LButton, Keys.RButton };

        internal static readonly float[] DEFAULT_RES_SCALES = new float[32]
                { 1.0f, 0.99f, 0.98f, 0.97f, 0.96f, 0.95f, 0.94f, 0.93f,
                0.92f, 0.91f, 0.9f, 0.89f, 0.88f, 0.86f, 0.84f, 0.82f,
                0.8f, 0.78f, 0.76f, 0.74f, 0.72f, 0.7f, 0.68f, 0.66f,
                0.64f, 0.62f, 0.6f, 0.58f, 0.56f, 0.54f, 0.52f, 0.5f };
        internal static readonly float[] ONEPERCENT_RES_SCALES = new float[32]
                { 1.0f, 0.968f, 0.936f, 0.904f, 0.872f, 0.84f, 0.808f, 0.776f,
                0.744f, 0.712f, 0.68f, 0.648f, 0.616f, 0.584f, 0.552f, 0.52f,
                0.488f, 0.456f, 0.424f, 0.392f, 0.36f, 0.328f, 0.296f, 0.264f,
                0.232f, 0.2f, 0.168f, 0.136f, 0.104f, 0.072f, 0.04f, 0.01f };

        internal static readonly int[] VEL_OFFSETS_CURRENT = new int[4] { 0x1510, 0x5C0, 0x1D0, 0x3F40 };
        internal static readonly int[] VEL_OFFSETS_OLD = new int[4] { 0x1510, 0x598, 0x1D0, 0x3F40 };
    }
}
