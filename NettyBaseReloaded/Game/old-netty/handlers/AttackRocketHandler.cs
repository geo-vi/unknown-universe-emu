using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackRocketHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            if (gameSession == null) return;

            var player = gameSession.Player;

            player.Controller?.LaunchMissle(new AttackTypeModule((short)player.Settings.Slotbar.SelectedRocket));

        }
    }
}
