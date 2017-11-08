using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Represents the physical window.
    /// </summary>
    public static class DesktopWindow
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

        /// <summary>
        /// The window's process name
        /// </summary>
        public const string WindowProcess = "LeagueClientUx";

        /// <summary>
        /// Get the launcher window's handle, or IntPtr.Zero if it isn't open.
        /// </summary>
        public static IntPtr Pointer =>
            (Process.GetProcessesByName(WindowProcess).FirstOrDefault()?.MainWindowHandle) ?? IntPtr.Zero;

        /// <summary>
        /// Is the launcher open?
        /// </summary>
        public static bool IsOpen => Process.GetProcessesByName(WindowProcess).Any();

        /// <summary>
        /// Is the launcher on the foreground?
        /// </summary>
        public static bool IsForeground => IsOpen && GetForegroundWindow() == Pointer;

        /// <summary>
        /// Returns the window's bounds, or Rectangle.Empty if it isn't open.
        /// </summary>
        public static Rectangle Bounds
        {
            get
            {
                if (!IsOpen)
                    return Rectangle.Empty;
                
                if (GetWindowRect(new HandleRef(new object(), Pointer), out var rect))
                {
                    Rectangle ret = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                    return ret;
                }
                
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// Bring launcher window to front
        /// </summary>
        public static bool BringToForeground()
        {
            IntPtr handle = Pointer;

            if (handle == null)
                return false;

            return SetForegroundWindow(handle);
        }
    }
}
