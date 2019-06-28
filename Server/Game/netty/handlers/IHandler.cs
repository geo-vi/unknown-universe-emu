using DotNetty.Buffers;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    interface IHandler
    {
        void execute(GameSession gameSession, IByteBuffer buffer);
    }
}
