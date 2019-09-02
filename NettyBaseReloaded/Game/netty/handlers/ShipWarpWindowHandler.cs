using NettyBaseReloaded.Game.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipWarpWindowHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            Packet.Builder.ShipWarpWindowCreateCommand(gameSession);
        }
    }
}
