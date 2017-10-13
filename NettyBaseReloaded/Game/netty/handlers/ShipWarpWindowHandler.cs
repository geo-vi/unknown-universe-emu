using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipWarpWindowHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            Packet.Builder.ShipWarpWindowCreateCommand(gameSession);
        }
    }
}
