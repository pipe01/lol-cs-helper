using Helper;
using Helper.Counters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace LoL_CS_Helper_2
{
    public partial class frmOverlay : Form
    {
        private Timer _WindowSyncTimer, _RefreshTimer;
        private Configuration _Config;
        private bool _Focused, _HookRunning, _Foreground;
        private Dictionary<string, Layout> _Layouts = new Dictionary<string, Layout>();
        private string _CurrentLayout = "Main";
        private string[][] _Counters = new string[5][];
        private MatchupProvider _MatchupProvider = new ProviderLolCounter();
        private GlobalHooks _Hooks;
        private IntPtr _LauncherHandle = IntPtr.Zero;

        private const int WM_MOVE = 0x0003;
        private const int WM_SETFOCUS = 0x0007;
        private const int WM_KILLFOCUS = 0x0008;
        private const int WM_ACTIVATE = 0x0006;

        public frmOverlay(Configuration config)
        {
            InitializeComponent();
            
            _Config = config;
            _Hooks = new GlobalHooks(this.Handle);

            _WindowSyncTimer = new Timer();
            _WindowSyncTimer.Interval = _Config.WindowSyncInterval;
            _WindowSyncTimer.Elapsed += WindowSync;
            
            _RefreshTimer = new Timer();
            _RefreshTimer.Interval = _Config.MinimumRefreshInterval;
            _RefreshTimer.Elapsed += _RefreshTimer_Elapsed;
            _RefreshTimer.AutoReset = false;
            
            Application.ApplicationExit += (a, b) => StopHooks();

            AddRegions();

            WindowSync(this, null);
            
            this.Hide();
        }

        private void StartHooks()
        {
            _HookRunning = true;

            _Hooks.CallWndProc.CallWndProc += CallWndProc_CallWndProc;
            _Hooks.CallWndProc.Start();
        }

        private void StopHooks()
        {
            _HookRunning = false;
            _Hooks.CallWndProc.Stop();
        }

        private void CallWndProc_CallWndProc(IntPtr Handle, IntPtr Message, IntPtr wParam, IntPtr lParam)
        {
            int msg = Message.ToInt32();

            if (Handle == _LauncherHandle)
                switch (msg)
                {
                    case WM_MOVE:
                        IntPtr xy = lParam;
                        int x = unchecked((short)(long)xy);
                        int y = unchecked((short)((long)xy >> 16));

                        this.Location = new Point(x, y);
                        break;
                    case WM_ACTIVATE:
                        _Foreground = wParam.ToInt32() == 1;
                        this.Refresh();
                        break;
                }
        }

        protected override void WndProc(ref Message m)
        {
            // This lets the GlobalHooks class check the message queue for this window to see if it's received
            // any hook messages.
            if (_HookRunning)
                _Hooks.ProcessWindowMessage(ref m);

            base.WndProc(ref m);
        }

        private void _RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            new Thread(async () =>
            {
                if (DesktopWindow.IsForeground)
                {
                    var champions = await Analyser.GetAllChampions();
                    champions = champions.Skip(5).ToArray();

                    for (int i = 0; i < champions.Length; i++)
                    {
                        string item = champions[i];

                        if (item == "Empty" || item == "None")
                        {
                            _Counters[i] = new string[0];
                            continue;
                        }

                        var counters = await _MatchupProvider.GetMatchupsForChampionAsync(item);

                        _Counters[i] = counters
                            .Where(o => o.Type == MatchupProvider.Matchup.MatchupType.WeakAgainst)
                            .Select(o => o.Against)
                            .ToArray();
                    }
                }

                _RefreshTimer.Start();
            }).Start();
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

            layout.AddRegion("watermark", 1143, 705, 75, 12);

            _Layouts.Add("Main", layout);

            SetLayout("Main");
        }

        private void SetLayout(string name)
        {
            _CurrentLayout = name;
            this.Refresh();

            if (_CurrentLayout == "ChampSelect")
                _RefreshTimer.Start();
            else
                _RefreshTimer.Stop();
        }

        private void WindowSync(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DesktopWindow.IsOpen)
                _LauncherHandle = DesktopWindow.Handle;
            else
                _LauncherHandle = IntPtr.Zero;

            if (DesktopWindow.IsForeground)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.TopMost = false;
                    this.TopMost = true;
                }));
                Analyser.Window.GraphicsWindow.RefreshWindowPicture();
            }

            bool champSelect = Analyser.Window.GraphicsWindow.IsOnChampSelect();

            try
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Location = DesktopWindow.Bounds.Location;
                    this.Size = DesktopWindow.Bounds.Size;
                    this.Refresh();

                    if (DesktopWindow.IsForeground)
                        if (champSelect && _CurrentLayout != "ChampSelect")
                            SetLayout("ChampSelect");
                        else if (!champSelect && _CurrentLayout == "ChampSelect")
                            SetLayout("Main");
                }));
            }
            catch (InvalidOperationException) { }
        }

        private async void frmOverlay_Paint(object sender, PaintEventArgs e)
        {
            if (!_Foreground && !_Focused)
            {
                _Focused = false;
                return;
            }

            if (_Focused)
                _Focused = false;


            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            var pen = new Pen(Color.Orange, 1);

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
                    Font font = new Font("Courier New", abs.Height, GraphicsUnit.Pixel);
                    Color clr = Color.DarkOrange;

                    e.Graphics.DrawString("Pipe's helper", font, new SolidBrush(clr), abs.Location);

                    font.Dispose();
                }

                if (item.Name.StartsWith("counter"))
                {
                    int index = int.Parse(item.Name.Last().ToString());

                    Bitmap image = await BuildCountersImage(_Counters[index], abs.Height);
                    e.Graphics.DrawImage(image, abs.Location);
                }
            }

            pen.Dispose();
        }

        private async Task<Bitmap> BuildCountersImage(string[] counters, int height)
        {

            int maxCounters = 3;
            maxCounters = counters == null ? maxCounters : Math.Min(counters.Length, maxCounters);

            if (counters != null && counters.Length == 0)
                return new Bitmap(1, 1);

            Bitmap ret = new Bitmap(height * maxCounters, height);
            Graphics g = Graphics.FromImage(ret);
            
            if (counters == null)
            {
                g.DrawString(
                    "Loading...",
                    new Font("Arial", 12),
                    Brushes.LightYellow,
                    new RectangleF(0, 0, ret.Width, ret.Height),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
                    });
                return ret;
            }

            counters = counters
                .Select(o => o
                    .Replace("Rek'sai", "RekSai")
                    .Replace("LeBlanc", "Leblanc")
                    .Replace("Kog'Maw", "KogMaw")
                    .Replace("Wukong", "MonkeyKing"))
                .Take(maxCounters)
                .Reverse()
                .ToArray();

            //Get the counters' images
            Dictionary<string, Image> counterImages = (await Riot.GetChampionImagesAsync(height))
                .Where(o => counters.Any(a => a.Equals(o.Key)))
                .ToDictionary(o => o.Key, o => o.Value);

            for (int i = 0; i < maxCounters; i++)
            {
                Image counter = counterImages[counters[i]];
                g.DrawImage(counter, new Rectangle(i * height, 0, height, height));
            }

            return ret;
        }

        private void frmOverlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopHooks();
        }

        private void frmOverlay_MouseDown(object sender, MouseEventArgs e)
        {
            _Focused = true;
            DesktopWindow.BringToForeground();
        }

        private void frmOverlay_Load(object sender, EventArgs e)
        {
            _WindowSyncTimer.Start();
            
            StartHooks();
        }
    }
}
