using System;
using System.Collections.Generic;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.netty.commands.old_client;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltRangeCommands : PrebuiltCommandBase
    {
        public static PrebuiltRangeCommands Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrebuiltRangeCommands();
                }
                return _instance;
            }
        }

        private static PrebuiltRangeCommands _instance;
        
        public override void AddCommands()
        {
            Packet.Builder.OldCommands.Add(Commands.SHIP_CREATE_COMMAND, async (client, actionParams) =>
                {
                    ArgumentFixer(actionParams, 19, out actionParams);
                    
                    var clanRelationModule = actionParams[11] as ClanRelationModule ?? new ClanRelationModule(commands.new_client.ClanRelationModule.NONE);
                    
                    var visualModifier = actionParams[11] as List<VisualModifierCommand> ?? new List<VisualModifierCommand>();

                    await client.Send(commands.old_client.ShipCreateCommand.write(Convert.ToInt32(actionParams[0]), Convert.ToInt32(actionParams[1]),
                        Convert.ToInt32(actionParams[2]), Convert.ToString(actionParams[3]), Convert.ToString(actionParams[4]),
                        Convert.ToInt32(actionParams[5]), Convert.ToInt32(actionParams[6]), Convert.ToInt32(actionParams[7]),
                        Convert.ToInt32(actionParams[8]), Convert.ToInt32(actionParams[9]), 
                        Convert.ToBoolean(actionParams[10]), clanRelationModule, Convert.ToInt32(actionParams[12]),
                        Convert.ToBoolean(actionParams[13]), Convert.ToBoolean(actionParams[14]), Convert.ToBoolean(actionParams[15]),
                        Convert.ToInt32(actionParams[16]), Convert.ToInt32(actionParams[17]), visualModifier).Bytes);
                });
            Packet.Builder.OldCommands.Add(Commands.SHIP_REMOVE_COMMAND, async (client, actionParams) =>
                {
                    ArgumentFixer(actionParams, 1, out actionParams);
                    await client.Send(commands.old_client.ShipRemoveCommand.write(Convert.ToInt32(actionParams[0])).Bytes);
                });
        }

        public void CreateShipCommand(Player player, Character targetShip)
        {
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                Out.QuickLog("Bugged player session, session cannot be found, Player ID: " + player.Id, LogKeys.ERROR_LOG);
                return;
            }

            if (targetShip is Player playerTargetShip)
            {
                Packet.Builder.BuildCommand(session.GameClient, Commands.SHIP_CREATE_COMMAND, player.UsingNewClient,
                    targetShip.Id, targetShip.Hangar.ShipDesign.Id, playerTargetShip.GetCurrentConfiguration().ExpansionStage,
                    playerTargetShip.Clan.Tag, playerTargetShip.Name, playerTargetShip.Position.X, playerTargetShip.Position.Y,
                    playerTargetShip.FactionId, playerTargetShip.Clan.Id, playerTargetShip.RankId, playerTargetShip.IsMapIntruder(),
                    new ClanRelationModule((short)playerTargetShip.Clan.Compare(targetShip.Clan)), playerTargetShip.Gates.GetGateRings(),
                    true, false, CharacterStateManager.Instance.IsInState(playerTargetShip, CharacterStates.CLOAKED), 
                    0, 0);
            }
        }

        public void RemoveShipCommand(Player player, Character targetShip)
        {
            var session = GameStorageManager.Instance.FindSession(player);
            if (session == null)
            {
                Out.QuickLog("Bugged player session, session cannot be found, Player ID: " + player.Id, LogKeys.ERROR_LOG);
                return;
            }
            
            Packet.Builder.BuildCommand(session.GameClient, Commands.SHIP_REMOVE_COMMAND, player.UsingNewClient, targetShip.Id);
        }
    }
}