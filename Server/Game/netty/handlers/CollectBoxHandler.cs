using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class CollectBoxHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var player = gameSession.Player;
            string hash = "";
            Packet.Builder.LegacyModule(gameSession, "0|" + ServerCommands.SET_ATTRIBUTE + "|" + ServerCommands.ASSEMBLE_COLLECTION_BEAM_ACTIVE + "|0|" + player.Id + "|-1");
            if (player.UsingNewClient)
            {
                var cmd = new CollectBoxRequest();
                cmd.readCommand(buffer);
                hash = cmd.itemHash;
            }
            else
            {
                var cmd = new commands.old_client.requests.CollectBoxRequest();
                cmd.readCommand(buffer);
                hash = cmd.itemHash;
            }

            if (player.Spacemap.HashedObjects.ContainsKey(hash))
            {
                (player.Spacemap.HashedObjects[hash] as Collectable)?.Collect(player);
            }
        }
    }
}
