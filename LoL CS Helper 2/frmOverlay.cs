using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace LoL_CS_Helper_2
{
    public partial class frmOverlay : Form
    {
        private Timer _WindowSyncTimer, _RefreshTimer;
        private Configuration _Config;
        private bool _Focused = false;
        private Dictionary<string, Layout> _Layouts = new Dictionary<string, Layout>();
        private string _CurrentLayout = "Main";

        public frmOverlay(Configuration config)
        {
            InitializeComponent();

            _Config = config;

            _WindowSyncTimer = new Timer();
            _WindowSyncTimer.Interval = _Config.WindowSyncInterval;
            _WindowSyncTimer.Elapsed += _WindowSyncTimer_Elapsed;
            
            _RefreshTimer = new Timer();
            _RefreshTimer.Interval = _Config.MinimumRefreshInterval;

            AddRegions();
        }

        private void AddRegions()
        {
            int startY = 141;

            var layout = new Layout();

            for (int i = 0; i < 5; i++)
            {
                int y = startY + (i * 80);
                layout.AddRegion("counters" + i, 1047, y, 150, 30);
            }

            _Layouts.Add("ChampSelect", layout);


            layout = new Layout();

            layout.AddRegion("watermark", 1143, 705, 71, 9);

            _Layouts.Add("Main", layout);

            SetLayout("ChampSelect");
        }

        private void SetLayout(string name)
        {
            _CurrentLayout = name;
            this.Refresh();
        }

        private void _WindowSyncTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Analyser.Window.GraphicsWindow.RefreshWindowPicture();

            bool champSelect = Analyser.Window.GraphicsWindow.IsOnChampSelect();

            try
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Location = DesktopWindow.Bounds.Location;
                    this.Size = DesktopWindow.Bounds.Size;
                    this.Refresh();

                    if (champSelect && _CurrentLayout != "ChampSelect")
                        SetLayout("ChampSelect");
                    else if (!champSelect && _CurrentLayout == "ChampSelect")
                        SetLayout("Main");
                }));
            }
            catch (InvalidOperationException) { }
        }

        private void frmOverlay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmOverlay_Paint(object sender, PaintEventArgs e)
        {
            if (!DesktopWindow.IsForeground && !_Focused)
            {
                _Focused = false;
                return;
            }

            if (_Focused)
                _Focused = false;


            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            var pen = new Pen(Color.Orange, 3);

            if (_Config.DebugDraw)
                e.Graphics.DrawRectangle(pen, new Rectangle(Point.Empty, this.Size));

            foreach (var item in _Layouts[_CurrentLayout])
            {
                var abs = item.GetAbsoluteBounds(this.Width, this.Height);

                if (_Config.DebugDraw)
                {
                    e.Graphics.DrawRectangle(pen, abs);
                    e.Graphics.DrawString(_CurrentLayout, new Font("Arial", 12), Brushes.Yellow, PointF.Empty);
                }

                if (item.Name == "watermark")
                {
                    Font font = new Font("Courier New", abs.Height + 3, GraphicsUnit.Pixel);
                    Color clr = Color.DarkOrange;

                    e.Graphics.DrawString("Pipe's helper", font, new SolidBrush(clr), abs.Location);

                    font.Dispose();
                }
            }

            pen.Dispose();
        }

        private void frmOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            _Focused = true;
            DesktopWindow.BringToForeground();
        }

        private void frmOverlay_Load(object sender, EventArgs e)
        {
            _WindowSyncTimer.Start();

            DesktopWindow.BringToForeground();
        }
    }
}
