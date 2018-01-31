using System;
using System.Linq;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class LegacySelectHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] packet)
        {
            switch (packet[0])
            {
                case ServerCommands.TECHS:
                    switch (packet[1])
                    {
                        case "1":
                            gameSession.Player.Controller.Miscs.UseItem("tech_energy-leech");
                            break;
                        case "2":
                            gameSession.Player.Controller.Miscs.UseItem("tech_chain-impulse");
                            break;
                        case "3":
                            gameSession.Player.Controller.Miscs.UseItem("tech_precision-targeter");
                            break;
                        case "4":
                            gameSession.Player.Controller.Miscs.UseItem("tech_backup-shields");
                            break;
                        case "5":
                            gameSession.Player.Controller.Miscs.UseItem("tech_battle-repair-bot");
                            break;
                    }
                    break;
                default:
                    switch (packet[1])
                    {
                        case ServerCommands.EMP:
                            gameSession.Player.Controller.Miscs.UseItem("ammunition_specialammo_emp-01");
                            break;
                        case ServerCommands.SMARTBOMB:
                            gameSession.Player.Controller.Miscs.UseItem("ammunition_mine_smb-01");
                            break;
                        case ServerCommands.INSTASHIELD:
                            gameSession.Player.Controller.Miscs.UseItem("equipment_extra_cpu_ish-01");
                            break;
                        case ClientCommands.ROBOT:
                            gameSession.Player.Controller.Miscs.UseItem(gameSession.Player.Equipment.GetRobot());
                            break;
                        case ClientCommands.SELECT_CLOAK:
                            gameSession.Player.Controller.Miscs.UseItem(gameSession.Player.Extras.FirstOrDefault(x => x.Value is Cloak).Key);
                            break;
                        case ClientCommands.AROL:
                            gameSession.Player.Controller.Miscs.UseItem("equipment_extra_cpu_arol-x");
                            break;
                        case ClientCommands.RLLB:
                            gameSession.Player.Controller.Miscs.UseItem("equipment_extra_cpu_rllb-x");
                            break;
                        case ClientCommands.CONFIGURATION:
                            gameSession.Player.Controller.Miscs.ChangeConfig();
                            break;
                        case ClientCommands.MINE:
                            switch (packet[2])
                            {
                                case ServerCommands.MINE_ACM:
                                    gameSession.Player.Controller.Miscs.UseItem("ammunition_mine_acm-01");
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
    }
}