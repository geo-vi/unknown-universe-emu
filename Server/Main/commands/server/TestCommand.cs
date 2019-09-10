using System;
using System.Linq;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Main.commands.server
{
    class TestCommand : GlobalCommand
    {
        public TestCommand() : base("test", "Command to test logics", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                Player epicenter = GameStorageManager.Instance.GameSessions.FirstOrDefault().Value.Player;
                Out.QuickLog("Epicenter chosen: " + epicenter.Name + " at position " + epicenter.Position + " for 500 units at amount 1000", LogKeys.SERVER_LOG);
                switch (args[1])
                {
                    case "heal":
                        CombatManager.Instance.HealArea(epicenter, 500, 1000, HealingTypes.HEALTH_AREA,
                            CalculationTypes.DEFINED);
                        Out.QuickLog("Healing area...", LogKeys.SERVER_LOG);
                        break;
                    case "damage":
                        Out.QuickLog("Damaging...", LogKeys.SERVER_LOG);
                        break;
                    case "revive":
                        var deadPlayer =
                            GameStorageManager.Instance.GameSessions.FirstOrDefault(x => x.Value.Player.IsDead).Value.Player;
                        deadPlayer.Controller.GetInstance<PlayerDestructionController>().ReviveCharacter();
                        Out.QuickLog("Reviving " + deadPlayer.Name + "...", LogKeys.SERVER_LOG);
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}