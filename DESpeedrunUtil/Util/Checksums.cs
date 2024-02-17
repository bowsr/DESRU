using Newtonsoft.Json;
using Serilog;
using System.Security.Cryptography;

namespace DESpeedrunUtil.Util {
    internal static class Checksums {

        private static Dictionary<string, string> _vanillaHashes = new();
        private static Dictionary<string, string> _srmodHashes = new();

        internal static void InitHashDict(string data, string dict) {
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            switch(dict) {
                case "vanilla":
                    _vanillaHashes = deserialized;
                    break;
                case "srmod":
                    _srmodHashes = deserialized;
                    break;
                default:
                    return;
            }
        }

        internal static string VerifyGameDataFiles(string gameDir) {
            string baseDir = gameDir + "\\base\\";
            var isModded = false;
            try {
                foreach(var key in _vanillaHashes.Keys) {
                    if(!CompareFromFile(baseDir + key, _vanillaHashes[key])) {
                        isModded = true;
                        Log.Information("Game data files have been modified. Checking if file signatures match known mods...");
                        break;
                    }
                }
                if(!isModded) return "vanilla";
                foreach(var key in _srmodHashes.Keys) {
                    if(!CompareFromFile(baseDir + key, _srmodHashes[key])) {
                        Log.Information("Game data files do not match SRMod signatures.");
                        break;
                    }
                    Log.Information("Match detected: SRMod is currently installed.");
                    return "srmod";
                }

                //TODO - proteh's Horde Mode hashes
            } catch(KeyNotFoundException knfe) {
                Log.Error(knfe, "Failed to verify file hash.");
                return "failed";
            }
            Log.Warning("Could not identify installed mods.");
            Log.Warning("If you have a leaderboard legal mod (e.g. SpeedrunMod by DrLa), it must be the only mod installed, otherwise DESRU cannot verify its installation.");
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

    }
}
