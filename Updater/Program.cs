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

            if(args.Length > 0) {
                for(int i = 0; i < args.Length; i++) {
                    var s = args[i];
                    try {
                        NewVersion = ValidateVersionString(s);
                        if(NewVersion != EMPTY_VERSION) break;
                    }catch(Exception) { }
                }
            }
            if(NewVersion != EMPTY_VERSION) {
                Application.Run(new ProgressWindow());
            }
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