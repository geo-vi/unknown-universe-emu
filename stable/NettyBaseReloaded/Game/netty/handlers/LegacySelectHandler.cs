using System;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class LegacySelectHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] packet)
        {
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
                    gameSession.Player.Controller.Miscs.UseItem(gameSession.Player.GetRobot());
                    break;
                case ClientCommands.CONFIGURATION:
                    gameSession.Player.Controller.Miscs.ChangeConfig();
                    break;
            }
        }
    }
}