using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace DESpeedrunUtil {
    internal static class Program {

        public const string APP_VERSION = "0.2";
        public static bool UpdateDetected = false;
        private static string _latestVersion;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.Console().WriteTo.File(@".\logs\desru.log", rollingInterval: RollingInterval.Day).CreateLogger();

            if(UpdateCheck()) {
                UpdateDetected = true;
                Log.Logger.Information("An update has been detected. New: {LatestVersion} | Installed: {AppVersion}", _latestVersion, APP_VERSION);
                UpdateDialog dialog = new(_latestVersion);
                System.Media.SystemSounds.Asterisk.Play();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    Process.Start(new ProcessStartInfo("https://github.com/bowsr/DESRU/releases/latest") { UseShellExecute = true });
                    return;
                }else if(result == DialogResult.Cancel) {
                    return;
                }
                Log.Logger.Warning("User chose to ignore update.");
            }
            try {
                Application.Run(new MainWindow());
            }catch(Exception e) {
                Log.Logger.Fatal(e, "A fatal error has occured.");
            }finally {
                Log.CloseAndFlush();
            }
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
            } else {
                Log.Logger.Warning("No releases were found when checking for updates.");
                return false;
            }
        }
    }
}