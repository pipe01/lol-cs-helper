using Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            Analyser anal = new Analyser();

            GraphicalWindow gWindow = anal.Window.GraphicsWindow;
            gWindow.SetTestPicture(Image.FromFile("test.png") as Bitmap);

            foreach (var item in anal.Window.Regions)//.Where(o => (o.RegionData as ChampionWindowRegionData).Team == ChampionTeam.Enemy))
            {
                var bmp = gWindow.GetRegionBitmap(item);
                bmp.Save(item.Name + ".png");

                var champ = SquareBitmapHelper.IsChampion(bmp);
                if (champ)
                    Console.WriteLine("{0}: {1}", item.Name, champ);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
