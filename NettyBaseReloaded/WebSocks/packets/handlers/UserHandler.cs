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
            int userId;
            if (!int.TryParse(packet[2], out userId))
                return;

            var gameSession = World.StorageManager.GetGameSession(userId);
            var player = gameSession?.Player;
            if (player == null) return;

            switch (packet[1])
            {
                case "drones":
                    player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
                    foreach (var playerEntity in player.Spacemap.Entities.Where(x => x.Value is Player))
                    {
                        var entitySession = World.StorageManager.GetGameSession(playerEntity.Value.Id);
                        if (entitySession != null)
                            Packet.Builder.DronesCommand(entitySession, player);
                    }
                    break;
                case "ammo":
                    Ammunition.ForceSync(player);
                    break;
            }
        }
    }
}
