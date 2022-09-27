using Serilog;

namespace Updater {
    internal static class Program {

        private static readonly Version EMPTY_VERSION = new();

        public static Version NewVersion { get; private set; } = EMPTY_VERSION;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            ApplicationConfiguration.Initialize();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(@".\logs\updater\updater_.log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Verbose()
                .CreateLogger();

            if(args.Length > 0) {
                for(int i = 0; i < args.Length; i++) {
                    var s = args[i];
                    try {
                        NewVersion = ValidateVersionString(s);
                        if(NewVersion != EMPTY_VERSION) break;
                    }catch(Exception e) {
                        Log.Error(e, "\"{String}\" is not a valid version string.", s);
                    }
                }
            }
            if(NewVersion != EMPTY_VERSION) {
                Application.Run(new ProgressWindow());
            }else {
                Log.Error("No valid version was passed. Aborting.");
            }
            Log.CloseAndFlush();
        }

        private static Version ValidateVersionString(string ver) {
            try {
                var v = new Version(ver);
                if(v.Major == 1 && v.Minor >= 0 && v.Build >= 0 && v.Revision == -1)
                    return v;
            }catch(Exception) {
                throw;
            }
            return EMPTY_VERSION;
        }
    }
}