namespace LoL_CS_Helper_2
{
    partial class frmOverlay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.watermark = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.watermark)).BeginInit();
            this.SuspendLayout();
            // 
            // watermark
            // 
            this.watermark.Location = new System.Drawing.Point(562, 397);
            this.watermark.Name = "watermark";
            this.watermark.Size = new System.Drawing.Size(100, 50);
            this.watermark.TabIndex = 0;
            this.watermark.TabStop = false;
            // 
            // frmOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.watermark);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmOverlay";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Overlay";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOverlay_FormClosing);
            this.Load += new System.EventHandler(this.frmOverlay_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmOverlay_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmOverlay_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.watermark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox watermark;
    }
}