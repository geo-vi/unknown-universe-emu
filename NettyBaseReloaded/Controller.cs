using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Properties;

namespace NettyBaseReloaded
{
    public partial class Controller : Form
    {
        public Controller()
        {
            InitializeComponent();
            Shown += (sender, args) =>
            {
                ticker.Start();
                Task.Factory.StartNew(Program.InitiateSession);
                if (Properties.Server.CONSOLE_MODE)
                    consoleBox.BackColor = Color.FromArgb(240, 0, 0, 0);
            };
            VisibleChanged += (s, a) =>
            {
                if (!Visible) ticker.Stop();
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AdministratePlayers().Show();
            Hide();
        }

        private void ticker_Tick(object sender, EventArgs e)
        {
            if (Server.CONSOLE_MODE)
                consoleBox.Invalidate();

            if (Server.RUNTIME == DateTime.MinValue)
                timeRunning.Text = "NULL";
            else timeRunning.Text = (DateTime.Now - Server.RUNTIME).ToString(@"dd\.hh\:mm\:ss");
            if (Global.State == State.LOADING) pictureBox1.BackColor = Color.Yellow;
            if (Global.State == State.LOADED) pictureBox1.BackColor = Color.GreenYellow;
            if (Global.State == State.READY) EnableControls();
            onlinePlayers.Text = World.StorageManager.GameSessions.Count.ToString();
        }

        private bool ControlsEnabled = false;
        private void EnableControls()
        {
            if (ControlsEnabled) return;
            foreach (Control control in Controls) control.Enabled = true;
            ControlsEnabled = true;
            pictureBox1.BackColor = Color.Green;
        }

        private void Controller_Load(object sender, EventArgs e)
        {
            foreach (Control control in Controls) control.Enabled = false;
            ticker.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new MapEditor().Show();
            Hide();
        }

        private void Controller_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private string CMD_TXT = "";
        private void PaintConsole(object sender, PaintEventArgs e)
        {
            if (!Server.CONSOLE_MODE) return;

            //var logsFromNewestToOldest = DebugLog.LogsProcessed.ToList();
            //logsFromNewestToOldest.Reverse();
            //if (logsFromNewestToOldest.Count > 15)
            //    logsFromNewestToOldest.RemoveRange(14, logsFromNewestToOldest.Count - 14);
            //logsFromNewestToOldest.Reverse();
            //PointF pos = new PointF(0, 0);
            //foreach (var line in logsFromNewestToOldest)
            //{
            //    e.Graphics.DrawString(line, new Font(FontFamily.GenericMonospace, 7f),
            //        new SolidBrush(Color.GreenYellow), pos);
            //    pos.Y += 8;
            //}

            if (CMD_TXT != "")
            {
                e.Graphics.DrawString(CMD_TXT, new Font(FontFamily.GenericMonospace, 7f),
                    new SolidBrush(Color.White), new PointF(0, e.ClipRectangle.Bottom - 10));
            }
        }

        private void serverEditor_Click(object sender, EventArgs e)
        {
            new ServerEditor().Show();
            Hide();
        }

        private void Controller_KeyPress(object sender, KeyPressEventArgs e)
        {
            CMD_TXT += e.KeyChar.ToString();
        }

        private void Controller_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                CMD_TXT = "";
            if (e.KeyCode == Keys.Back)
            {
                CMD_TXT = CMD_TXT.Remove(CMD_TXT.Length - 1, 1);
            }
            if (e.KeyCode == Keys.Enter)
            {
                //TODO: Execute command
                //if (CMD_TXT == "clean" || CMD_TXT == "clear") DebugLog.LogsProcessed.Clear();
                //CMD_TXT = "";
            }
        }
    }
}
