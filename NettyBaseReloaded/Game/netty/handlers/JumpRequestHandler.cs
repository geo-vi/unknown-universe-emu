using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class JumpRequestHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] args)
        {
            var player = gameSession.Player;
            var portals = gameSession.Player.Range.Objects.ToList().Where(x => x.Value is Jumpgate);
            if (portals.Count() > 1)
            {
                Jumpgate closestJumpgate = null;
                foreach (var portal in portals)
                {
                    var _portal = portal.Value as Jumpgate;
                    if (closestJumpgate == null)
                    {
                        closestJumpgate = _portal;
                    }
                    else
                    {
                        if (closestJumpgate.Position.DistanceTo(player.Position) >
                            _portal.Position.DistanceTo(player.Position))
                            closestJumpgate = _portal;
                    }
                }
                closestJumpgate.click(player);
            }
            else
            {
                foreach (var portal in portals)
                {
                    (portal.Value as Jumpgate).click(player);
                }
            }
        }

    }
}
