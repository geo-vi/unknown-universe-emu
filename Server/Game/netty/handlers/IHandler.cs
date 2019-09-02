using DotNetty.Buffers;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    interface IHandler
    {
        void Execute(GameSession gameSession, IByteBuffer buffer);
    }
}
