using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2
{
    class Program
    {
        static Analyser anal = new Analyser();

        
        static void Main(string[] args)
        {
            Analyser.Window.GraphicsWindow.SetTestPicture(Image.FromFile("test.png") as Bitmap);

            Loop();
        }

        static void Loop()
        {
            Console.WriteLine("Start");

            GraphicalWindow gWindow = Analyser.Window.GraphicsWindow;
            
            string[] finalChampions = new string[10];

            Stopwatch sw = Stopwatch.StartNew();

            //Join the picking and not-picking champion regions together in a tuple list
            List<Tuple<WindowRegion, WindowRegion>> regionTuples = new List<Tuple<WindowRegion, WindowRegion>>();

            WindowRegion pairFirst = null;
            foreach (var item in Analyser.Window.Regions)
            {
                var data = item.RegionData as ChampionWindowRegionData;

                if (!data.IsChoosing)
                    pairFirst = item;
                else
                    regionTuples.Add(new Tuple<WindowRegion, WindowRegion>(pairFirst, item));
            }

            foreach (var item in regionTuples)
            {
                WindowRegion goodRegion = null;
                Bitmap goodBitmap = null;
                string champion = null;


                //We get both of the elements, the one where the player is choosing, and the one where he isn't
                var picking = item.Item1;
                var notPicking = item.Item2;

                
                //Get both of the bitmaps
                Bitmap pBmp = gWindow.GetRegionBitmap(picking);
                Bitmap npBmp = gWindow.GetRegionBitmap(notPicking);

                string pChamp, npChamp;


                //If the picking bitmap is an empty champion, it means that the user is picking, but he hasn't
                //chosen any champion yet
                if (SquareBitmapHelper.IsEmptyChampion(pBmp))
                    pChamp = "Empty";
                else //If it isn't, try to get the champion
                    pChamp = anal.GetChampion(pBmp).ConfigureAwait(false).GetAwaiter().GetResult();

                //If the locked champion bitmap isn't empty, the user has already picked a champion
                if (SquareBitmapHelper.IsEmptyChampion(npBmp))
                    npChamp = "Empty";
                else
                    npChamp = anal.GetChampion(npBmp).ConfigureAwait(false).GetAwaiter().GetResult();
                

                //TODO: Maybe switch the order of the following statements
                //If the picking champion is actually a champion, choose that as the "good one"
                if (pChamp != "")
                {
                    goodRegion = picking;
                    goodBitmap = pBmp;
                    champion = pChamp;
                }
                else if (npChamp != "") //The locked champion is valid, use that one
                {
                    goodRegion = notPicking;
                    goodBitmap = npBmp;
                    champion = npChamp;
                }

                var bmp = goodBitmap;
                bmp.Save(goodRegion.Name + ".png");
                
                if (champion == "")
                    champion = "None";

                Console.WriteLine("{0}\t{1}", goodRegion.Name, champion);
            }
            
            sw.Stop();
            
            Console.WriteLine("Done in {0:0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();

            Console.Clear();
            Loop();
        }
    }
}
