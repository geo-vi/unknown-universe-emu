using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Socketty.handlers;

namespace NettyBaseReloaded.Socketty
{
    class PacketHandler
    {
        public static Dictionary<string, IHandler> HandledPackets = new Dictionary<string, IHandler>();

        public static void AddCommands()
        {
            HandledPackets.Add(Constants.REFRESH_REQUEST, new RefreshRequestHandler());
            HandledPackets.Add(Constants.RESTART, new RestartRequestHandler());
            HandledPackets.Add(Constants.MSG, new MessageRequestHandler());
        }

        public static void Handle(string packet)
        {
            if (packet.Contains('|'))
            {
                var split = packet.Split('|');
                var id = split[0];
                if (HandledPackets.ContainsKey(id)) HandledPackets[id].execute(packet);
            }
            else
            {
                if (HandledPackets.ContainsKey(packet)) HandledPackets[packet].execute(packet);
            }
        }
    }
}
