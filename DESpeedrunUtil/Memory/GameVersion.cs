namespace DESpeedrunUtil.Memory {
    internal class GameVersion {

        public static List<GameVersion> Collection = new();

        public string Name { get; init; }
        public int ModuleSize { get; init; }
        public string MD5 { get; init; }

        public GameVersion(int moduleSize, string name, string md5) {
            ModuleSize = moduleSize;
            Name = name;
            MD5 = md5;
            Collection.Add(this);
        }

        public static GameVersion GetVersionByName(string name) {
            var version = Collection.Find(v => v.Name.Equals(name));
            return version ?? new GameVersion(-1, "Unrecognized Version (" + name + ")", "n/a");
        }
        public static GameVersion GetVersionByChecksum(string md5) {
            var version = Collection.Find(v => v.MD5.Equals(md5));
            return version ?? new GameVersion(-1, "Unknown Version", md5);
        }
        public static GameVersion GetVersionByModuleSize(int moduleSize) {
            if(moduleSize == 507191296 || moduleSize == 515133440 || moduleSize == 510681088) return GetVersionByName("1.0 (Release)");
            var version = Collection.Find(v => v.ModuleSize == moduleSize);
            return version ?? new GameVersion(moduleSize, "Unknown Version", "n/a");
        }
        public static bool IsValidVersionString(string version) {
            return version switch {
                "1.0 (Release)" => true,
                "May Patch Steam" => true,
                "May Hotfix Steam" => true,
                "1.1" => true,
                "2.0" => true,
                "2.1" => true,
                "3.0" => true,
                "3.1" => true,
                "4.0" => true,
                "4.1" => true,
                "5.0" => true,
                "5.1" => true,
                "6.0" => true,
                "6.1" => true,
                "6.2" => true,
                "6.3" => true,
                "6.4" => true,
                "6.66" => true,
                "6.66 Rev 1" => true,
                "6.66 Rev 1.1" => true,
                "6.66 Rev 2" => true,
                "6.66 Rev 2 (Gamepass)" => true,
                "6.66 Rev 2.2" => true,
                "6.66 Rev 3" => true,
                _ => false,
            };
        }

    }
}
