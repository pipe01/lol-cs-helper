using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Launcher_Window
{
    /// <summary>
    /// Represents the structure of the window.
    /// </summary>
    internal class VirtualWindow
    {
        /// <summary>
        /// Represents a region in the window. All measurements are on a unit scale.
        /// </summary>
        public class WindowRegion
        {
            /// <summary>
            /// Region name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// X position from 0.0 to 1.0
            /// </summary>
            public double RelativeX { get; set; }

            /// <summary>
            /// Y position from 0.0 to 1.0
            /// </summary>
            public double RelativeY { get; set; }

            /// <summary>
            /// Width from 0.0 to 1.0
            /// </summary>
            public double RelativeWidth { get; set; }

            /// <summary>
            /// Height from 0.0 to 1.0
            /// </summary>
            public double RelativeHeight { get; set; }
        }

        /// <summary>
        /// Collection of window regions. See <see cref="WindowRegion"/>.
        /// </summary>
        public class WindowRegionCollection : ICollection<WindowRegion>
        {
            private List<WindowRegion> InnerList = new List<WindowRegion>();

            public int Count => InnerList.Count;

            public bool IsReadOnly => false;

            public void Add(WindowRegion item) => InnerList.Add(item);

            public void Clear() => InnerList.Clear();

            public bool Remove(WindowRegion item) => InnerList.Remove(item);

            public bool Contains(WindowRegion item) => InnerList.Contains(item);

            public void CopyTo(WindowRegion[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);

            public IEnumerator<WindowRegion> GetEnumerator() => InnerList.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();
        }

        /// <summary>
        /// Region collection.
        /// </summary>
        public WindowRegionCollection Regions { get; private set; } = new WindowRegionCollection();


    }
}
