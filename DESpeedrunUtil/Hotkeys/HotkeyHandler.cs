using DESpeedrunUtil.Macro;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    internal class HotkeyHandler {

        private readonly MainWindow _parent;
        private globalKeyboardHook _hook;
        private bool _resScaleKeyEnabled = false;

        public bool Enabled { get; private set; }
        public Keys ResScaleHotkey { get; private set; } = Keys.None;

        public FPSHotkeyMap FPSHotkeys { get; init; }

        public HotkeyHandler(Keys res, string fpsJson, MainWindow parent) {
            _hook = new globalKeyboardHook();
            _parent = parent;
            ResScaleHotkey = res;
            FPSHotkeys = new(fpsJson);

            _hook.KeyDown += new KeyEventHandler(Hook_KeyDown);
            _hook.KeyUp += new KeyEventHandler(Hook_KeyUp);
        }

        public void EnableHotkeys() {
            if(Enabled) return;
            AddHotkeys();
            Enabled = true;
            Log.Verbose("Hotkeys enabled.");
        }
        public void DisableHotkeys() {
            if(!Enabled) return;
            _hook.HookedKeys.Clear();
            Enabled = false;
            Log.Verbose("Hotkeys disabled.");
        }
        public void RefreshKeys() {
            if(!Enabled) return;
            Log.Verbose("Refreshing hotkeys.");
            AddHotkeys();
        }
        public void ToggleResScaleKey(bool enabled) {
            _resScaleKeyEnabled = enabled;
            RefreshKeys();
        }

        private void ChangeResScaleHotkey(Keys key) => ResScaleHotkey = key;
        private void AddHotkeys() {
            _hook.HookedKeys.Clear();
            if(ResScaleHotkey != Keys.None && _resScaleKeyEnabled) _hook.HookedKeys.Add(ResScaleHotkey);
            foreach(FPSHotkeyMap.FPSKey fKey in FPSHotkeys.GetAllFPSKeys()) {
                if(fKey.Key != Keys.None && fKey.Limit != -1) _hook.HookedKeys.Add(fKey.Key);
            }
        }

        private void Hook_KeyDown(object sender, KeyEventArgs e) {
            int hk = -1;
            if(e.KeyCode == Keys.None || (e.Alt && e.KeyCode == Keys.F4)) {
                e.Handled = false;
                return;
            }
            if(e.KeyCode == ResScaleHotkey) {
                _parent.ToggleResScaling();
            }else if(FPSHotkeys.ContainsKey(e.KeyCode)) {
                _parent.ToggleFPSCap(FPSHotkeys.GetLimitFromKey(e.KeyCode));
            }
            e.Handled = true;
        }
        private void Hook_KeyUp(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.None) {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }

        #region STATIC METHODS
        /// <summary>
        /// Changes the desired hotkey specified by an int <paramref name="type"/>. Includes duplicate checking and resolving.
        /// </summary>
        /// <param name="key">New Keys value to assign</param>
        /// <param name="type">Which hotkey to change</param>
        /// <param name="macro"><see cref="FreescrollMacro"/> Instance</param>
        /// <param name="hotkeys"><see cref="HotkeyHandler"/> Instance</param>
        public static void ChangeHotkeys(Keys key, int type, FreescrollMacro macro, HotkeyHandler hotkeys) {
            // Duplicate check
            // If a dupe is found, sets dupe to the old key of the currently changing field
            //   type:  0-1  -> FreescrollMacro (type == 0) DownScrollKey if true, UpScrollKey if false
            //            2  -> Res Scaling Hotkey
            //            3+ -> FPSKey (id == type - 3)
            var fpstype = type - 3;
            var maxkeys = hotkeys.FPSHotkeys.Count() + 2;
            if(key != Keys.None) {
                if(type < 0 || type > maxkeys) return;
                Keys oldKey;
                if(type < 2) {
                    oldKey = macro.GetHotkey(type == 0);
                }else if(type == 2) {
                    oldKey = hotkeys.ResScaleHotkey;
                }else {
                    oldKey = hotkeys.FPSHotkeys.GetKeyFromID(fpstype);
                }
                for(int i = 0; i <= maxkeys; i++) {
                    if(i == type) continue;
                    if(i < 2) {
                        var down = (i == 0);
                        if(key == macro.GetHotkey(down)) {
                            Log.Verbose("Duplicate hotkey found. Swapping {Key0} with {Key1}", oldKey, key);
                            macro.ChangeHotkey(oldKey, down);
                            break;
                        }
                    }else if(i == 2) {
                        if(key == hotkeys.ResScaleHotkey) {
                            Log.Verbose("Duplicate hotkey found. Swapping {Key0} with {Key1}", oldKey, key);
                            hotkeys.ChangeResScaleHotkey(oldKey);
                            break;
                        }
                    }else {
                        if(key == hotkeys.FPSHotkeys.GetKeyFromID(i - 3)) {
                            Log.Verbose("Duplicate hotkey found. Swapping {Key0} with {Key1}", oldKey, key);
                            hotkeys.FPSHotkeys.ChangeKey(i - 3, oldKey);
                            break;
                        }
                    }
                }
            }

            if(type < 2) {
                macro.ChangeHotkey(key, type == 0);
                Log.Information("Macro key <{Type}> changed to {Key}", type == 4, key);
            }else if(type == 2) {
                hotkeys.ChangeResScaleHotkey(key);
                Log.Information("ResScale Hotkey changed to {Key}", type, key);
            }else {
                hotkeys.FPSHotkeys.ChangeKey(fpstype, key);
                Log.Information("FPSHotkey {FPSKey} changed to {Key}", fpstype, key);
            }
        }
        public static Keys ModKeySelector(int modifier) {
            return modifier switch {
                0 => (GetAsyncKeyState(Keys.RControlKey) & 0x01) == 1 ? Keys.RControlKey : Keys.LControlKey,
                1 => (GetAsyncKeyState(Keys.RShiftKey) & 0x01) == 1 ? Keys.RShiftKey : Keys.LShiftKey,
                2 => (GetAsyncKeyState(Keys.RMenu) & 0x01) == 1 ? Keys.RMenu : Keys.LMenu,
                _ => Keys.None,
            };
        }

        public static Keys ConvertMouseButton(MouseButtons button) {
            return button switch {
                MouseButtons.Left => Keys.LButton,
                MouseButtons.Right => Keys.RButton,
                MouseButtons.Middle => Keys.MButton,
                MouseButtons.XButton1 => Keys.XButton1,
                MouseButtons.XButton2 => Keys.XButton2,
                _ => Keys.None,
            };
        }

        public static string TranslateKeyNames(Keys key) {
            return key switch {
                Keys.LButton => "MOUSE1",
                Keys.RButton => "MOUSE2",
                Keys.MButton => "MOUSE3",
                Keys.XButton1 => "MOUSE4",
                Keys.XButton2 => "MOUSE5",
                Keys.LMenu => "Alt",
                Keys.RMenu => "RAlt",
                Keys.LShiftKey => "Shift",
                Keys.RShiftKey => "RShift",
                Keys.LControlKey => "Control",
                Keys.RControlKey => "RControl",
                Keys.LWin => "Win",
                Keys.RWin => "RWin",
                Keys.D1 => "1",
                Keys.D2 => "2",
                Keys.D3 => "3",
                Keys.D4 => "4",
                Keys.D5 => "5",
                Keys.D6 => "6",
                Keys.D7 => "7",
                Keys.D8 => "8",
                Keys.D9 => "9",
                Keys.D0 => "0",
                Keys.Oemplus => "=",
                Keys.OemMinus => "-",
                Keys.OemOpenBrackets => "[",
                Keys.Oem6 => "]",
                Keys.Oem5 => "\\",
                Keys.Oem1 => ";",
                Keys.Oemcomma => ",",
                Keys.OemPeriod => ".",
                Keys.Next => "PageDown",
                Keys.Back => "Backspace",
                Keys.None => "",
                _ => key.ToString()
            };
        }
        #endregion

        [DllImport("user32.dll")]
        public static extern ushort GetAsyncKeyState(Keys vKey);

        internal class FPSHotkeyMap {

            public const int DEFAULT_KEYS = 15;

            private Dictionary<int, FPSKey> _keys;

            public FPSHotkeyMap() {
                Initialize("");
            }
            public FPSHotkeyMap(string json) {
                Initialize(json);
            }
            private void Initialize(string json) {
                if(json != string.Empty) {
                    _keys = JsonConvert.DeserializeObject<Dictionary<int, FPSKey>>(json);
                }else {
                    _keys = new();
                    for(int i = 0; i < DEFAULT_KEYS; i++) {
                        _keys.Add(i, new FPSKey());
                    }
                }
            }

            public Dictionary<int, FPSKey>.ValueCollection GetAllFPSKeys() => _keys.Values;
            public int Count() => _keys.Keys.Count;

            public Keys GetKeyAndLimitFromID(int id, out int fps) {
                try {
                    var fpskey = _keys[id];
                    fps = fpskey.Limit;
                    return fpskey.Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    _keys.Add(id, new FPSKey());
                    fps = -1;
                    return Keys.None;
                }
            }
            public Keys GetKeyFromID(int id) {
                try {
                    return _keys[id].Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    _keys.Add(id, new FPSKey());
                    return Keys.None;
                }
            }
            public int GetLimitFromID(int id) {
                try {
                    return _keys[id].Limit;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    _keys.Add(id, new FPSKey());
                    return -1;
                }
            }

            public int GetLimitFromKey(Keys key) {
                foreach(FPSKey k in _keys.Values) {
                    if(k.Key == key) return k.Limit;
                }
                return -1;
            }

            public bool ContainsKey(Keys key) {
                foreach(FPSKey k in _keys.Values) {
                    if(k.Key == key) return true;
                }
                return false;
            }

            public int GetIDFromKey(Keys key) {
                var id = -1;
                foreach(int i in _keys.Keys) {
                    if(_keys[i].Key == key) {
                        id = i;
                        break;
                    }
                }
                return id;
            }

            public void ChangeLimit(int id, int limit) {
                if(id == -1) return;
                var key = GetKeyFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }
            public void ChangeKey(int id, Keys key) {
                if(id == -1) return;
                var limit = GetLimitFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }

            public string SerializeIntoJSON() => JsonConvert.SerializeObject(_keys, Formatting.Indented);

            internal readonly struct FPSKey {
                public Keys Key { get; init; }
                public int Limit { get; init; }

                public FPSKey(Keys k, int fps) {
                    Key = k;
                    Limit = fps;
                }
                public FPSKey() : this(Keys.None, -1) { }
            }
        }
    }
}
