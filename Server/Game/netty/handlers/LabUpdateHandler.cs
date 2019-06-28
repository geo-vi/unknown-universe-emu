using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using Server.Game.netty;

namespace NettyBaseReloaded.Game.netty.handlers
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
