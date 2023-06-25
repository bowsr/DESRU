using System.Runtime.InteropServices;

namespace DESpeedrunUtil.Interop {
    internal static class DLLImports {

        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        internal static extern ushort GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

    }
}
