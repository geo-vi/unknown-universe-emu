using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    class DroneFormationChangeHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new DroneFormationChangeRequest();
            cmd.readCommand(buffer);
            var formationId = cmd.selectedFormationId;

            DroneManager.Instance.ChangeDroneFormation(gameSession.Player, formationId);
        }
    }
}
