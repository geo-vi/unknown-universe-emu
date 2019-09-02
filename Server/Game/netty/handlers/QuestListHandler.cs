using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;

namespace Server.Game.netty.handlers
{
    class QuestListHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;
            Packet.Builder.QuestListCommand(gameSession);
        }
    }
}
