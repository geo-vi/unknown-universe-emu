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
                    Console.WriteLine(actionParams[0]);
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
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                Out.QuickLog("Trying to send a message to an invalid session", LogKeys.ERROR_LOG);
                return;
            }
            Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "A", "STD", message);
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
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                Out.QuickLog("Trying to send a message to an invalid session", LogKeys.ERROR_LOG);
                return;
            }
            
            Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, 0, "CC", player.CurrentConfig);
        }
    }
}