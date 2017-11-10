using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Represents the structure of the window.
    /// </summary>
    public class VirtualWindow
    {
        /// <summary>
        /// Collection of window regions. See <see cref="WindowRegion"/>.
        /// </summary>
        public class WindowRegionCollection : ICollection<WindowRegion>
        {
            private List<WindowRegion> InnerList = new List<WindowRegion>();

            public WindowRegion this[string key] => Find(key);

            /// <summary>
            /// Region count.
            /// </summary>
            public int Count => InnerList.Count;

            /// <summary>
            /// Read only?
            /// </summary>
            public bool IsReadOnly => false;

            /// <summary>
            /// Add <see cref="WindowRegion"/> to the collection.
            /// </summary>
            /// <param name="item"></param>
            public void Add(WindowRegion item) => InnerList.Add(item);

            /// <summary>
            /// Clear regions.
            /// </summary>
            public void Clear() => InnerList.Clear();

            /// <summary>
            /// Remove region
            /// </summary>
            /// <param name="item">Region to delete</param>
            public bool Remove(WindowRegion item) => InnerList.Remove(item);

            /// <summary>
            /// Contains region?
            /// </summary>
            /// <param name="item">Region to find</param>
            public bool Contains(WindowRegion item) => InnerList.Contains(item);

            /// <summary>
            /// Contains region with name '<paramref name="key"/>'?
            /// </summary>
            /// <param name="key">Name of the region</param>
            public bool ContainsKey(string key) => InnerList.Any(o => o.Name.Equals(key));

            /// <summary>
            /// Find region by name
            /// </summary>
            /// <param name="key">Region name</param>
            public WindowRegion Find(string key) => InnerList.Where(o => o.Name.Equals(key)).FirstOrDefault();

            /// <summary>
            /// Copy regions to array.
            /// </summary>
            public void CopyTo(WindowRegion[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            /// <summary>
            /// Get enumerator.
            /// </summary>
            public IEnumerator<WindowRegion> GetEnumerator() => InnerList.GetEnumerator();

            /// <summary>
            /// Get enumerator.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();
        }

        /// <summary>
        /// Region collection.
        /// </summary>
        public WindowRegionCollection Regions { get; private set; } = new WindowRegionCollection();

        private GraphicalWindow _GWindow;
        /// <summary>
        /// Get the graphical window associated with this window.
        /// </summary>
        public GraphicalWindow GraphicsWindow => _GWindow ?? (_GWindow = new GraphicalWindow(this));

        /// <summary>
        /// Add default champion regions. If P = picking and N = not picking, the order is NPNPNP...
        /// </summary>
        public void AddChampionRegions()
        {
            //Add all the champion regions
            //I took these measurements from my launcher, so the launcher size is 1280x720

            int squareSize = 60;
            int startY = 104;

            //Left side champion squares
            for (int i = 0; i < 5; i++)
            {
                int y = startY + (i * 80);

                AddChampionRegion(false, false, i, "Ally" + i, 18, y, squareSize, squareSize);
                AddChampionRegion(false, true, i, "AllyChoosing" + i, 56, y, squareSize, squareSize);
            }

            //Right side champion squares
            for (int i = 0; i < 5; i++)
            {
                int y = startY + (i * 80);
                AddChampionRegion(true, false, i, "Enemy" + i, 1202, y, squareSize, squareSize);
                AddChampionRegion(true, true, i, "EnemyChoosing" + i, 1164, y, squareSize, squareSize);
            }

            Regions.Add(WindowRegion.FromAbsolute("ChampSelectTrigger", 1142, 690, 26, 26, 1280, 720));

            void AddChampionRegion(bool enemy, bool choosing, int index, string name, int x, int y, int w, int h,
                int launcherW = 1280, int launcherH = 720)
            {
                var region = WindowRegion.FromAbsolute(name, x, y, w, h, launcherW, launcherH);
                region.RegionData = new ChampionWindowRegionData
                {
                    Index = index,
                    IsChoosing = choosing,
                    Team = enemy ? ChampionTeam.Enemy : ChampionTeam.Ally
                };

                Regions.Add(region);
            }

        }
    }
}
