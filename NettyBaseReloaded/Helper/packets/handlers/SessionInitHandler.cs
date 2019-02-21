using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Helper.objects;
using NettyBaseReloaded.Helper.packets.commands;
using NettyBaseReloaded.Helper.packets.requests;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Helper.packets.handlers
{
    class SessionInitHandler
    {
        public static void Execute(DiscordClient client, string[] packet)
        {
            var sessionRequest = new SessionInitRequest();
            sessionRequest.Read(packet);
            var brain = new HelperBrain(client)
                {DiscordId = sessionRequest.DiscordId, DiscordName = sessionRequest.DiscordName};
            HelperBrain._instance = brain;
            var connectedSessions = World.StorageManager.GameSessions;
            List<ConnectedPlayer> playersConnected = new List<ConnectedPlayer>();
            foreach (var session in connectedSessions)
            {
                playersConnected.Add(new ConnectedPlayer(session.Value.Player.Id, session.Value.Player.GlobalId, session.Value.Player.Name));
            }

            HelperBrain.SendCommand(new InitializeSessionCommand(playersConnected));
            Console.WriteLine("BOT [Helper] successfully connected");
        }
    }
}
