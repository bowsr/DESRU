namespace DESpeedrunUtil.Util {
    internal static class StringUtil {

        /// <summary>
        /// Separates a single hex representation of an array of bytes
        /// into an array of strings each representing a single byte.
        /// </summary>
        /// <param name="hex">Hex String representation of the array of bytes</param>
        /// <returns>A string array with each value representing a single byte</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static string[] SeparateHexIntoArray(string hex) {
            if(!IsValidHexString(hex)) throw new ArgumentException("\"" + hex + "\" is not a valid hex string.");

            string[] array = new string[hex.Length / 2];
            for(int i = 0; i < hex.Length / 2; i++)
                array[i] = hex[(i * 2)..((i + 1) * 2)];

            return array;
        }

        /// <summary>
        /// Converts a hex string into an array of bytes.
        /// </summary>
        /// <param name="hex">Hex String representation of the array of bytes</param>
        /// <returns>An array of bytes</returns>
        internal static byte[] ConvertHexStringToBytes(string hex) => SeparateHexIntoArray(hex).Select(b => Convert.ToByte(b, 16)).ToArray();

        private static bool IsValidHexString(string hex) => hex.Length % 2 == 0 && hex.ToLower().All(c => "0123456789abcdef".Contains(c));

    }
}
