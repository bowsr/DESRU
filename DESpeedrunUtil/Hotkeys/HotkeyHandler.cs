using DESpeedrunUtil.Macro;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    internal class HotkeyHandler {

        private readonly MainWindow _parent;
        private GlobalInputHook _hook;
        private bool _resScaleKeyEnabled = false;

        public bool Enabled { get; private set; }
        public Keys ResScaleHotkey { get; private set; } = Keys.None;

        public List<Keys> HookedHotkeys { get; private set; }

        /// <summary>
        /// Data structure for storing FPS hotkeys
        /// </summary>
        public FPSHotkeyMap FPSHotkeys { get; init; }

        public HotkeyHandler(Keys res, string fpsJson, MainWindow parent) {
            ResScaleHotkey = res;
            FPSHotkeys = new(fpsJson);
            _parent = parent;

            if(!Program.UseRawInput) {
                _hook = new GlobalInputHook();
                _hook.KeyDown += new KeyEventHandler(Hook_KeyDown);
                _hook.KeyUp += new KeyEventHandler(Hook_KeyUp);
                _hook.MouseDown += new MouseEventHandler(Hook_MouseDown);
                _hook.MouseUp += new MouseEventHandler(Hook_MouseUp);
                _hook.MouseScroll += new EventHandler(Hook_MouseScroll);
            } else {
                _parent.RIKeyDown += new KeyEventHandler(Hook_KeyDown);
                _parent.RIKeyUp += new KeyEventHandler(Hook_KeyUp);
                _parent.RIMouseDown += new MouseEventHandler(Hook_MouseDown);
                _parent.RIMouseUp += new MouseEventHandler(Hook_MouseUp);
                _parent.RIMouseScroll += new EventHandler<MouseWheelEventArgs>(Hook_MouseScroll);
            }

            HookedHotkeys = new();
            if(ResScaleHotkey != Keys.None) HookedHotkeys.Add(ResScaleHotkey);
            foreach(FPSHotkeyMap.FPSKey fkey in FPSHotkeys.GetAllFPSKeys()) {
                if(fkey.Key != Keys.None) HookedHotkeys.Add(fkey.Key);
            }
            Log.Information("Initialized HotkeyHandler");
        }

        public void HookMouse() => _hook.HookMouse();
        public void UnhookMouse() => _hook.UnhookMouse();

        /// <summary>
        /// Enables global hotkeys
        /// </summary>
        public void EnableHotkeys() {
            if(Enabled) return;
            AddHotkeys();
            Enabled = true;
            Log.Verbose("Hotkeys enabled");
        }
        /// <summary>
        /// Disables global hotkeys
        /// </summary>
        public void DisableHotkeys() {
            if(!Enabled) return;
            if(!Program.UseRawInput) _hook.HookedKeys.Clear();
            Enabled = false;
            Log.Verbose("Hotkeys disabled");
        }
        /// <summary>
        /// Refreshes the currently hooked <see cref="Keys"/> if hotkeys are enabled
        /// </summary>
        public void RefreshKeys() {
            if(!Enabled) return;
            Log.Verbose("Refreshing hotkeys");
            AddHotkeys();
        }

        /// <summary>
        /// Toggles the Resolution Scaling hotkey
        /// </summary>
        /// <param name="enabled"><see langword="true"/> if enabled</param>
        public void ToggleResScaleKey(bool enabled) {
            _resScaleKeyEnabled = enabled;
            RefreshKeys();
        }

        private void ChangeResScaleHotkey(Keys key) => ResScaleHotkey = key;
        private void AddHotkeys() {
            HookedHotkeys.Clear();
            if(ResScaleHotkey != Keys.None) HookedHotkeys.Add(ResScaleHotkey);
            foreach(FPSHotkeyMap.FPSKey fkey in FPSHotkeys.GetAllFPSKeys()) {
                if(fkey.Key != Keys.None) HookedHotkeys.Add(fkey.Key);
            }

            if(Program.UseRawInput) return;

            _hook.HookedKeys.Clear();
            if(ResScaleHotkey != Keys.None && _resScaleKeyEnabled) _hook.HookedKeys.Add(ResScaleHotkey);
            foreach(FPSHotkeyMap.FPSKey fKey in FPSHotkeys.GetAllFPSKeys()) {
                if(fKey.Key != Keys.None && fKey.Limit != -1) _hook.HookedKeys.Add(fKey.Key);
            }
        }

        private void Hook_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.None || (e.Alt && e.KeyCode == Keys.F4) || !Enabled) {
                e.Handled = false;
                return;
            }
            if(e.KeyCode == ResScaleHotkey && _resScaleKeyEnabled) {
                _parent.ToggleResScaling();
            }else if(FPSHotkeys.ContainsKey(e.KeyCode)) {
                _parent.ToggleFPSCap(FPSHotkeys.GetLimitFromKey(e.KeyCode));
            }
            e.Handled = true;
        }
        private void Hook_KeyUp(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.None || !Enabled) {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
        private void Hook_MouseDown(object sender, MouseEventArgs e) {
            var key = ConvertMouseButton(e.Button);
            if(key == Keys.None || !Enabled) return;
            if(key == ResScaleHotkey && _resScaleKeyEnabled) {
                _parent.ToggleResScaling();
            }else if(FPSHotkeys.ContainsKey(key)) {
                _parent.ToggleFPSCap(FPSHotkeys.GetLimitFromKey(key));
            }
        }
        private void Hook_MouseUp(object sender, MouseEventArgs e) {
            if(ConvertMouseButton(e.Button) == Keys.None || !Enabled) return;
        }
        private void Hook_MouseScroll(object sender, EventArgs e) => _parent.TrackMouseWheel(((MouseWheelEventArgs) e).Direction);

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
                            Log.Information("Duplicate of Macro " + ((down) ? "Down" : "Up") + "scroll Key. Swapping {Key0} with {Key1}", oldKey, key);
                            macro.ChangeHotkey(oldKey, down);
                            break;
                        }
                    }else if(i == 2) {
                        if(key == hotkeys.ResScaleHotkey) {
                            Log.Information("Duplicate of ResScaleHotkey. Swapping {Key0} with {Key1}", oldKey, key);
                            hotkeys.ChangeResScaleHotkey(oldKey);
                            break;
                        }
                    }else {
                        if(key == hotkeys.FPSHotkeys.GetKeyFromID(i - 3)) {
                            Log.Information("Duplicate of FPSHotkey " + (i - 3) +  ". Swapping {Key0} with {Key1}", oldKey, key);
                            hotkeys.FPSHotkeys.ChangeKey(i - 3, oldKey);
                            break;
                        }
                    }
                }
            }

            if(type < 2) {
                macro.ChangeHotkey(key, type == 0);
                Log.Information("Macro " + ((type == 0) ? "Down" : "Up") + "scroll Key changed to {Key}", key);
            }else if(type == 2) {
                hotkeys.ChangeResScaleHotkey(key);
                Log.Information("ResScale Hotkey changed to {Key}", type, key);
            }else {
                hotkeys.FPSHotkeys.ChangeKey(fpstype, key);
                Log.Information("FPSHotkey {FPSKey} changed to {Key}", fpstype, key);
            }

            hotkeys.RefreshKeys();
        }

        /// <summary>
        /// Gets a key representing which modifier key was pressed
        /// </summary>
        /// <param name="modifier">Which modifier to check</param>
        /// <returns>Specific <see cref="Keys"/> value of the mod key pressed</returns>
        public static Keys ModKeySelector(int modifier) {
            return modifier switch {
                0 => (GetAsyncKeyState(Keys.RControlKey) & 0x01) == 1 ? Keys.RControlKey : Keys.LControlKey,
                1 => (GetAsyncKeyState(Keys.RShiftKey) & 0x01) == 1 ? Keys.RShiftKey : Keys.LShiftKey,
                2 => (GetAsyncKeyState(Keys.RMenu) & 0x01) == 1 ? Keys.RMenu : Keys.LMenu,
                _ => Keys.None,
            };
        }

        /// <summary>
        /// Converts <see cref="MouseButtons"/> into their respective <see cref="Keys"/>
        /// </summary>
        /// <param name="button">The button to convert</param>
        /// <returns><see cref="Keys"/> representation of the button</returns>
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

        /// <summary>
        /// Translates <see cref="Keys"/> names into more widely known representations
        /// </summary>
        /// <param name="key">Key to translate</param>
        /// <returns>Name of key</returns>
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

        /// <summary>
        /// Dictionary wrapper for storing FPS Hotkeys and their respective limits
        /// </summary>
        internal class FPSHotkeyMap {

            /// <summary>
            /// Default number of hotkeys to generate if there are none stored in JSON
            /// </summary>
            public const int DEFAULT_KEYS = 15;

            private Dictionary<int, FPSKey> _keys;

            /// <summary>
            /// Initializes data structure with default values
            /// </summary>
            public FPSHotkeyMap() {
                Initialize("");
            }
            /// <summary>
            /// Initializes data structure with stored values in JSON
            /// </summary>
            /// <param name="json">JSON string</param>
            public FPSHotkeyMap(string json) {
                Initialize(json);
            }
            private void Initialize(string json) {
                if(json != string.Empty) {
                    _keys = JsonConvert.DeserializeObject<Dictionary<int, FPSKey>>(json);
                    for(int i = 0; i < _keys.Keys.Count; i++) {
                        var key = GetKeyAndLimitFromID(i, out int lim);
                        if(lim > 250) {
                            lim = 250;
                            _keys[i] = new FPSKey(key, lim);
                        }else if(lim <= 0) {
                            lim = -1;
                            _keys[i] = new FPSKey(key, lim);
                        }else continue;
                    }
                }else {
                    Log.Information("FPS Hotkey settings did not exist. Generating default list.");
                    _keys = new();
                    for(int i = 0; i < DEFAULT_KEYS; i++) {
                        _keys.Add(i, new FPSKey());
                    }
                }
            }

            /// <summary>
            /// Fetches a collection of all <see cref="FPSKey"/> values
            /// </summary>
            /// <returns>Collection of <see cref="FPSKey"/></returns>
            public Dictionary<int, FPSKey>.ValueCollection GetAllFPSKeys() => _keys.Values;
            /// <summary>
            /// Fetches the number of hotkey entries
            /// </summary>
            /// <returns>Count of IDs</returns>
            public int Count() => _keys.Keys.Count;

            /// <summary>
            /// Fetches both the <see cref="Keys"/> and Limit of the specified hotkey ID
            /// </summary>
            /// <param name="id">ID of hotkey</param>
            /// <param name="fps"><see langword="int"/> reference of limit</param>
            /// <returns>The key of the hotkey</returns>
            public Keys GetKeyAndLimitFromID(int id, out int fps) {
                try {
                    var fpskey = _keys[id];
                    fps = fpskey.Limit;
                    return fpskey.Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    if(id >= 0) _keys.Add(id, new FPSKey());
                    fps = -1;
                    return Keys.None;
                }
            }
            /// <summary>
            /// Fetches the <see cref="Keys"/> of the specified hotkey ID
            /// </summary>
            /// <param name="id">ID of hotkey</param>
            /// <returns>The key of the hotkey</returns>
            public Keys GetKeyFromID(int id) {
                try {
                    return _keys[id].Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    if(id >= 0) _keys.Add(id, new FPSKey());
                    return Keys.None;
                }
            }
            /// <summary>
            /// Fetches the limit of the specified hotkey ID
            /// </summary>
            /// <param name="id">ID of hotkey</param>
            /// <returns>The hotkey's FPS limit</returns>
            public int GetLimitFromID(int id) {
                try {
                    return _keys[id].Limit;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}. Generating new entry.", id);
                    if(id >= 0) _keys.Add(id, new FPSKey());
                    return -1;
                }
            }

            /// <summary>
            /// Fetches the limit of the specified key
            /// </summary>
            /// <param name="key">Key of the hotkey</param>
            /// <returns>The hotkey's FPS Limit</returns>
            public int GetLimitFromKey(Keys key) {
                foreach(FPSKey k in _keys.Values) {
                    if(k.Key == key) return k.Limit;
                }
                return -1;
            }

            /// <summary>
            /// Checks if the data structure contains a hotkey with the specified <see cref="Keys"/>
            /// </summary>
            /// <param name="key">Key to check</param>
            /// <returns><see langword="true"/> if key is found</returns>
            public bool ContainsKey(Keys key) {
                foreach(FPSKey k in _keys.Values) {
                    if(k.Key == key) return true;
                }
                return false;
            }

            /// <summary>
            /// Changes the FPS limit of the specified hotkey ID
            /// </summary>
            /// <param name="id">ID of the hotkey</param>
            /// <param name="limit">New FPS Limit</param>
            public void ChangeLimit(int id, int limit) {
                if(id == -1) return;
                var key = GetKeyFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }
            /// <summary>
            /// Changes the <see cref="Keys"/> of the specified hotkey ID
            /// </summary>
            /// <param name="id">ID of the hotkey</param>
            /// <param name="key">New Key</param>
            public void ChangeKey(int id, Keys key) {
                if(id == -1) return;
                var limit = GetLimitFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }

            /// <summary>
            /// Serializes the wrapped Dictionary into a JSON object string
            /// </summary>
            /// <returns>The JSON string representation</returns>
            public string SerializeIntoJSON() => JsonConvert.SerializeObject(_keys, Formatting.Indented);

            /// <summary>
            /// Struct that stores both the <see cref="Keys"/> and Limit together
            /// </summary>
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
