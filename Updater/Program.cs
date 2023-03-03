using System.Diagnostics;
using System.Reflection;

namespace Updater {
    internal static class Program {

        private static readonly Version EMPTY_VERSION = new();

        public static Version NewVersion { get; private set; } = EMPTY_VERSION;

        /// <summary>
        /// Launch DESRU upon exit
        /// </summary>
        public static bool LaunchOnClose { get; set; } = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            ApplicationConfiguration.Initialize();

            if(args.Length > 0) {
                for(int i = 0; i < args.Length; i++) {
                    var s = args[i];
                    NewVersion = ValidateVersionString(s);
                    if(NewVersion != EMPTY_VERSION) break;
                }
                if(NewVersion != EMPTY_VERSION) {
                    Application.Run(new ProgressWindow());
                    if(LaunchOnClose) LaunchDESRU();
                }
            }
        }

        private static Version ValidateVersionString(string ver) {
            try {
                var v = new Version(ver);
                if(v.Major == 1 && v.Minor >= 0 && v.Build >= 0 && v.Revision == -1)
                    return v;
            } catch(Exception) { }
            return EMPTY_VERSION;
        }

        private static void LaunchDESRU() {
            var loc = Assembly.GetExecutingAssembly().Location;
            var psi = new ProcessStartInfo("cmd.exe", string.Format("/c \"{0}\\DESRU.exe\"", loc[..loc.LastIndexOf('\\')])) {
                CreateNoWindow = true
            };
            Process.Start(psi);
        }
    }
}