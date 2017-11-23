using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class CollectBoxHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;
            string hash = "";
            Packet.Builder.LegacyModule(gameSession, "0|" + ServerCommands.SET_ATTRIBUTE + "|" + ServerCommands.ASSEMBLE_COLLECTION_BEAM_ACTIVE + "|0|" + player.Id + "|-1");
            if (player.UsingNewClient)
            {
                var cmd = new commands.new_client.requests.CollectBoxRequest();
                cmd.readCommand(bytes);
                hash = cmd.itemHash;
            }
            else
            {
                var cmd = new commands.old_client.requests.CollectBoxRequest();
                cmd.readCommand(bytes);
                hash = cmd.itemHash;
            }
            (player.Spacemap.HashedObjects[hash] as BonusBox)?.Collect(player);
        }
    }
}
