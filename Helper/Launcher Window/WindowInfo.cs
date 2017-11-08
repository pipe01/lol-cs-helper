using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Launcher_Window
{
    internal static class WindowInfo
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr WindowHandle);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public const string WindowProcess = "LeagueClientUx";

        /// <summary>
        /// Get the launcher window's handle, or null if it isn't open.
        /// </summary>
        public static IntPtr? Pointer => Process.GetProcessesByName(WindowProcess).FirstOrDefault()?.MainWindowHandle;
    }
}
