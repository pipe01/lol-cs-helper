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
            anal.Window.GraphicsWindow.SetTestPicture(Image.FromFile("test.png") as Bitmap);

            Loop();
        }

        static void Loop()
        {
            Console.WriteLine("Start");

            GraphicalWindow gWindow = anal.Window.GraphicsWindow;
            
            string[] finalChampions = new string[10];

            Stopwatch sw = Stopwatch.StartNew();

            int i = 0;
            foreach (var item in anal.Window.Regions.Where(o => !(o.RegionData as ChampionWindowRegionData).IsChoosing))
            {
                var bmp = gWindow.GetRegionBitmap(item);
                bmp.Save(item.Name + ".png");

                var champ = anal.GetChampion(bmp).ConfigureAwait(false).GetAwaiter().GetResult();

                if (champ == "")
                    champ = "None";

                if (SquareBitmapHelper.IsEmptyChampion(bmp))
                    Console.WriteLine("{0}\tEmpty", item.Name);
                else
                    Console.WriteLine("{0}\t{1}", item.Name, champ);
            }

            sw.Stop();
            
            Console.WriteLine("Done in {0:0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();

            Console.Clear();
            Loop();
        }
    }
}
