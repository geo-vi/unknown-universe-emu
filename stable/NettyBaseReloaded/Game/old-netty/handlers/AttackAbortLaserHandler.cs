using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackAbortLaserHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            if (gameSession?.Player.Controller == null || gameSession.Player.Selected == null) return;

            gameSession.Player.Controller.Attacking = false;

            gameSession.Player.Selected.Controller.Attacked = false;
        }
    }
}
