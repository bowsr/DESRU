using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    internal class HotkeyHandler {

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

        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(Keys vKey);
    }
}
