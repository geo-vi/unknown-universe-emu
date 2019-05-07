using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.ammo;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class HellstormSelectRocketHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new HellstormSelectRocketRequest();
            request.readCommand(buffer);
            var ammo = AmmoConverter.AmmoTypeToString(request.rocketType.type);
            gameSession.Player.RocketLauncher?.ChangeLoad(ammo);
        }
    }
}
