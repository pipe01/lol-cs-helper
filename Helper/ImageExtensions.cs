using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="Target">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        public static Image Resize(this Image Target, int width, int height)
            => (Target as Bitmap).Resize(width, height);

        /// <summary>
        /// Resize the bitmap to the specified width and height.
        /// </summary>
        /// <param name="Target">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        public static Bitmap Resize(this Bitmap Target, int width, int height)
        {
            lock (Target)
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(Target.HorizontalResolution, Target.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(Target, destRect, 0, 0, Target.Width, Target.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
        }
    }
}
