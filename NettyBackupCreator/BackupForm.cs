using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBackupCreator.Networking;

namespace NettyBackupCreator
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            label1.Hide();
            progressBar1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Show();
            label1.Text = "Initiating connection...";
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            TcpListener();
        }

        public void TcpListener()
        {
            var downloader = new DownloaderClient("164.132.4.31", 1337);
            while (downloader.link == "")
                Thread.Sleep(10);

            //BeginWebDownload(downloader.link);
            Debug.WriteLine(downloader.link);
        }

        public void BeginWebDownload(string link)
        {
            progressBar1.Show();
            button1.Hide();
            button2.Hide();
            button3.Hide();
            WebClient client = new WebClient();
            client.DownloadFile(link, "backup");
            client.DownloadProgressChanged += delegate(object sender, DownloadProgressChangedEventArgs args)
            {
                progressBar1.Value += args.ProgressPercentage;
            };
            progressBar1.Hide();
            button1.Show();
            button2.Show();
            button3.Show();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }
    }
}
