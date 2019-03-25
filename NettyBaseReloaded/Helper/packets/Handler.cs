using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.packets.handlers;
using NettyBaseReloaded.Helper.packets.requests;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Helper.packets
{
    class Handler
    {
        private readonly Dictionary<string, IHandler> HandledCommands = new Dictionary<string, IHandler>();

        public Handler()
        {
            LoadCommands();
        }

        public void LoadCommands()
        {
            HandledCommands.Add(PingRequest.Prefix, new PingHandler());
            HandledCommands.Add(KillRequest.Prefix, new KillHandler());
            HandledCommands.Add(DisconnectGameRequest.Prefix, new DisconnectGameHandler());
            HandledCommands.Add(DiscordChannelMessageRequest.Prefix, new DiscordChannelMessageHandler());
        }

        public void Handle(DiscordClient client, string packet)
        {
            string prefix;
            string[] arrayOfPacket;
            if (packet.Contains("|"))
            {
                arrayOfPacket = packet.Split('|');
                prefix = arrayOfPacket[0];
            }
            else
            {
                prefix = packet;
                arrayOfPacket = new[] {prefix};
            }

            if (HelperBrain._instance != null && prefix != "" && HandledCommands.ContainsKey(prefix))
            {
                HandledCommands[prefix].Execute(HelperBrain._instance, arrayOfPacket);
            }
            else if (prefix == SessionInitRequest.Prefix)
            {
                SessionInitHandler.Execute(client, arrayOfPacket);
            }
        }
    }
}
