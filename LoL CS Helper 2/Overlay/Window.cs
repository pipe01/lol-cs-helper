using Helper;
using LoL_CS_Helper_2.Overlay.Layouts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoL_CS_Helper_2.Overlay
{
    public class Window
    {
        private OverlayForm _Overlay;
        private Configuration _Config;
        private Layout _CurrentLayout = null;

        public Size Size => _Overlay.Size;

        public Window(Configuration config)
        {
            _Config = config;

            _Overlay = new OverlayForm(config, DesktopWindow.Handle);
            _Overlay.Paint += _Overlay_Paint;

            SetLayout<LayoutMain>();
        }

        /// <summary>
        /// Show the overlay.
        /// </summary>
        public void Show()
        {
            _Overlay.Show();
        }

        /// <summary>
        /// Hide the overlay.
        /// </summary>
        public void Hide()
        {
            _Overlay.Hide();
        }

        /// <summary>
        /// Set the current layout.
        /// </summary>
        /// <typeparam name="T">The layout type.</typeparam>
        public void SetLayout<T>() where T : Layout
        {
            _CurrentLayout = Activator.CreateInstance(typeof(T), _Config) as T;
            _Overlay.Refresh();
        }

        private async void _Overlay_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.Yellow, 30, 30, 30, 30);

            if (_CurrentLayout != null)
            {
                Bitmap buffer = new Bitmap(Size.Width, Size.Height);
                Graphics g = Graphics.FromImage(buffer);

                await Task.Run(() => _CurrentLayout.Draw(_Overlay.Surface, _Overlay.Size));

                //e.Graphics.DrawImage(buffer, Point.Empty);
            }
        }
    }
}
