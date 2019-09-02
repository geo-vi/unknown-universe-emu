using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.WebSocks.packets.handlers;

namespace NettyBaseReloaded.WebSocks.packets
{
    class Handler
    {
        public static Dictionary<string, IHandler> HandledPackets = new Dictionary<string, IHandler>();

        public static void AddHandlers()
        {
            HandledPackets.Add("user", new UserHandler());
            HandledPackets.Add("shop", new ShopHandler());
            HandledPackets.Add("clan", new ClanHandler());
            HandledPackets.Add("ext", new ExternalClientHandler());
        }

        public static void Handle(WebSocketReceiver receiver, string packet)
        {
            IHandler handler;
            var finalPacket = packet.Contains("|") ? packet.Split('|') : new[] { packet };
            HandledPackets.TryGetValue(finalPacket[0], out handler);
            handler?.execute(receiver, finalPacket);
            receiver.Send("success");
        }
    }
}
