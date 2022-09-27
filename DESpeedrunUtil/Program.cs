using Linearstar.Windows.RawInput;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace DESpeedrunUtil {
    internal static class Program {

        public const string APP_VERSION = "1.0.6";
        public static bool UpdateDetected = false;
        private static string _latestVersion;

        /// <summary>
        /// Form/Memory Timer intervals, in milliseconds
        /// </summary>
        public static int TimerInterval { get; private set; } = 16;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);

            ApplicationConfiguration.Initialize();
            
            var logCfg = new LoggerConfiguration().WriteTo.File(@".\logs\desru_.log", rollingInterval: RollingInterval.Day);
            bool verbose = false, timer = false;
            uint interval = 0;
            if(args.Length > 0) {
                bool t = false;
                for(int i = 0; i < args.Length; i++) {
                    var s = args[i];
                    if(s.Equals("-v")) {
                        verbose = true;
                        continue;
                    }
                    if(s.Equals("-t") && !timer) {
                        t = true;
                        timer = true;
                        continue;
                    }
                    if(t) {
                        if(!uint.TryParse(s, out interval)) interval = 0;
                        t = false;
                    }
                }
            }
            
            if(verbose) {
                Log.Logger = logCfg.MinimumLevel.Verbose().CreateLogger();
            }else {
                Log.Logger = logCfg.MinimumLevel.Debug().CreateLogger();
            }
            Log.Information("----- DESRU Session Started -----");
            if(Directory.Exists(@".\updateFiles"))
                Task.Run(async delegate {
                    await Task.Delay(2500);
                    Log.Information("Cleaning up previous update.");
                    foreach(string file in Directory.GetFiles(@".\updateFiles")) {
                        File.Move(file, file.Replace("updateFiles\\", ""), true);
                    }
                    Directory.Delete(@".\updateFiles");
                    Log.Information("Finished up previous update.");
                });
            if(verbose) Log.Verbose("Verbose Logging Enabled");
            if(timer) {
                if(interval == 0) {
                    Log.Warning("-t Parameter value must be a valid positive integer");
                }else {
                    if(interval < 16) {
                        interval = 16;
                        Log.Warning("-t Parameter value must not be lower than 16");
                    }else if(interval > 500) {
                        interval = 500;
                        Log.Warning("-t Parameter value must not exceed 500");
                    }
                    TimerInterval = (int) interval;
                    Log.Information("User specified a TimerInterval of {Interval}ms", interval);
                }
            }

            if(CheckForDuplicateProcesses()) {
                Log.Fatal("Found duplicate instances of DESRU! Please close all instances first or restart your system.");
                MessageBox.Show("Multiple instances of DESRU have been found.\n" +
                    "Please close every instance before re-opening DESRU or reboot your system.", "Duplicate DESRU Instances Detected");
                CloseLogger();
                return;
            }

            if(UpdateCheck()) {
                UpdateDetected = true;
                Log.Information("An update has been detected. New: {LatestVersion} | Installed: {AppVersion}", _latestVersion, APP_VERSION);
                UpdateDialog dialog = new(_latestVersion);
                System.Media.SystemSounds.Asterisk.Play();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    var loc = Assembly.GetExecutingAssembly().Location;
                    var psi = new ProcessStartInfo("cmd.exe", string.Format("/c \"{0}\\Updater.exe\" {1}", loc[..loc.LastIndexOf('\\')], _latestVersion)) {
                        CreateNoWindow = true
                    };
                    Process.Start(psi);

                    Log.Information("Initiated update for version {Version}", _latestVersion);
                    CloseLogger();
                    return;
                }else if(result == DialogResult.Cancel) {
                    CloseLogger();
                    return;
                }
                Log.Warning("User chose to ignore update.");
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
                Log.Fatal(e, "A fatal error has occured.");
            }finally {
                RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
                RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
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
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }

        private static bool UpdateCheck() {
            string json;
            try {
                WebClient client = new();
                client.Headers.Add("User-Agent", "Nothing");
                json = client.DownloadString("https://api.github.com/repos/bowsr/DESRU/releases");
            }catch(WebException we) {
                Log.Error(we, "An error occured when attempting to retrieve app releases." +
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
                Log.Warning("No releases were found when checking for updates.");
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