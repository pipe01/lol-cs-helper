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
        private Timer _WindowSyncTimer = new Timer();

        public frmOverlay()
        {
            InitializeComponent();
        }

        private void frmOverlay_Load(object sender, EventArgs e)
        {

        }
    }
}
