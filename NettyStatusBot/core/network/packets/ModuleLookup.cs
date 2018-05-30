using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyStatusBot.core.network.packets
{
    class ModuleLookup
    {
        public static event EventHandler<PacketBase> PacketReceived;

        private static Dictionary<string, PacketBase> Packets = new Dictionary<string, PacketBase>
        {
            {"pong", new PongPacket()}
        };

        public static void Find(string packet)
        {
            PacketBase packetBase = null;
            Debug.WriteLine(packet);
            if (packet.Contains('|'))
            {
                if (Packets.TryGetValue(packet.Split('|')[0], out packetBase))
                {
                    packetBase.readPacket(packet);
                }
            }
            else
            {
                if (Packets.TryGetValue(packet, out packetBase))
                {
                    packetBase.readPacket(packet);
                }
            }
            PacketReceived?.Invoke(null, packetBase);
        }
    }
}
