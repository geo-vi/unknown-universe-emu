using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
