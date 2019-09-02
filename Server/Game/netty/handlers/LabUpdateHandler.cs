using DotNetty.Buffers;

namespace Server.Game.netty.handlers
{
    class LabUpdateHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                return;
            }

            Packet.Builder.LabUpdateItemCommand(gameSession, gameSession.Player.Skylab);
        }
    }
}
