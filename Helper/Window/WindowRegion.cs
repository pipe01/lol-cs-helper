using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// Represents a region in the window. All measurements are on a unit scale.
    /// </summary>
    public class WindowRegion
    {
        public WindowRegion(string name, double relx, double rely, double relw, double relh)
        {
            this.Name = name;
            this.RelativeX = relx;
            this.RelativeY = rely;
            this.RelativeWidth = relw;
            this.RelativeHeight = relh;
            this.RegionData = null;
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
        /// Custom data for this region.
        /// </summary>
        public IWindowRegionData RegionData { get; set; }

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
    /// Represents window region data.
    /// </summary>
    public interface IWindowRegionData { }

    public enum ChampionTeam
    {
        Ally,
        Enemy
    }

    /// <summary>
    /// Represents champion data for regions.
    /// </summary>
    public class ChampionWindowRegionData : IWindowRegionData
    {
        /// <summary>
        /// Champion team.
        /// </summary>
        public ChampionTeam Team { get; set; }

        /// <summary>
        /// Champion index on the team.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Whether the summoner is still deciding this champ.
        /// </summary>
        public bool IsChoosing { get; set; }
    }
}
