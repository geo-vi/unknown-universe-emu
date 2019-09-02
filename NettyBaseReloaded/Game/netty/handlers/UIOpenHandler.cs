using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.new_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
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
