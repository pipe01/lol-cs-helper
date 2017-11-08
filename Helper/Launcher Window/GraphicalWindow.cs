using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Launcher_Window
{
    /// <summary>
    /// Represents the graphics of the window.
    /// </summary>
    internal class GraphicalWindow
    {
        private VirtualWindow _VirtualWindow;
        private Bitmap _WindowBitmap = new Bitmap(1, 1);

        /// <summary>
        /// Whether to capture the window every time a method is called.
        /// </summary>
        public bool AutoCapture { get; set; } = false;

        /// <summary>
        /// Construct a new <see cref="GraphicalWindow"/>.
        /// </summary>
        /// <param name="virtualWindow">Virtual window that we are going to work with.</param>
        public GraphicalWindow(VirtualWindow virtualWindow)
        {
            _VirtualWindow = virtualWindow;
        }

        private Bitmap GetWindowBitmap()
        {
            if (AutoCapture)
                RefreshWindowPicture();

            return _WindowBitmap;
        }

        /// <summary>
        /// Capture the window and store it for later use.
        /// </summary>
        public void RefreshWindowPicture()
        {
            _WindowBitmap = WindowCapture.CaptureWindow();
        }

        /// <summary>
        /// Crop the region from the window picture.
        /// </summary>
        /// <param name="region">Window to crop.</param>
        public Bitmap GetRegionBitmap(VirtualWindow.WindowRegion region)
        {
            //If the region doesn't belong to this window, abort
            if (!_VirtualWindow.Regions.Contains(region))
                return null;

            //Get the window picture
            Bitmap window = GetWindowBitmap();

            //If the picture is null, abort
            if (window == null)
                return null;

            using (var fastBmp = window.FastLock())
            {
                
            }
        }
    }
}
