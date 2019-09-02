using System;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Networking.clients;
using Server.Utils;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltLegacyCommands : PrebuiltCommandBase
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static PrebuiltLegacyCommands Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrebuiltLegacyCommands();
                }
                return _instance;
            }
        }

        private static PrebuiltLegacyCommands _instance;

        /// <summary>
        /// Adding all relevant commands
        /// </summary>
        public override void AddCommands()
        {
            Packet.Builder.OldCommands.Add(Commands.LEGACY_MODULE,
                async (client, actionParams) =>
                {
                    ArgumentFixer(actionParams, 1, out actionParams);
                    await client.Send(commands.old_client.LegacyModule.write((string) actionParams[0]).Bytes);
                });
        }

        /// <summary>
        /// Server message command which will display on player's screen
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public void ServerMessage(Player player, string message)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "STD", message);
            }
        }

        /// <summary>
        /// Global server message displaying on screen
        /// </summary>
        /// <param name="message"></param>
        public void GlobalServerMessage(string message)
        {
            var packet = new object[] 
            {
                "0|A|STD|" + message
            };
            
            Packet.Builder.BuildToAllConnections(Commands.LEGACY_MODULE, packet, packet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void UpdateConfigurations(Player player)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "CC",
                    player.CurrentConfig);
            }
        }

        /// <summary>
        /// Sends all the booty keys u currently own
        /// </summary>
        /// <param name="player"></param>
        public void SendBootyKeys(Player player)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "BK",
                    player.Information.BootyKeys[0]);
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "BKR",
                    player.Information.BootyKeys[1]);
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "BKB",
                    player.Information.BootyKeys[2]);
            }
        }

        public void SendQuickbuyPriceMenu(Player player)
        {
            if (GetSession(player, out var session))
            {

                //~TODO: Create quickbuy packet
                //Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "g", "a", "b,1000,1,10000.0 ");
            }
        }
        
        public void ConfigurationCommand(Player player)
        {
            if (GetSession(player, out var session))
            {
                Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, "0|A|CC|" + player.CurrentConfig); // Config
            }
        }

        public void SendCooldown(Player player, string cooldownType, int totalSeconds)
        {
            
        }
    }
}