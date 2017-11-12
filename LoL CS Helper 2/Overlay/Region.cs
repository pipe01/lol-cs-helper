using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2.Overlay
{
    public class Region
    {
        public string Name { get; private set; }

        public RectangleF Bounds { get; private set; }
        public PointF Location => Bounds.Location;
        public SizeF Size => Bounds.Size;

        public float X => Location.X;
        public float Y => Location.Y;

        public float Width => Size.Width;
        public float Height => Size.Height;

        public Region(string name, float x, float y, float w, float h)
        {
            this.Name = name;
            this.Bounds = new RectangleF(x, y, w, h);
        }

        public Rectangle GetAbsoluteBounds(int totalW, int totalH)
        {
            return new Rectangle(
                (int)(X * totalW),
                (int)(Y * totalH),
                (int)(Width * totalW),
                (int)(Height * totalH));
        }
    }
}
