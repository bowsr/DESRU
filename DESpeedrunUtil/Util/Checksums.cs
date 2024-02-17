using Newtonsoft.Json;
using Serilog;
using System.Security.Cryptography;

namespace DESpeedrunUtil.Util {
    internal static class Checksums {

        private static Dictionary<string, string> _vanillaChecksums = new();
        private static Dictionary<string, string> _srmodChecksums = new();

        internal static void InitChecksumDict(string data, string dict) {
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            switch(dict) {
                case "vanilla":
                    _vanillaChecksums = deserialized;
                    break;
                case "srmod":
                    _srmodChecksums = deserialized;
                    break;
                default:
                    return;
            }
        }

        internal static string VerifyGameDataFiles(string gameDir) {
            if(gameDir.ToLower().Contains(@"\windowsapps\")) return "UWP";
            string baseDir = gameDir + "\\base\\";
            var isModded = false;
            try {
                Log.Information("Checking if game resource file checksums match any known mods...");
                // Vanilla
                if(VerifyChecksums(baseDir, ref _vanillaChecksums)) {
                    Log.Information("Game resource files have not been modified. Likely that no mods were installed.");
                    return "vanilla";
                }
                // SRMod
                if(VerifyChecksums(baseDir, ref _srmodChecksums)) {
                    Log.Information("Known mod match found: Speedrun Mod by DrLa");
                    return "srmod";
                }

                //TODO - proteh's Horde Mode hashes
            } catch(KeyNotFoundException knfe) {
                Log.Error(knfe, "Failed to verify checksum.");
                return "failed";
            }
            Log.Warning("Could not identify installed mods.");
            Log.Warning("If you have a leaderboard legal mod installed (e.g. SpeedrunMod by DrLa), it must be the only mod, otherwise DESRU cannot verify its installation.");
            return "unknown";
        }

        internal static string GetMD5ChecksumFromFile(string filePath) {
            if(filePath.ToLower().Contains(@"\windowsapps\")) return "UWP";
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "").ToLower();
        }

        internal static bool Compare(string checksum0, string checksum1) => checksum0.Replace("-", "").ToLower().Equals(checksum1.Replace("-", "").ToLower());
        internal static bool CompareFromFile(string filePath, string checksum) => GetMD5ChecksumFromFile(filePath).Equals(checksum.Replace("-", "").ToLower());

        private static bool VerifyChecksums(string dir, ref Dictionary<string, string> checksums) {
            foreach(var key in checksums.Keys)
                if(!CompareFromFile(dir + key, checksums[key])) 
                    return false;
            return true;
        }

    }
}
