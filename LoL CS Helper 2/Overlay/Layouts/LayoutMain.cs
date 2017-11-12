using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2.Overlay.Layouts
{
    public class LayoutMain : Layout
    {
        public LayoutMain(Configuration config) : base(config) { }

        protected override void DrawInner(Graphics g, Size size)
        {
            
        }

        protected override void LoadRegions()
        {
            AddAbsoluteRegion("test", 50, 50, 50, 50, 1280, 720);
        }
    }
}
