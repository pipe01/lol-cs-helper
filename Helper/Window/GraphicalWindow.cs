﻿using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Represents the graphics of the window.
    /// </summary>
    public class GraphicalWindow
    {
        private VirtualWindow _VirtualWindow;
        private Bitmap _WindowBitmap = new Bitmap(1, 1);
        private bool _DebugPicture = false;

        /// <summary>
        /// Whether to capture the window every time a method is called.
        /// </summary>
        public bool AutoCapture { get; set; } = false;

        /// <summary>
        /// Construct a new <see cref="GraphicalWindow"/>.
        /// </summary>
        /// <param name="virtualWindow">Virtual window that we are going to work with.</param>
        internal GraphicalWindow(VirtualWindow virtualWindow)
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
        /// Set the picture to the passed bitmap.
        /// </summary>
        /// <param name="bmp">Custom window picture used for debugging. The picture won't be refreshed
        /// when calling <see cref="RefreshWindowPicture"/>.</param>
        public void SetTestPicture(Bitmap bmp)
        {
            _DebugPicture = true;
            _WindowBitmap = bmp;
        }

        /// <summary>
        /// Capture the window and store it for later use.
        /// </summary>
        public void RefreshWindowPicture()
        {
            if (_DebugPicture)
                return;

            _WindowBitmap = WindowCapture.CaptureWindow();
        }

        /// <summary>
        /// Crop a region from the window picture.
        /// </summary>
        /// <param name="region">Window to crop.</param>
        public Bitmap GetRegionBitmap(WindowRegion region)
        {
            //If the region doesn't belong to this window, abort
            if (!_VirtualWindow.Regions.Contains(region))
                return null;

            //Get the window picture
            Bitmap window = GetWindowBitmap();

            //If the picture is null, abort
            if (window == null)
                return null;

            //Get the region's absolute rectangle for the window
            Rectangle regionRect = region.GetAbsoluteRect(window.Width, window.Height);

            //Create region bitmap
            Bitmap regionBitmap = new Bitmap(regionRect.Width, regionRect.Height);

            //Use FastBitmapLib
            using (var fastBmp = regionBitmap.FastLock())
            {
                //Copy the region from the window to the region bitmap
                fastBmp.CopyRegion(window, regionRect, new Rectangle(0, 0, regionBitmap.Width, regionBitmap.Height));
            }

            return regionBitmap;
        }
    }
}
