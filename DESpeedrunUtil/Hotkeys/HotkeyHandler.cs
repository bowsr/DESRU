using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    internal class HotkeyHandler {

        private globalKeyboardHook Hook;
        private MainWindow Parent;

        public bool Enabled { get; private set; }
        public Keys FPSHotkey0 { get; private set; }
        public Keys FPSHotkey1 { get; private set; }
        public Keys FPSHotkey2 { get; private set; }

        public HotkeyHandler(Keys fps0, Keys fps1, Keys fps2, MainWindow parent) {
            Hook = new globalKeyboardHook();
            Parent = parent;
            FPSHotkey0 = fps0;
            FPSHotkey1 = fps1;
            FPSHotkey2 = fps2;

            Hook.KeyDown += new KeyEventHandler(Hook_KeyDown);
            Hook.KeyUp += new KeyEventHandler(Hook_KeyUp);
        }

        public void EnableHotkeys() {
            if(Enabled) return;
            AddHotkeys();
            Enabled = true;
        }
        public void DisableHotkeys() {
            if(!Enabled) return;
            Hook.HookedKeys.Clear();
            Enabled = false;
        }
        public void RefreshKeys() {
            if(!Enabled) return;
            AddHotkeys();
        }

        private void AddHotkeys() {
            Hook.HookedKeys.Clear();
            if(FPSHotkey0 != Keys.None) Hook.HookedKeys.Add(FPSHotkey0);
            if(FPSHotkey1 != Keys.None) Hook.HookedKeys.Add(FPSHotkey1);
            if(FPSHotkey2 != Keys.None) Hook.HookedKeys.Add(FPSHotkey2);
        }

        private void Hook_KeyDown(object sender, KeyEventArgs e) {
            int hk = -1;
            if(e.KeyCode == FPSHotkey0) {
                if(e.KeyCode == Keys.None) {
                    e.Handled = false;
                    return;
                }
                hk = 0;
            }else if(e.KeyCode == FPSHotkey1) {
                if(e.KeyCode == Keys.None) {
                    e.Handled = false;
                    return;
                }
                hk = 1;
            }else if(e.KeyCode == FPSHotkey2) {
                if(e.KeyCode == Keys.None) {
                    e.Handled = false;
                    return;
                }
                hk = 2;
            }
            Parent.ToggleFPSCap(hk);
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
        public static Keys ModKeySelector(int modifier) {
            Keys pressedKey;

            switch(modifier) {
                case 0:
                    pressedKey = (GetAsyncKeyState(Keys.RControlKey) & 0x01) == 1 ? Keys.RControlKey : Keys.LControlKey;
                    break;
                case 1:
                    pressedKey = (GetAsyncKeyState(Keys.RShiftKey) & 0x01) == 1 ? Keys.RShiftKey : Keys.LShiftKey;
                    break;
                case 2:
                    pressedKey = (GetAsyncKeyState(Keys.RMenu) & 0x01) == 1 ? Keys.RMenu : Keys.LMenu;
                    break;
                default:
                    pressedKey = Keys.None;
                    break;
            }
            return pressedKey;
        }

        public static Keys ConvertMouseButton(MouseButtons button) {
            Keys pressedKey;

            switch(button) {
                case MouseButtons.Left:
                    pressedKey = Keys.LButton;
                    break;
                case MouseButtons.Right:
                    pressedKey = Keys.RButton;
                    break;
                case MouseButtons.Middle:
                    pressedKey = Keys.MButton;
                    break;
                case MouseButtons.XButton1:
                    pressedKey = Keys.XButton1;
                    break;
                case MouseButtons.XButton2:
                    pressedKey = Keys.XButton2;
                    break;
                default:
                    pressedKey = Keys.None;
                    break;
            }
            return pressedKey;
        }

        public static string TranslateKeyNames(Keys key) {
            string name;
            switch(key) {
                case Keys.LButton:
                    name = "MOUSE1";
                    break;
                case Keys.RButton:
                    name = "MOUSE2";
                    break;
                case Keys.MButton:
                    name = "MOUSE3";
                    break;
                case Keys.XButton1:
                    name = "MOUSE4";
                    break;
                case Keys.XButton2:
                    name = "MOUSE5";
                    break;
                case Keys.LMenu:
                    name = "Alt";
                    break;
                case Keys.RMenu:
                    name = "RAlt";
                    break;
                case Keys.LShiftKey:
                    name = "Shift";
                    break;
                case Keys.RShiftKey:
                    name = "RShift";
                    break;
                case Keys.LControlKey:
                    name = "Control";
                    break;
                case Keys.RControlKey:
                    name = "RControl";
                    break;
                case Keys.LWin:
                    name = "Win";
                    break;
                case Keys.RWin:
                    name = "RWin";
                    break;
                default:
                    name = key.ToString();
                    break;
            }
            return name;
        }
        #endregion

        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(Keys vKey);
    }
}
