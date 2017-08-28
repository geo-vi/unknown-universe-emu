using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NettyBackupCreator.Networking
{
    class DownloaderClient
    {
        private XSocket XSocket;
        public string link { get; set; }
        public DownloaderClient(string ip, int port)
        {
            var client = new XSocket(ip, port);
            client.OnReceive += XSocketOnOnReceive;
            XSocket = client;
            XSocket.Connect();
            XSocket.Write("BUF");
            XSocket.Read(true);
        }

        private void XSocketOnOnReceive(object sender, EventArgs eventArgs)
        {
            var packet = (StringArgs) eventArgs;
            var location = packet.Packet;
            link = location;
        }
    }
}
