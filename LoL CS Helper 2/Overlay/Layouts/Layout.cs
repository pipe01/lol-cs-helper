using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2.Overlay.Layouts
{
    public abstract class Layout
    {
        private Configuration _Config;
        private Pen _DebugPen = new Pen(Color.Orange, 1);

        public Layout(Configuration config)
        {
            _Config = config;
            LoadRegions();
        }

        /// <summary>
        /// Draw on the specified Graphics
        /// </summary>
        /// <param name="graphics">The surface to draw on.</param>
        /// <param name="containerSize">The size of the layout.</param>
        public void Draw(Graphics graphics, Size containerSize)
        {
            if (_Config.DebugDraw)
            {
                DebugDraw(graphics, containerSize);
            }
        }

        protected IList<Region> Regions { get; } = new List<Region>();

        protected abstract void DrawInner(Graphics g, Size size);
        protected abstract void LoadRegions();

        protected void AddAbsoluteRegion(string name, int x, int y, int w, int h, int totalW, int totalH)
        {
            float tW = totalW;
            float tH = totalH;

            Regions.Add(new Region(name, x / tW, y / tH, w / tW, y / tH));
        }

        private void DebugDraw(Graphics g, Size size)
        {
            lock (g)
            {
                foreach (var item in Regions)
                {
                    var abs = item.GetAbsoluteBounds(size.Width, size.Height);

                    g.DrawRectangle(_DebugPen, abs);
                }

                g.DrawRectangle(_DebugPen, new Rectangle(0, 0, size.Width - 1, size.Height - 1));
            }
        }
    }
}
