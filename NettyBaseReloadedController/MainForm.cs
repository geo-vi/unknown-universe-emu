using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using global::NettyBaseReloadedController.Main.global;
using NettyBaseReloadedController.Main;
using NettyBaseReloadedController.Main.global.interfaces;
using NettyBaseReloadedController.Main.global.objects;
using NettyBaseReloadedController.Main.netty.commands;
using NettyBaseReloadedController.Networking;

namespace NettyBaseReloadedController
{
    public partial class MainForm : Form, ITick
    {
        private ToolStripDropDown ViewDropdown { get; }

        public int PageId { get; set; }
        private int MapId { get; set; }

        private int ViewMode { get; set; }

        private ControllerClient Client { get; set; }

        public MainForm()
        {
            Program.mainForm = this;

            InitializeComponent();
            InitiateDelegates();
            Initiate();

            ViewMode = 0;
            PageId = 0;
            MapId = 1;
            ViewDropdown = viewToolStripMenuItem.DropDown;
            statisticsOnlyToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;
            mapStatsToolStripMenuItem.Enabled = false;

            InitiatePage();

            Controller.Initiate();

            InitiateConnection();

            Controller.Global.TickManager.Tickables.Add(this);

            Write.ToConsole("test");
        }

        private void Initiate()
        {
            pictureBox1.BackColor = Color.Black;
            logBox.Hide();
            chatBox.Hide();
            chatTextBox.Hide();
            chatInfo.Hide();
            viewInfo.Text = "null";
            chatInfo.Text = "null";
        }

        private void InitiateConnection()
        {
            var socket = new Client("164.132.4.31").XSocket;
            Client = new ControllerClient(socket);
            Client.Send(LoginRequest.write("test", "test"));
        }

        private void InitiateDelegates()
        {
            toolStripMenuItem4.Click += (s, e) => ChangeMap(1);
            toolStripMenuItem5.Click += (s, e) => ChangeMap(2);
            toolStripMenuItem13.Click += (sender, args) => ChangeMap(9);
        }

        public void Tick()
        {
            if (Controller.Global.STATE != Global.STATE_LOADED)
            {
                pictureBoxText = "LOADING...";
            }
            else
            {
                DrawOnPicture();
            }
            pictureBox1.Refresh();
            UpdatePing();
        }

        private DateTime LastPingCheck = new DateTime(2016, 12, 24, 0,0,0);
        private void UpdatePing()
        {
            if (LastPingCheck.AddSeconds(1) < DateTime.Now)
            {
                var ping = new Ping();
                PingReply reply = ping.Send("164.132.4.31");
                pingLabel.Text = reply.RoundtripTime + " ms";
                if (reply.RoundtripTime > 500)
                    pingProgressBar.Value = 500;
                else
                    pingProgressBar.Value = (int) reply.RoundtripTime;

                LastPingCheck = DateTime.Now;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {

        }

        private void mapOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Hide();
            pictureBox1.Size = new Size(624, 384);
            ViewMode = 1;
            pictureBox1.Show();
            viewInfo.Hide();
            viewConsole.Hide();
        }

        private void statisticsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Hide();
        }

        private void mapStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Hide();
            pictureBox1.Size = new Size(416, 256);
            ViewMode = 0;
            pictureBox1.Show();
            viewInfo.Show();
            viewConsole.Show();
        }

        private string pictureBoxText = "0-1";
        private void DrawOnPicture()
        {
            switch (MapId)
            {
                case 0:
                    pictureBoxText = "0-1";
                    break;
                case 1:
                    pictureBoxText = "1-1";
                    break;
                case 2:
                    pictureBoxText = "1-2";
                    break;
                case 9:
                    pictureBoxText = "3-1";
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(pictureBoxText, myFont, Brushes.White, new Point(pictureBox1.Size.Width / 2 - (pictureBoxText.Length * 6), pictureBox1.Size.Height / 2 - 14));
            }

            var multiplySize = 0.00;

            if (ViewMode == 1)
                multiplySize = 0.03;
            else multiplySize = 0.02;

            foreach (var entity in Controller.Global.StorageManager.Spacemaps[MapId].Entities)
            {
                if (entity.Value is Player)
                {
                    var player = (Player) entity.Value;
                    Rectangle ee = new Rectangle((int)(player.Position.X * multiplySize), (int)(player.Position.Y * multiplySize), 1, 1);

                    var color = Color.Gray;

                    switch (player.FactionId)
                    {
                        case 1:
                            color = Color.DarkRed;
                            break;
                        case 2:
                            color = Color.CornflowerBlue;
                            break;
                        case 3:
                            color = Color.ForestGreen;
                            break;
                    }

                    using (Pen pen = new Pen(color, 2))
                    {
                        e.Graphics.DrawRectangle(pen, ee);
                    }
                }
                else
                {
                    Rectangle ee = new Rectangle((int)(entity.Value.Position.X * multiplySize), (int)(entity.Value.Position.Y * multiplySize), 1, 1);
                    using (Pen pen = new Pen(Color.IndianRed, 2))
                    {
                        e.Graphics.DrawRectangle(pen, ee);
                    }
                }
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePage(1);
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePage(0);
        }

        const int MAIN_PAGE = 0;
        const int SETTINGS_PAGE = 1;
        const int CHAT_PAGE = 2;
        const int LOG_PAGE = 3;
        
        private void ChangePage(int pageId)
        {
            HidePage();
            ShowPage(pageId);
        }

        private void HidePage()
        {
            switch (PageId)
            {
                case MAIN_PAGE:
                    pictureBox1.Hide();
                    viewInfo.Hide();
                    viewConsole.Hide();
                    break;
                case CHAT_PAGE:
                    chatBox.Hide();
                    chatTextBox.Hide();
                    chatInfo.Hide();
                    break;
                case LOG_PAGE:
                    logBox.Hide();
                    break;
            }
        }

        private void ShowPage(int pageId)
        {
            switch (pageId)
            {
                case MAIN_PAGE:
                    mapStatsToolStripMenuItem_Click(this, EventArgs.Empty);
                    break;
                case SETTINGS_PAGE:
                    break;
                case CHAT_PAGE:
                    chatBox.Show();
                    chatTextBox.Show();
                    chatInfo.Show();
                    break;
                case LOG_PAGE:
                    logBox.Show();
                    break;
            }
            PageId = pageId;
            InitiatePage();
        }

        private void InitiatePage()
        {
            switch (PageId)
            {
                case MAIN_PAGE:
                    viewToolStripMenuItem.DropDown = ViewDropdown;
                    mapStatsToolStripMenuItem_Click(this, EventArgs.Empty);
                    break;
                case SETTINGS_PAGE:
                    viewToolStripMenuItem.DropDown = new ToolStripDropDown();
                    break;
            }
        }

        private void chatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePage(2);
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePage(3);
        }

        public void ChangeMap(int mapId)
        {
            Controller.Global.StorageManager.Spacemaps[MapId].Entities.Clear();
            MapId = mapId;
            Client?.Send(MapChangeRequest.write(mapId));
        }

        public void WriteToConsole(string text, Color color)
        {
            try
            {
                //Debug.WriteLine("dasdasdasdasdasdsadasdasdasdad");
                viewConsole.AppendText(text);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        public void WriteToLog(string text)
        {
            logBox.AppendText(text);
        }

        public void WriteToChat(string text, Color color)
        {
            chatBox.AppendText(text, color);
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
