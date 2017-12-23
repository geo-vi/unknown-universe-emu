using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class Killscreen
    {
        public Killscreen(Player player)
        {
            if (player == null)
                return;

            // TODO::Add Killscreen

            World.StorageManager.GetGameSession(player.Id).Disconnect(GameSession.DisconnectionType.NORMAL);
        }
    }
}
