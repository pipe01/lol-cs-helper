using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static async Task<string[]> GetAllChampions()
        {
            GraphicalWindow gWindow = Window.GraphicsWindow;

            string[] finalChampions = new string[10];

            Stopwatch sw = Stopwatch.StartNew();

            //Join the picking and not-picking champion regions together in a tuple list
            List<Tuple<WindowRegion, WindowRegion>> regionTuples = new List<Tuple<WindowRegion, WindowRegion>>();

            WindowRegion pairFirst = null;
            foreach (var item in Window.Regions)
            {
                var data = item.RegionData as ChampionWindowRegionData;

                if (!data.IsChoosing)
                    pairFirst = item;
                else
                    regionTuples.Add(new Tuple<WindowRegion, WindowRegion>(pairFirst, item));
            }


            int i = 0;
            foreach (var item in regionTuples)
            {
                var goodRegion = await GetGoodRegion(gWindow, item.Item1, item.Item2);
                Bitmap goodBitmap = gWindow.GetRegionBitmap(goodRegion.Item1);
                string champion = goodRegion.Item2;
                
                if (champion == "")
                    champion = "None";

                finalChampions[i++] = champion;

                Debug.WriteLine("{0}\t{1}", goodRegion.Item1.Name, champion);
            }

            sw.Stop();

            Debug.WriteLine("Took {0} ms", sw.ElapsedMilliseconds);

            return finalChampions;
        }

        private static async Task<Tuple<WindowRegion, string>> GetGoodRegion(GraphicalWindow gWindow,
            WindowRegion region1, WindowRegion region2)
        {
            //Get both of the bitmaps
            Bitmap pBmp = gWindow.GetRegionBitmap(region1);
            Bitmap npBmp = gWindow.GetRegionBitmap(region2);

            string pChamp, npChamp;

            //If the picking bitmap is an empty champion, it means that the user is picking, but he hasn't
            //chosen any champion yet
            if (SquareBitmapHelper.IsEmptyChampion(pBmp))
                pChamp = "Empty";
            else //If it isn't, try to get the champion
                pChamp = await GetChampion(pBmp);

            //If the locked champion bitmap isn't empty, the user has already picked a champion
            if (SquareBitmapHelper.IsEmptyChampion(npBmp))
                npChamp = "Empty";
            else
                npChamp = await GetChampion(npBmp);


            //TODO: Maybe switch the order of the following statements
            //If the picking champion is actually a champion, choose that as the "good one"
            if (pChamp != "")
            {
                return new Tuple<WindowRegion, string>(region1, pChamp);
            }
            else if (npChamp != "") //The locked champion is valid, use that one
            {
                return new Tuple<WindowRegion, string>(region2, npChamp);
            }

            return null;
        }
    }
}
