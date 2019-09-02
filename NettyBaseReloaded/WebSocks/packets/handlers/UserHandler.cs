using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class UserHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            try
            {
                int userId;
                if (!int.TryParse(packet[2], out userId))
                    return;

                var gameSession = World.StorageManager.GetGameSession(userId);
                var player = gameSession?.Player;
                if (player == null) return;
                switch (packet[1])
                {
                    case "eq":
                        if (player.State.InEquipmentArea)
                        {
                            player.Equipment.Reload();
                            player.Refresh();
                            player.State.WaitingForEquipmentRefresh = false;
                        }
                        else player.State.WaitingForEquipmentRefresh = true;
                        break;
                }
            }
            catch 
            {
            }
        }

    }
}
