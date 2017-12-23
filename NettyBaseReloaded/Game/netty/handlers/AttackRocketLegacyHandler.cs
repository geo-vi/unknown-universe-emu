using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackRocketLegacyHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            if (gameSession == null) return;
            var targetId = Int32.Parse(param[1]);


            // Get if targetId is valid
            var player = gameSession.Player;

            if (player.Selected == null) return;

            if (!player.Spacemap.Entities.ContainsKey(targetId) || player.Selected.Id != targetId)
            {
                //Debug.WriteLine("Selected ID: " + player.Selected.Id + " Target ID: " + targetId);
                return;
            }

            player.Controller.Attack.LaunchMissle(player.Settings.CurrentRocket.LootId);
        }
    }
}
