using DotNetty.Buffers;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class MoveHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var newVector = new Vector(0, 0);

            if (gameSession.Player.UsingNewClient)
            {
                var movementCommand = new MoveRequest();
                movementCommand.readCommand(buffer);

                newVector = new Vector(movementCommand.NewX, movementCommand.NewY);
            }
            else
            {
                var movementCommand = new Server.Game.netty.commands.old_client.requests.MoveRequest();
                movementCommand.readCommand(buffer);

                newVector = new Vector(movementCommand.targetX, movementCommand.targetY);
            }

            //Console.WriteLine("{0}, {1}", gameSession.Player.Id, newVector);
            MovementController.Move(gameSession.Player, newVector);
        }

    }
}
