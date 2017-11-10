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
        static void Main(string[] args)
        {
            Analyser.Window.GraphicsWindow.SetTestPicture(Image.FromFile("test.png") as Bitmap);

            Loop();
        }

        static void Loop()
        {
            foreach (var item in Analyser.GetAllChampions().GetAwaiter().GetResult())
            {
                Console.WriteLine(item);
            }
            
            Console.ReadLine();

            Console.Clear();
            Loop();
        }
    }
}
