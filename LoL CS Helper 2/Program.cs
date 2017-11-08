using Helper;
using System;
using System.Collections.Generic;
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

            VirtualWindow window = new VirtualWindow();
            window.Regions.Add(VirtualWindow.WindowRegion.FromAbsolute("Region1", 50, 50, 100, 100, 1280, 720));

            GraphicalWindow gWindow = new GraphicalWindow(window);
            gWindow.RefreshWindowPicture();
            var bmp = gWindow.GetRegionBitmap(window.Regions.First());

            bmp.Save("test.png");

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
