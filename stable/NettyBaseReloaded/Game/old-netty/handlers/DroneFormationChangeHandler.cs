using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class DroneFormationChangeHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            var targetFormationId = parser.Int();

            gameSession.Player.Controller.Miscs.ChangeDroneFormation((DroneFormation) targetFormationId);
        }
    }
}
