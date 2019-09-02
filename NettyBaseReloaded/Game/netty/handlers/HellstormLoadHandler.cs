using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class HellstormLoadHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var player = gameSession.Player;
            if (player.RocketLauncher != null)
                player.RocketLauncher.Loading = true;
        }
    }
}
