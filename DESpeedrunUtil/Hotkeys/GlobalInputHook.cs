using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Hotkeys {
    /// <summary>
    /// A class that manages a global low level input hook
    /// </summary>
    class GlobalInputHook {
        #region Constant, Structure and Delegate Definitions
        /// <summary>
        /// defines the callback type for the hook
        /// </summary>
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);
        private KeyboardHookProc _keyHookDelegate;

        public delegate int MouseHookProc(int code, int wParam, ref MouseHookStruct lParam);
        private MouseHookProc _mouseHookDelegate;

        public struct KeyboardHookStruct {
            public int VKCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }
        public struct MouseHookStruct {
            public Point Point;
            public int MouseData;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;

        const int WH_MOUSE_LL = 14;
        const int WM_MOUSEMOVE = 0x200;
        const int WM_LBUTTONDOWN = 0x201;
        const int WM_RBUTTONDOWN = 0x204;
        const int WM_MBUTTONDOWN = 0x207;
        const int WM_LBUTTONUP = 0x202;
        const int WM_RBUTTONUP = 0x205;
        const int WM_MBUTTONUP = 0x208;
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_MOUSEHWHEEL = 0x020E;
        const int WM_XBUTTONDOWN = 0x020B;
        const int WM_XBUTTONUP = 0x020C;

        const byte XBUTTON1 = 0x1;
        const byte XBUTTON2 = 0x2;
        #endregion

        #region Instance Variables
        /// <summary>
        /// The collections of keys to watch for. Includes MouseButtons translated to Keys
        /// </summary>
        public List<Keys> HookedKeys = new();
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        IntPtr _kHook = IntPtr.Zero;
        IntPtr _mHook = IntPtr.Zero;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;
        /// <summary>
        /// Occurs when one of the hooked mouse buttons is pressed
        /// </summary>
        public event MouseEventHandler MouseDown;
        /// <summary>
        /// Occurs when one of the hooked mouse buttons is released
        /// </summary>
        public event MouseEventHandler MouseUp;
        /// <summary>
        /// Occurs when the mouse wheel is scrolled in either direction
        /// </summary>
        public event EventHandler MouseScroll;
        #endregion

        #region Constructors and Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalInputHook"/> class and installs the keyboard hook.
        /// </summary>
        public GlobalInputHook() {
            _keyHookDelegate = KeyProc;
            _mouseHookDelegate = MouseProc;
            Hook();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="GlobalInputHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~GlobalInputHook() {
            Unhook();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Installs the global hook
        /// </summary>
        public void Hook() {
            IntPtr hInstance = LoadLibrary("User32");
            _kHook = SetWindowsHookEx(WH_KEYBOARD_LL, _keyHookDelegate, hInstance, 0);
            _mHook = SetWindowsHookEx(WH_MOUSE_LL, _mouseHookDelegate, hInstance, 0);
        }

        /// <summary>
        /// Uninstalls the global hook
        /// </summary>
        public void Unhook() {
            UnhookWindowsHookEx(_kHook);
            UnhookWindowsHookEx(_mHook);
        }

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int KeyProc(int code, int wParam, ref KeyboardHookStruct lParam) {
            if (code >= 0) {
                Keys key = (Keys)lParam.VKCode;
                if (HookedKeys.Contains(key)) {
                    KeyEventArgs kea = new KeyEventArgs(key);
                    if((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && KeyDown != null) {
                        KeyDown(this, kea);
                    }else if((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && KeyUp != null) {
                        KeyUp(this, kea);
                    }
                    if(kea.Handled) return 1;
                }
            }
            return CallNextHookEx(_kHook, code, wParam, ref lParam);
        }

        /// <summary>
        /// The callback for the mouse hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The mousehook event information</param>
        /// <returns></returns>
        public int MouseProc(int code, int wParam, ref MouseHookStruct lParam) {
            if(code >= 0) {
                ushort subCode;
                if(wParam == WM_MOUSEWHEEL) {
                    subCode = HighWord(lParam.MouseData);
                    if((subCode == 120 || subCode == 65416) && MouseScroll != null) {
                        MouseScroll(this, new MouseWheelEventArgs(subCode == 65416));
                    }
                }else {
                    var button = MouseButtons.None;
                    switch(wParam) {
                        case WM_MBUTTONDOWN:
                        case WM_MBUTTONUP:
                            button = MouseButtons.Middle;
                            break;
                        case WM_XBUTTONDOWN:
                        case WM_XBUTTONUP:
                            subCode = HighWord(lParam.MouseData);
                            if(subCode == XBUTTON1) button = MouseButtons.XButton1;
                            if(subCode == XBUTTON2) button = MouseButtons.XButton2;
                            break;
                    }
                    if(button != MouseButtons.None && HookedKeys.Contains(HotkeyHandler.ConvertMouseButton(button))) {
                        var mbea = new MouseEventArgs(button, 1, 0, 0, 0);
                        if((wParam == WM_MBUTTONDOWN || wParam == WM_XBUTTONDOWN) && MouseDown != null) {
                            MouseDown(this, mbea);
                        }else if((wParam == WM_MBUTTONUP || wParam == WM_XBUTTONUP) && MouseUp != null) {
                            MouseUp(this, mbea);
                        }
                    }
                }
            }
            return CallNextHookEx(_mHook, code, wParam, ref lParam);
        }
        #endregion

        public static ushort HighWord(int data) => (ushort) ((data >> 16) & 0xffff);
        public static ushort LowWord(int data) => (ushort) data;

        #region DLL imports
        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, MouseHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);
        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref MouseHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
        #endregion

        /// <summary>
        /// Extension of EventArgs to send MouseWheel scroll direction
        /// </summary>
        internal class MouseWheelEventArgs: EventArgs {
            public bool Direction { get; init; }

            public MouseWheelEventArgs(bool dir): base() {
                Direction = dir;
            }
        }
    }
}
