using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyStatusBot.core.network.packets
{
    class PongPacket : PacketBase
    {
        public override string Header => "pong";

        public TimeSpan RunningTime;
        public int PlayersOnline;

        public override void readPacket(string packet)
        {
            var splitted = packet.Split('|');
            RunningTime = TimeSpan.Parse(splitted[1]);
            PlayersOnline = int.Parse(splitted[2]);
            Debug.WriteLine($"{RunningTime} + {PlayersOnline}");
        }
    }
}
