using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyStatusBot.core.network.packets
{
    abstract class PacketBase
    {
        public abstract string Header { get; }

        public abstract void readPacket(string packet);
    }
}
