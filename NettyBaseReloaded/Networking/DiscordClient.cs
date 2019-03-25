using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Helper;

namespace NettyBaseReloaded.Networking
{
    class DiscordClient
    {
        private XSocket XSocket;

        public DiscordClient(XSocket gameSocket)
        {
            XSocket = gameSocket;
            XSocket.OnReceive += XSocketOnOnReceive;
            XSocket.Read(true);
        }

        private void XSocketOnOnReceive(object sender, EventArgs e)
        {
            var packetArgs = (StringArgs)e;
            HelperBrain.Handler.Handle(this, packetArgs.Packet);
        }

        public void Write(string message)
        {
            try
            {
                XSocket.Write(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Error writing to BOT (Helper)");
            }
        }
    }
}
