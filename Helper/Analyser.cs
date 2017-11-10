using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Analyser
    {
        public VirtualWindow Window { get; private set; } = new VirtualWindow();

        public Analyser()
        {
            Window.AddChampionRegions();
        }

        public async Task<string> GetChampion(Bitmap bmp)
        {
            int comparisonSize = 15;

            var images = await Riot.GetChampionImagesAsync(comparisonSize);

            Bitmap resizedTarget = bmp.ResizeBitmap(comparisonSize, comparisonSize);
            Color[,] arrayTarget = GetColorArray(resizedTarget);
            List<Tuple<string, float>> champions = new List<Tuple<string, float>>();
            
            foreach (var item in images)
            {
                float score = CompareBitmap(arrayTarget, item.Value as Bitmap);

                champions.Add(new Tuple<string, float>(item.Key, score));
            }

            champions.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            var lowest = champions.First();

            Console.WriteLine("Lowest: " + lowest.Item2);

            if (lowest.Item2 > 15)
                return "";

            images.Clear();

            return lowest.Item1;
        }

        private static Color[,] GetColorArray(Bitmap bmp)
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

        private static float CompareBitmap(Color[,] Target, Bitmap to)
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
