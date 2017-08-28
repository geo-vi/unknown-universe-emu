using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NettyBaseReloadedBrowser
{
    public partial class Overlay : Form
    {
        public Overlay(Form tocover)
        {
            InitializeComponent();
            this.Opacity = 0.65;      // Tweak as desired
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.None;
            this.Location = tocover.PointToScreen(Point.Empty);
            this.ClientSize = tocover.ClientSize;
            tocover.LocationChanged += Cover_LocationChanged;
            tocover.ClientSizeChanged += Cover_ClientSizeChanged;
            this.Show(tocover);
            tocover.Focus();
            // Disable Aero transitions, the plexiglass gets too visible
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(tocover.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
            label3.ForeColor = Color.GreenYellow;
            label3.Text = "Currently on 3-2";
            label5.ForeColor = Color.GreenYellow;
            label5.Text = "general_Rejection";
            Loop();
        }

        private void Cover_LocationChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass follows the owner
            this.Location = this.Owner.PointToScreen(Point.Empty);
        }
        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            // Ensure the plexiglass keeps the owner covered
            this.ClientSize = this.Owner.ClientSize;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Restore owner
            this.Owner.LocationChanged -= Cover_LocationChanged;
            this.Owner.ClientSizeChanged -= Cover_ClientSizeChanged;
            if (!this.Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
            base.OnFormClosing(e);
        }
        protected override void OnActivated(EventArgs e)
        {
            // Always keep the owner activated instead
            this.BeginInvoke(new Action(() => this.Owner.Activate()));
        }
        private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        private async void Loop()
        {
            while (Visible)
            {
                dateLabel.Text = DateTime.Now.ToShortDateString();
                hourLabel.Text = DateTime.Now.ToLongTimeString();
                await Task.Delay(1000);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            progressBar1.Show();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var avatar = new Bitmap(Directory.GetCurrentDirectory() + "/res/avatar.jpg");
            e.Graphics.DrawImage(avatar, 0, 0, pictureBox1.Width, pictureBox1.Height);
            ControlPaint.DrawBorder(e.Graphics, pictureBox1.ClientRectangle, Color.GreenYellow, ButtonBorderStyle.Solid);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void MouseEnter(object sender, EventArgs e)
        {
            if (sender == panel8)
                panel8.BackColor = Color.FromArgb(57, 155, 140);
            if (sender == panel7)
                panel7.BackColor = Color.FromArgb(57, 155, 140);
            if (sender == panel6)
                panel6.BackColor = Color.FromArgb(57, 155, 140);
        }

        private void MouseLeave(object sender, EventArgs e)
        {
            if (sender == panel8)
                panel8.BackColor = Color.FromArgb(75, 75, 75);
            if (sender == panel7)
                panel7.BackColor = Color.FromArgb(75, 75, 75);
            if (sender == panel6)
                panel6.BackColor = Color.FromArgb(118, 231, 180);

        }

        private void panel6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("News");
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Changelog");

        }

        private void panel8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Settings");
            progressBar1.Show();

        }
    }
}
