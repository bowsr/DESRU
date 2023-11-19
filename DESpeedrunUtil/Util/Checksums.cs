using Serilog;
using System.Security.Cryptography;

namespace DESpeedrunUtil.Util {
    internal static class Checksums {

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
