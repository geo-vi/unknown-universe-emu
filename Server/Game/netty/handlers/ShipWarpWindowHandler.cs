using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;

namespace Server.Game.netty.handlers
{
    class ShipWarpWindowHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            Packet.Builder.ShipWarpWindowCreateCommand(gameSession);
        }
    }
}
