﻿using DESpeedrunUtil.Macro;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    internal class HotkeyHandler {

        private globalKeyboardHook _hook;
        private readonly MainWindow _parent;

        public bool Enabled { get; private set; }
        private bool _key0Enabled = false, _key1Enabled = false, _key2Enabled = false, _key3Enabled = false;
        private Keys _fpsHotkey0 = Keys.None;
        private Keys _fpsHotkey1 = Keys.None;
        private Keys _fpsHotkey2 = Keys.None;
        private Keys _resScaleHotkey = Keys.None;

        public HotkeyHandler(Keys fps0, Keys fps1, Keys fps2, Keys res, MainWindow parent) {
            _hook = new globalKeyboardHook();
            _parent = parent;
            _fpsHotkey0 = fps0;
            _fpsHotkey1 = fps1;
            _fpsHotkey2 = fps2;
            _resScaleHotkey = res;

            _hook.KeyDown += new KeyEventHandler(Hook_KeyDown);
            _hook.KeyUp += new KeyEventHandler(Hook_KeyUp);
        }

        public Keys GetHotkeyByNumber(int num) {
            return num switch {
                0 => _fpsHotkey0,
                1 => _fpsHotkey1,
                2 => _fpsHotkey2,
                3 => _resScaleHotkey,
                _ => Keys.None,
            };
        }
        public void ChangeHotkey(Keys key, int fps) {
            switch(fps) {
                case 0:
                    _fpsHotkey0 = key;
                    break;
                case 1:
                    _fpsHotkey1 = key;
                    break;
                case 2:
                    _fpsHotkey2 = key;
                    break;
                case 3:
                    _resScaleHotkey = key;
                    break;
            }
            RefreshKeys();
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
        public void ToggleIndividualHotkeys(int hotkey, bool enabled) {
            switch(hotkey) {
                case 0:
                    _key0Enabled = enabled;
                    break;
                case 1:
                    _key1Enabled = enabled;
                    break;
                case 2:
                    _key2Enabled = enabled;
                    break;
                case 3:
                    _key3Enabled = enabled;
                    break;
            }
            RefreshKeys();
        }

        private void AddHotkeys() {
            _hook.HookedKeys.Clear();
            if(_fpsHotkey0 != Keys.None && _key0Enabled) _hook.HookedKeys.Add(_fpsHotkey0);
            if(_fpsHotkey1 != Keys.None && _key1Enabled) _hook.HookedKeys.Add(_fpsHotkey1);
            if(_fpsHotkey2 != Keys.None && _key2Enabled) _hook.HookedKeys.Add(_fpsHotkey2);
            if(_resScaleHotkey != Keys.None && _key3Enabled) _hook.HookedKeys.Add(_resScaleHotkey);
        }

        private void Hook_KeyDown(object sender, KeyEventArgs e) {
            int hk = -1;
            if(e.KeyCode == Keys.None || (e.Alt && e.KeyCode == Keys.F4)) {
                e.Handled = false;
                return;
            }
            if(e.KeyCode == _fpsHotkey0) {
                hk = 0;
            }else if(e.KeyCode == _fpsHotkey1) {
                hk = 1;
            }else if(e.KeyCode == _fpsHotkey2) {
                hk = 2;
            }else if(e.KeyCode == _resScaleHotkey) {
                hk = 3;
            }
            _parent.HotkeyPressed(hk);
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
            //   type: 0-2 -> HotkeyHandler FPSHotkeyX
            //         3   -> Res Scaling Hotkey
            //         4-5 -> FreescrollMacro (type == 4) DownScrollKey if true, UpScrollKey if false
            if(key != Keys.None) {
                if(type < 0 || type > 5) return;
                Keys oldKey;
                if(type <= 3) {
                    oldKey = hotkeys.GetHotkeyByNumber(type);
                }else {
                    oldKey = macro.GetHotkey(type == 4);
                }
                for(int i = 0; i <= 5; i++) {
                    if(i == type) continue;
                    if(i <= 3) {
                        if(key == hotkeys.GetHotkeyByNumber(i)) {
                            Log.Verbose("Duplicate hotkey found. Swapping {Key0} with {Key1}", oldKey, key);
                            hotkeys.ChangeHotkey(oldKey, i);
                            break;
                        }
                    } else {
                        var down = (i == 4);
                        if(key == macro.GetHotkey(down)) {
                            Log.Verbose("Duplicate hotkey found. Swapping {Key0} with {Key1}", oldKey, key);
                            macro.ChangeHotkey(oldKey, down);
                            break;
                        }
                    }
                }
            }

            if(type <= 3) {
                hotkeys.ChangeHotkey(key, type);
                Log.Information("Hotkey {HK} changed to {Key}", type, key);
            } else {
                macro.ChangeHotkey(key, type == 4);
                Log.Information("Macro key <{Type}> changed to {Key}", type == 4, key);
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

        private class FPSHotkeyMap {

            private Dictionary<int, FPSKey> _keys;

            public FPSHotkeyMap() {
                Initialize(File.ReadAllText(@".\fpskeys.json"));
            }
            public FPSHotkeyMap(string json) {
                Initialize(json);
            }
            private void Initialize(string json) {
                _keys = JsonConvert.DeserializeObject<Dictionary<int, FPSKey>>(json);
            }

            public Keys GetKeyAndLimitFromID(int id, out int fps) {
                try {
                    var fpskey = _keys[id];
                    fps = fpskey.Limit;
                    return fpskey.Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}", id);
                    fps = -1;
                    return Keys.None;
                }
            }
            public Keys GetKeyFromID(int id) {
                try {
                    return _keys[id].Key;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}", id);
                    return Keys.None;
                }
            }
            public int GetLimitFromID(int id) {
                try {
                    return _keys[id].Limit;
                }catch(KeyNotFoundException e) {
                    Log.Error(e, "There is no FPSKey associated with ID: {ID}", id);
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
                var key = GetKeyFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }
            public void ChangeKey(int id, Keys key) {
                var limit = GetLimitFromID(id);
                _keys[id] = new FPSKey(key, limit);
            }

            public string SerializeIntoJSON() => JsonConvert.SerializeObject(_keys, Formatting.Indented);

            struct FPSKey {
                public Keys Key { get; init; }
                public int Limit { get; init; }

                public FPSKey(Keys k, int fps) {
                    Key = k;
                    Limit = fps;
                }
            }
        }
    }
}
