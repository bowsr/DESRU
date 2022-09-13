using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace DESpeedrunUtil {
    internal static class Program {

        public const string APP_VERSION = "0.7.2";
        public static bool UpdateDetected = false;
        private static string _latestVersion;

        /// <summary>
        /// MainWindow & MemoryHandler timer intervals, in milliseconds
        /// </summary>
        public const int TIMER_INTERVAL = 16;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);

            ApplicationConfiguration.Initialize();
            
            var logCfg = new LoggerConfiguration().WriteTo.Console().WriteTo.File(@".\logs\desru_.log", rollingInterval: RollingInterval.Day);
            bool verbose = false;
            if(args.Length > 0) {
                foreach(string s in args) {
                    if(s.Equals("-v")) {
                        verbose = true;
                        break;
                    }
                }
            }
            if(verbose) {
                Log.Logger = logCfg.MinimumLevel.Verbose().CreateLogger();
            }else {
                Log.Logger = logCfg.MinimumLevel.Debug().CreateLogger();
            }
            Log.Information("----- DESRU Session Started -----");

            if(CheckForDuplicateProcesses()) {
                Log.Fatal("Found duplicate instances of DESRU! Please close all instances first or restart your system.");
                MessageBox.Show("Multiple instances of DESRU have been found.\n" +
                    "Please close every instance before re-opening DESRU or reboot your system.", "Duplicate DESRU Instances Detected");
                CloseLogger();
                return;
            }

            if(UpdateCheck()) {
                UpdateDetected = true;
                Log.Logger.Information("An update has been detected. New: {LatestVersion} | Installed: {AppVersion}", _latestVersion, APP_VERSION);
                UpdateDialog dialog = new(_latestVersion);
                System.Media.SystemSounds.Asterisk.Play();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    Process.Start(new ProcessStartInfo("https://github.com/bowsr/DESRU/releases/latest") { UseShellExecute = true });
                    Log.Information("Opened new update's release page.");
                    CloseLogger();
                    return;
                }else if(result == DialogResult.Cancel) {
                    CloseLogger();
                    return;
                }
                Log.Logger.Warning("User chose to ignore update.");
            }
            if(!FileCheck()) {
                MessageBox.Show("Your DESRU installation is broken.\n" +
                    "Please reinstall DESRU, and make sure every file is extracted from the ZIP archive.", "Broken DESRU Installation");
                CloseLogger();
                return;
            }
            try {
                Application.Run(new MainWindow());
            }catch(Exception e) {
                Log.Logger.Fatal(e, "A fatal error has occured.");
            }finally {
                CloseLogger();
            }
        }

        static void CloseLogger() {
            Log.Information("-----  DESRU Session Ended  -----");
            Log.CloseAndFlush();
        }

        static void CrashHandler(object sender, UnhandledExceptionEventArgs args) {
            Log.Fatal((Exception) args.ExceptionObject, "Unhandled exception encountered! runtime: {Terminating}", args.IsTerminating);
            Log.CloseAndFlush();
        }

        private static bool UpdateCheck() {
            string json;
            try {
                WebClient client = new();
                client.Headers.Add("User-Agent", "Nothing");
                json = client.DownloadString("https://api.github.com/repos/bowsr/DESRU/releases");
            }catch(WebException we) {
                Log.Logger.Error(we, "An error occured when attempting to retrieve app releases." +
                    "Make sure this program has the ability to connect to the internet.");
                return false;
            }
            dynamic info = JsonConvert.DeserializeObject(json);
            if(info.Count > 0) {
                _latestVersion = info[0].tag_name;
                Version latest = new(_latestVersion);
                Version current = new(APP_VERSION);
                return latest.CompareTo(current) > 0;
            }else {
                Log.Logger.Warning("No releases were found when checking for updates.");
                return false;
            }
        }

        private static bool FileCheck() {
            if(!File.Exists(@".\macro\DOOMEternalMacro.exe")) {
                Log.Error("Macro does not exist. Aborting program.");
                return false;
            }
            if(!File.Exists(@".\meath00k\XINPUT1_3.dll")) {
                Log.Error("meath00k does not exist. Aborting program.");
                return false;
            }
            if(!File.Exists(@".\fonts\EternalBattleBold.ttf") || !File.Exists(@".\fonts\EternalLogo.ttf") || 
                !File.Exists(@".\fonts\EternalUi2Bold.ttf") || !File.Exists(@".\fonts\EternalUi2Regular.ttf")) {
                Log.Error("One or more fonts are missing. Aborting program.");
                return false;
            }
            return true;
        }

        private static bool CheckForDuplicateProcesses() {
            List<Process> procList = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.Contains("DESRU"));
            if(procList.Count > 1) return true;
            return false;
        }
    }
}