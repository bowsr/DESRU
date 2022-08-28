using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace DESpeedrunUtil {
    internal static class Program {

        public const string APP_VERSION = "0.2";
        private static string _latestVersion;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if(UpdateCheck()) {
                UpdateDialog dialog = new(_latestVersion);
                System.Media.SystemSounds.Asterisk.Play();
                var result = dialog.ShowDialog();
                if(result == DialogResult.OK) {
                    Process.Start(new ProcessStartInfo("https://github.com/bowsr/DESRU/releases/latest") { UseShellExecute = true });
                    return;
                }else if(result == DialogResult.Cancel) {
                    return;
                }
            }
            Application.Run(new MainWindow());
        }

        private static bool UpdateCheck() {
            string json;
            try {
                WebClient client = new();
                client.Headers.Add("User-Agent", "Nothing");
                json = client.DownloadString("https://api.github.com/repos/bowsr/DESRU/releases");
            }catch(WebException we) {
                Debug.WriteLine(we.Message + "\n" + we.StackTrace);
                return false;
            }
            dynamic info = JsonConvert.DeserializeObject(json);
            if(info.Count > 0) {
                _latestVersion = info[0].tag_name;
                Version latest = new(_latestVersion);
                Version current = new(APP_VERSION);
                return latest.CompareTo(current) > 0;
            }else return false;
        }
    }
}