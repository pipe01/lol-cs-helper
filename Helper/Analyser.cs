using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Analyser
    {
        private static bool Initialised = false;
        public static VirtualWindow Window { get; private set; } = new VirtualWindow();

        static Analyser()
        {
            if (!Initialised)
                Window.AddChampionRegions();
        }
        
        public static async Task<string> GetChampion(Bitmap bmp)
        {
            int comparisonSize = 10;

            var images = await Riot.GetChampionImagesAsync(comparisonSize);

            Bitmap resizedTarget = bmp.Resize(comparisonSize, comparisonSize);
            Color[,] arrayTarget = resizedTarget.GetColorArray();
            List<Tuple<string, float>> champions = new List<Tuple<string, float>>();
            
            foreach (var item in images)
            {
                float score = arrayTarget.Compare(item.Value as Bitmap);

                champions.Add(new Tuple<string, float>(item.Key, score));
            }

            champions.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            var lowest = champions.First();
            
            if (lowest.Item2 > 15)
                return "";

            images.Clear();

            return lowest.Item1;
        }

    }
}
