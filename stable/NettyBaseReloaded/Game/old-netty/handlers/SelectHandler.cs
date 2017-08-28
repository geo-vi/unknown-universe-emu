using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class SelectHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] packet)
        {
            switch (packet[1])
            {
                case ClientCommands.CONFIGURATION:
                    gameSession.Player.Controller.Miscs.ChangeConfig(Convert.ToInt32(packet[2]));
                    break;
                case ClientCommands.ROBOT:
                    gameSession.Player.Controller.CPUs.Activate(PlayerController.CPU.Types.ROBOT);
                    break;
            }
        }
    }
}
