using Helper;
using Helper.Counters;
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
            //Analyser.Window.GraphicsWindow.SetTestPicture(Image.FromFile("test.png") as Bitmap);

            new frmOverlay(Configuration.LoadFromFile("config.json")).ShowDialog();

            Loop();
        }

        static void Loop()
        {
            ProviderLolCounter counters = new ProviderLolCounter();

            foreach (var item in Analyser.GetAllChampions().GetAwaiter().GetResult())
            {
                string cStr;

                if (item == "Empty")
                {
                    cStr = "";
                }
                else
                {
                    var cList = counters.GetMatchupsForChampion(item)
                    .Where(o => o.Type == MatchupProvider.Matchup.MatchupType.WeakAgainst)
                    .Select(o => o.Against)
                    .ToArray();

                    cStr = String.Join(", ", cList);
                }

                Console.WriteLine("{0}: {1}", item, cStr);
            }
            
            Console.ReadLine();

            Console.Clear();
            Loop();
        }
    }
}
