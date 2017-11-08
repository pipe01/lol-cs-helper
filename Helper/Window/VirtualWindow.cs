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
        /// Represents a region in the window. All measurements are on a unit scale.
        /// </summary>
        public struct WindowRegion
        {
            public WindowRegion(string name, double relx, double rely, double relw, double relh)
            {
                this.Name = name;
                this.RelativeX = relx;
                this.RelativeY = rely;
                this.RelativeWidth = relw;
                this.RelativeHeight = relh;
            }

            /// <summary>
            /// Region name.
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// X position from 0.0 to 1.0.
            /// </summary>
            public double RelativeX { get; private set; }

            /// <summary>
            /// Y position from 0.0 to 1.0.
            /// </summary>
            public double RelativeY { get; private set; }

            /// <summary>
            /// Width from 0.0 to 1.0.
            /// </summary>
            public double RelativeWidth { get; private set; }

            /// <summary>
            /// Height from 0.0 to 1.0.
            /// </summary>
            public double RelativeHeight { get; private set; }

            /// <summary>
            /// Get an equivalent absolute rectangle for the specified width and height
            /// </summary>
            /// <param name="width">New absolute width</param>
            /// <param name="height">New absolute height</param>
            public Rectangle GetAbsoluteRect(int width, int height)
            {
                return new Rectangle(
                    (int)(RelativeX * width),
                    (int)(RelativeY * height),
                    (int)(RelativeWidth * width),
                    (int)(RelativeHeight * height));
            }

            /// <summary>
            /// Create <see cref="WindowRegion"/> from absolute measurements.
            /// </summary>
            /// <param name="name">Region name.</param>
            /// <param name="x">Absolute X.</param>
            /// <param name="y">Absolute Y.</param>
            /// <param name="w">Absolute width.</param>
            /// <param name="h">Absolute height.</param>
            /// <param name="windowW">Width of the window that the measurements belong to.</param>
            /// <param name="windowH">It's height.</param>
            public static WindowRegion FromAbsolute(string name, int x, int y, int w, int h, int windowW, int windowH)
            {
                double rx = (double)x / windowW;
                double ry = (double)y / windowH;
                double rw = (double)w / windowW;
                double rh = (double)h / windowH;

                return new WindowRegion(name, rx, ry, rw, rh);
            }
        }

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
    }
}
