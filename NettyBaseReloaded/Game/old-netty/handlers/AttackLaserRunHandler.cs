using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackLaserRunHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var parser = new ByteParser(bytes);

            if (gameSession == null) return;
            var targetId = parser.Int();

            // Get if targetId is valid
            var player = gameSession.Player;

            if (player.Selected == null) return;

            if (!player.Spacemap.Entities.ContainsKey(targetId) || player.Selected.Id != targetId)
            {
                Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }
            Debug.WriteLine("Attacking " + targetId);

            player.Controller.Attacking = true;
            player.Controller.LaserAttack();
        }
    }
}
