using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Launcher_Window
{
    internal static class Capture
    {
        #region WinApi
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        #endregion

        private static Bitmap PrintWindow(IntPtr hwnd)
        {
            var rect = Window.Bounds;

            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bmp);
            IntPtr hdc = gfx.GetHdc();

            PrintWindow(hwnd, hdc, 0);

            gfx.ReleaseHdc(hdc);
            gfx.Dispose();

            return bmp;
        }
    }
}
