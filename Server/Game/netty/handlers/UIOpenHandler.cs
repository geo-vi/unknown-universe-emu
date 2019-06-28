using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands.new_client.requests;

namespace Server.Game.netty.handlers
{
    class UIOpenHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var commandM1B = new UIOpenRequest();
            commandM1B.readCommand(buffer);
            var itemId = commandM1B.itemId;
            if (itemId == "ship_warp") Packet.Builder.ShipWarpWindowCreateCommand(gameSession);
        }
    }
}
