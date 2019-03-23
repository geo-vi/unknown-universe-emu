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
                if (!int.TryParse(packet[3], out userId))
                    return;

                var gameSession = World.StorageManager.GetGameSession(userId);
                var player = gameSession?.Player;
                if (player == null) return;
                switch (packet[1])
                {
                    case "eq":
                        player.State.WaitingForEquipmentRefresh = true;
                        break;
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
                    case "extras":
                        Console.WriteLine(packet[2]);
                        if (packet[2].Contains("BK"))
                        {
                            player.Information.UpdateBootyKeys();
                        }

                        break;
                }
            }
            catch(Exception) { }
        }

    }
}
