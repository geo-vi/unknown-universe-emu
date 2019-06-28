using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class DroneFormationChangeHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var formationId = 0;
            if (!gameSession.Player.UsingNewClient)
            {
                var cmd = new DroneFormationChangeRequest();
                cmd.readCommand(buffer);
                formationId = cmd.selectedFormationId;
            }
            
            gameSession.Player.Controller.Miscs.UseItem(Slotbar.Items.FormationIds[formationId]);
        }
    }
}
