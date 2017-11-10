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
        /// Checks if the specified bitmap belongs to an empty champion.
        /// </summary>
        /// <param name="bmp">The bitmap to check.</param>
        public static bool IsEmptyChampion(Bitmap bmp)
        {
            int areaSize = 5;
            Rectangle centerRect = new Rectangle(
                bmp.Width / 2 - areaSize / 2,
                bmp.Height / 2 - areaSize / 2,
                areaSize,
                areaSize);

            Rectangle rightRect = new Rectangle(
                bmp.Width - areaSize,
                bmp.Height / 2 - areaSize / 2,
                areaSize,
                areaSize);

            //We check if either the center of the image or the middle-right 5x5 areas match the empty color

            return CheckRect(centerRect) || CheckRect(rightRect);

            //Check if the average color for a rectangle matches the empty color
            bool CheckRect(Rectangle rect)
                => KindaEquals(bmp.GetAverageColorForArea(rect), Color.FromArgb(30, 40, 40), 5);
        }

        /// <summary>
        /// Checks if two colors are the same, with a tolerance
        /// </summary>
        /// <param name="a">First color.</param>
        /// <param name="b">Second color.</param>
        /// <param name="threshold">Maximum difference between two color components.</param>
        /// <returns></returns>
        private static bool KindaEquals(Color a, Color b, int threshold = 5)
        {
            return
                (Math.Abs(a.R - b.R) <= threshold) &&
                (Math.Abs(a.G - b.G) <= threshold) &&
                (Math.Abs(a.B - b.B) <= threshold);
        }
    }
}
