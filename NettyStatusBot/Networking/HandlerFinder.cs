using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using NettyStatusBot.Networking.handlers;
using NettyStatusBot.Networking.packets;

namespace NettyStatusBot.Networking
{
    class HandlerFinder
    {
        public static Dictionary<string, IHandler> Handlers = new Dictionary<string, IHandler>();

        public static void AddCommands()
        {
            Handlers.Add("cl", new ChatLogHandler());
        }

        public static void Handle(DiscordSocketClient client, string s)
        {
            if (!Handlers.Any()) AddCommands();
            var packetBase = PacketBase.GetBase(s);
            if (Handlers.ContainsKey(packetBase)) Handlers[packetBase].execute(client, s);
        }
    }
}
