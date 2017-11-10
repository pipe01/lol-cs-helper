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

        public frmOverlay(Configuration config)
        {
            InitializeComponent();

            _Config = config;

            _WindowSyncTimer = new Timer();
            _WindowSyncTimer.Interval = _Config.WindowSyncInterval;
            _WindowSyncTimer.Elapsed += _WindowSyncTimer_Elapsed;
            
            _RefreshTimer = new Timer();
            _RefreshTimer.Interval = _Config.MinimumRefreshInterval;
        }

        private void _WindowSyncTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Location = DesktopWindow.Bounds.Location;
                    this.Size = DesktopWindow.Bounds.Size;
                    this.Refresh();
                }));
            }
            catch (InvalidOperationException)
            {

            }
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

            e.Graphics.DrawRectangle(new Pen(Color.Orange, 3), new Rectangle(Point.Empty, this.Size));
        }

        private void frmOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            _Focused = true;
            DesktopWindow.BringToForeground();
        }

        private void frmOverlay_Load(object sender, EventArgs e)
        {
            _WindowSyncTimer.Start();
        }
    }
}
