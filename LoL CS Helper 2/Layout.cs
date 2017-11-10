using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoL_CS_Helper_2
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

    public class Layout : IEnumerable<Region>
    {
        private List<Region> Regions = new List<Region>();

        public IEnumerator<Region> GetEnumerator() => Regions.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Regions.GetEnumerator();

        public Region this[string key]
        {
            get
            {
                return Regions.Where(o => o.Name.Equals(key)).FirstOrDefault();
            }
        }

        public void AddRegion(string name, float x, float y, float w, float h, float totalW = 1280, float totalH = 720)
        {
            Regions.Add(new Region(name, x / totalW, y / totalH, w / totalW, h / totalH));
        }
    }
}
