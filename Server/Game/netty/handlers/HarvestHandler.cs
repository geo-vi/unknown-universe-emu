using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
