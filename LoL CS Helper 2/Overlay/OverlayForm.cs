using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoL_CS_Helper_2.Overlay
{
    public class OverlayForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;


        private Configuration _Config;
        private GlobalHooks _Hooks;
        private IntPtr _LauncherHandle;

        public Graphics Surface => this.CreateGraphics();
        
        private const int WM_MOVE = 0x0003;
        private const int WM_ACTIVATE = 0x0006;
    
        public OverlayForm(Configuration config, IntPtr launcherHandle)
        {
            _Config = config;
            _LauncherHandle = launcherHandle;

            _Hooks = new GlobalHooks(this.Handle);
            _Hooks.CallWndProc.CallWndProc += CallWndProc_CallWndProc;
            
            this.Visible = false;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = this.BackColor;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Overlay";

            this.Load += OverlayForm_Load;

            HideTaskbar();
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            _Hooks.CallWndProc.Start();
            this.Refresh();
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
                        bool activated = wParam.ToInt32() == 1;

                        this.Visible = activated;
                        break;
                }
        }

        private void HideTaskbar()
        {
            SetWindowLong(this.Handle, GWL_EX_STYLE, (GetWindowLong(this.Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
        }

        protected override void WndProc(ref Message m)
        {
            _Hooks?.ProcessWindowMessage(ref m);

            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                //Make the window top-most
                var cp = base.CreateParams;
                cp.ExStyle |= 8;  // Turn on WS_EX_TOPMOST
                return cp;
            }
        }
    }
}
