using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Launcher_Window
{
    /// <summary>
    /// Represents the graphics of the window.
    /// </summary>
    internal class GraphicalWindow
    {
        private VirtualWindow _VirtualWindow;

        /// <summary>
        /// Construct a new <see cref="GraphicalWindow"/>.
        /// </summary>
        /// <param name="virtualWindow">Virtual window that we are going to work with.</param>
        public GraphicalWindow(VirtualWindow virtualWindow)
        {
            _VirtualWindow = virtualWindow;
        }
    }
}
