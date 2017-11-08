using System;
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
    }
}
