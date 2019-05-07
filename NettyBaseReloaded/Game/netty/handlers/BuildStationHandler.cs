using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class BuildStationHandler : IHandler
    {
        public void execute(GameSession session, IByteBuffer buffer)
        {
            if (session.Player.UsingNewClient) return;
            var cmd = new BuildStationRequest();
            cmd.readCommand(buffer);
            if (session.Player.Range.Objects.Values.Where(x => x is Asteroid)
                .FirstOrDefault(x => ((Asteroid) x).AssignedBattleStationId == cmd.battleStationId) is Asteroid asteroid)
                asteroid.InitializeBuildingState(session.Player, cmd.buildTimeInMinutes);
        }
    }
}
