using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;

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
            Task.Factory.StartNew(CheckingLoop);
        }

        private void XSocketOnOnReceive(object sender, EventArgs e)
        {
            var packetArgs = (StringArgs)e;
            if (packetArgs.Packet == "ping")
            {
                XSocket.Write($"pong|{World.StorageManager.GameSessions.Count}|{(DateTime.Now - Properties.Server.RUNTIME).TotalHours}");
            }
            //Socketty.PacketHandler.Handle(packetArgs.Packet);
        }

        private async Task CheckingLoop()
        {
            int lastPlayerCount = 0;
            DateTime lastSentTime = new DateTime();
            while (true)
            {
                var playerCount = World.StorageManager.GameSessions.Count;
                if (playerCount != lastPlayerCount || lastSentTime.AddMinutes(5) <= DateTime.Now)
                {
                    XSocket.Write($"pong|{playerCount}|{(DateTime.Now - Properties.Server.RUNTIME).TotalHours}");
                    lastSentTime = DateTime.Now;
                    lastPlayerCount = playerCount;
                }
                await Task.Delay(500);
            }
        }
    }
}
