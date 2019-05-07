using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class HarvestHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;
            var cmd = new HarvestRequest();
            cmd.readCommand(buffer);
            string itemHash = cmd.itemHash;
            var player = gameSession.Player;
            var resource = player.Spacemap.HashedObjects[itemHash];

            if (resource != null)
            {
                if (player.Position.DistanceTo(resource.Position) > 200) return;
                var ore = resource as Ore;
                ore?.Collect(player);
            }
        }
    }
}
