using DESpeedrunUtil.Memory;
using Linearstar.Windows.RawInput;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace DESpeedrunUtil {
    internal static class Program {

        public const string APP_VERSION = "1.6.7";
        public static bool UpdateDetected = false;
        private static string _latestVersion, _changelog;
        private static bool _checkFailed = false;

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
#if DEBUG
            logCfg.WriteTo.Debug();
#endif
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
            } else {
                Log.Logger = logCfg.MinimumLevel.Debug().CreateLogger();
            }
            Log.Information("----- DESRU Session Started -----");
            if(Directory.Exists(@".\updateFiles"))
                Task.Run(async delegate {
                    await Task.Delay(1500);
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
                } else {
                    if(interval < 16) {
                        interval = 16;
                        Log.Warning("-t Parameter value must not be lower than 16");
                    } else if(interval > 500) {
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

            var update = UpdateCheck();
            if(!update && _checkFailed && Properties.Settings.Default.DetectedUpdate != "0.0") {
                _latestVersion = Properties.Settings.Default.DetectedUpdate;
                update = new Version(_latestVersion).CompareTo(new Version(APP_VERSION)) > 0;
            }
            if(update) {
                UpdateDetected = true;
                Log.Information("An update has been detected. New: {LatestVersion} | Installed: {AppVersion}", _latestVersion, APP_VERSION);
                UpdateDialog dialog = new(_latestVersion, _changelog[.._changelog.LastIndexOf("## Installation")]);
                System.Media.SystemSounds.Asterisk.Play();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    bool brokenUpdater = !File.Exists(@".\Updater.exe") || !File.Exists(@".\Updater.dll") ||
                        !File.Exists(@".\Updater.deps.json") || !File.Exists(@".\Updater.runtimeconfig.json");
                    if(brokenUpdater) {
                        Log.Error("One or more Updater files are missing. Aborting program.");
                        var msg = MessageBox.Show("Cannot launch the Updater. One or more files are missing.\n" +
                            "You will need to manually update DESRU.", "Broken Updater Installation");
                        if(msg == DialogResult.OK) {
                            Process.Start(new ProcessStartInfo("https://github.com/bowsr/DESRU/releases/latest") { UseShellExecute = true });
                            Log.Information("Opened new update's release page.");
                        }
                        Properties.Settings.Default.DetectedUpdate = _latestVersion;
                        Properties.Settings.Default.Save();
                        CloseLogger();
                        return;
                    }
                    var loc = Assembly.GetExecutingAssembly().Location;
                    var psi = new ProcessStartInfo("cmd.exe", string.Format("/c \"{0}\\Updater.exe\" {1}", loc[..loc.LastIndexOf('\\')], _latestVersion)) {
                        CreateNoWindow = true
                    };
                    Process.Start(psi);

                    Log.Information("Initiated update for version {Version}", _latestVersion);
                    CloseLogger();
                    return;
                } else if(result == DialogResult.Cancel) {
                    Properties.Settings.Default.DetectedUpdate = _latestVersion;
                    Properties.Settings.Default.Save();
                    CloseLogger();
                    return;
                }
                Log.Warning("User chose to ignore update.");
                Properties.Settings.Default.DetectedUpdate = _latestVersion;
                Properties.Settings.Default.Save();
            }
            if(!UpdateDetected) {
                Properties.Settings.Default.DetectedUpdate = "0.0";
                Properties.Settings.Default.Save();
            }
            if(!FileCheck()) {
                MessageBox.Show("Your DESRU installation is broken.\n" +
                    "Please reinstall DESRU, and make sure every file is extracted from the ZIP archive.", "Broken DESRU Installation");
                CloseLogger();
                return;
            }

            if(File.Exists(@".\offsets.json")) File.Delete(@".\offsets.json");
            GameVersion.Collection = JsonConvert.DeserializeObject<List<GameVersion>>(System.Text.Encoding.UTF8.GetString(Properties.Resources.gameVersions));

            try {
                Application.Run(new MainWindow());
            } catch(Exception e) {
                Log.Fatal(e, "A fatal error has occured.");
            } finally {
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
                using WebClient client = new();
                client.Headers.Add("User-Agent", "Nothing");
                json = client.DownloadString("https://api.github.com/repos/bowsr/DESRU/releases");
            } catch(WebException we) {
                Log.Error(we, "An error occured when attempting to retrieve app releases." +
                    "Make sure this program has the ability to connect to the internet.");
                _checkFailed = true;
                return false;
            }
            dynamic info = JsonConvert.DeserializeObject(json);
            if(info.Count > 0) {
                _latestVersion = info[0].tag_name;
                _changelog = info[0].body;
                Version latest = new(_latestVersion);
                Version current = new(APP_VERSION);
                return latest.CompareTo(current) > 0;
            } else {
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

        /*private static void InitializeGameVersions() {
            _ = new GameVersion(510681088, "1.0 (Release)", "7ea73e0ee1a2066dc43502930ededced");
            _ = new GameVersion(482037760, "May Patch Steam", "f7cc91087e48408ee5703affcc356bee");
            _ = new GameVersion(546783232, "May Hotfix Steam", "401bc43fc0d3719285cfb6ccee6b3393");
            _ = new GameVersion(492113920, "1.1", "70426fc580f2c064cb618317fd1a9d5e");
            _ = new GameVersion(490299392, "2.0", "53b98af0cb0e3ffe9019ac62bad83703");
            _ = new GameVersion(505344000, "2.1", "4fd80156710af0b0bd5d8a648e209f9b");
            _ = new GameVersion(475557888, "3.0", "c03c73dbd60683e3bf157a362e5da16e");
            _ = new GameVersion(504107008, "3.1", "3c770f56d3c148b648ee1d294e288e1c");
            _ = new GameVersion(478056448, "4.0", "88da87233f2b02fb5086de9296ea2440");
            _ = new GameVersion(472821760, "4.1", "c2b429b2eb398f836dd10d22944b9c76");
            _ = new GameVersion(475787264, "5.0", "96556f8b0dfc56111090a6b663969b86");
            _ = new GameVersion(459132928, "5.1", "6ae01a116b0683443002f93b7f32ba73");
            _ = new GameVersion(481435648, "6.0", "56961731a6d153b133842856ea36edea");
            _ = new GameVersion(465915904, "6.1", "7ce1b2029a94b7bfe1ec4bd76b9cf6a1");
            _ = new GameVersion(464543744, "6.2", "dd75e40d9dba5a95708a0e4f625a96af");
            _ = new GameVersion(483786752, "6.3", "9e9013ae3481e963a5fc3a049a8a1542");
            _ = new GameVersion(494395392, "6.4", "8829459d8270a69e78b1797eb8785227");
            _ = new GameVersion(508350464, "6.66", "8e4462adcc44dc89287f687939a96af3");
            _ = new GameVersion(478367744, "6.66 Rev 1", "c7b3d11cd57e8312cf24c945fc9647ce");
            _ = new GameVersion(475570176, "6.66 Rev 1.1", "eeb62f46eddbab27809b82d94f9f4927");
            _ = new GameVersion(510251008, "6.66 Rev 2", "b2d372b0a193bd6d7712630850d4bad3");
            _ = new GameVersion(445820928, "6.66 Rev 2 (Gamepass)", "c0d3e753d0e53ccd9b47e1617b8de462");
        }*/
    }
}