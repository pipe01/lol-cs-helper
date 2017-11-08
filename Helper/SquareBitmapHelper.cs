using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class SquareBitmapHelper
    {
        /// <summary>
        /// Return whether the given bitmap is a champion square.
        /// </summary>
        /// <param name="bmp">Bitmap.</param>
        /// <param name="threshold">Maximum difference between corners.</param>
        public static bool IsChampion(Bitmap bmp, int threshold = 15)
        {
            //We get four 9x9 corners of the image and get the average color for each one.
            //Champion square images have the same 4 corners, so the difference between the average colors
            //must be very small

            int s = 3;
            Rectangle[] corners = new Rectangle[]
            {
                new Rectangle(0, 0, s, s),
                new Rectangle(0, bmp.Height - s, s, s),
                new Rectangle(bmp.Width - s, 0, s, s),
                new Rectangle(bmp.Width - s, bmp.Height - s, s, s)
            };

            var reds = new List<int>();

            foreach (var corner in corners)
            {
                var avg = GetAverageColor(bmp, corner);
                reds.Add((avg.R + avg.G + avg.B) / 3);
            }
            
            int delta = reds.Max() - reds.Min();
            
            return delta <= threshold;
        }

        private static Color GetAverageColor(Color[] clrs)
        {
            int r = 0, g = 0, b = 0;

            foreach (var item in clrs)
            {
                r += item.R;
                g += item.G;
                b += item.B;
            }

            return Color.FromArgb(r / clrs.Length, g / clrs.Length, b / clrs.Length);
        }

        /// <summary>
        /// Get an image's average color in an area
        /// </summary>
        /// <param name="bmp">The image</param>
        /// <param name="area">The area</param>
        /// <returns></returns>
        private static Color GetAverageColor(Bitmap bmp, Rectangle area)
        {
            int allR = 0, allG = 0, allB = 0;
            int count = 0;

            using (var fastBmp = bmp.FastLock())
            {
                for (int x = area.X; x < area.Width + area.X; x++)
                {
                    for (int y = area.Y; y < area.Height + area.Y; y++)
                    {
                        var data = fastBmp.GetPixel(x, y);

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
}
