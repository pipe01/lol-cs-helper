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
        public static bool IsEmptyChampion(Bitmap bmp)
        {
            using (var fastBmp = bmp.FastLock())
            {
                var points = GetRandomPoints(fastBmp, 20);
                int correct = points.Count(o => KindaEquals(o, Color.FromArgb(28, 37, 41)));

                return correct > 5;
            }
        }

        private static bool KindaEquals(Color a, Color b, int threshold = 5)
        {
            return
                (Math.Abs(a.R - b.R) <= threshold) &&
                (Math.Abs(a.G - b.G) <= threshold) &&
                (Math.Abs(a.B - b.B) <= threshold);
        }

        private static Color[] GetRandomPoints(FastBitmap bmp, int points)
        {
            Random rnd = new Random();
            Color[] ret = new Color[points];

            for (int i = 0; i < points; i++)
            {
                ret[i] = bmp.GetPixel(rnd.Next(0, bmp.Width), rnd.Next(0, bmp.Height));
            }

            return ret;
        }
    }
}
