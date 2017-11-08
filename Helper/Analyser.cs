using System;
using System.Collections.Generic;
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
            AddRegions();
        }

        private void AddChampionRegion(bool enemy, bool choosing, int index, string name, int x, int y, int w, int h,
            int launcherW = 1280, int launcherH = 720)
        {
            var region = WindowRegion.FromAbsolute(name, x, y, w, h, launcherW, launcherH);
            region.RegionData = new ChampionWindowRegionData
            {
                Index = index,
                IsChoosing = choosing,
                Team = enemy ? ChampionTeam.Enemy : ChampionTeam.Ally
            };

            Window.Regions.Add(region);
        }

        private void AddRegions()
        {
            //Add all the champion regions
            //I took these measurements from my launcher, so the launcher size is 1280x720

            int squareSize = 60;
            int startY = 104;

            //Left side champion squares
            for (int i = 0; i < 5; i++)
            {
                int y = startY + (i * 80);

                AddChampionRegion(false, true, i, "AllyChoosing" + i, 18, y, squareSize, squareSize);
                AddChampionRegion(false, false, i, "Ally" + i, 56, y, squareSize, squareSize);
            }

            //Right side champion squares
            for (int i = 0; i < 5; i++)
            {
                int y = startY + (i * 80);
                AddChampionRegion(true, true, i, "EnemyChoosing" + i, 1240, y, squareSize, squareSize);
                AddChampionRegion(true, false, i, "Enemy" + i, 1202, y, squareSize, squareSize);
            }
        }
    }
}
