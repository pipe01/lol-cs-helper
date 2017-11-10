using FastBitmapLib;
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

        /// <summary>
        /// Gets the average color for an area of the target image
        /// </summary>
        /// <param name="area">Area rectangle</param>
        public static Color GetAverageColorForArea(this Bitmap Target, Rectangle area)
        {
            lock (Target)
            {
                int allR = 0, allG = 0, allB = 0;
                int count = 0;

                using (var ub = Target.FastLock())
                {
                    for (int x = area.X; x < area.Width + area.X; x++)
                    {
                        for (int y = area.Y; y < area.Height + area.Y; y++)
                        {
                            var data = ub.GetPixel(x, y);

                            allR += data.R;
                            allG += data.G;
                            allB += data.B;

                            count++;
                        }
                    }
                }
                
                return Color.FromArgb(allR / count, allG / count, allB / count);
            }
        }

        /// <summary>
        /// Convert image to two-dimensional array containg the raw pixels
        /// </summary>
        public static Color[,] GetColorArray(this Bitmap bmp)
        {
            Color[,] ret = new Color[bmp.Width, bmp.Height];
            using (var fastBmp = bmp.FastLock())
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        ret[x, y] = fastBmp.GetPixel(x, y);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Compare two bitmaps and return the difference percetange. 0% means they are the same, 100%
        /// means they are completely different.
        /// </summary>
        /// <param name="Target">First bitmap in the form of two-dimensional pixel array.</param>
        /// <param name="to">Seconds bitmap</param>
        /// <returns></returns>
        public static float Compare(this Color[,] Target, Bitmap to)
        {
            Size size;
            Color[,] CA, CB;

            lock (Target)
            {
                lock (to)
                {
                    //if (Target.Size != to.Size)
                    //    throw new ArgumentException("The images' sizes must match.", nameof(to));

                    size = to.Size;

                    CA = Target;
                    CB = GetColorArray(to);
                }
            }

            float diff = 0;

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    diff += (float)Math.Abs(CA[x, y].R - CB[x, y].R) / 255;
                    diff += (float)Math.Abs(CA[x, y].G - CB[x, y].G) / 255;
                    diff += (float)Math.Abs(CA[x, y].B - CB[x, y].B) / 255;
                }
            }

            return 100 * diff / (size.Width * size.Height * 3);
        }
    }
}
