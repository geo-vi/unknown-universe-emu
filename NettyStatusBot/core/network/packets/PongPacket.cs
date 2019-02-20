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

        public override void readPacket(string packet)
        {
        }
    }
}
