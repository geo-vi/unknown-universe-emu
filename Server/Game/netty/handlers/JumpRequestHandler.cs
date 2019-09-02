using System.Collections.Generic;
using System.Linq;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace Server.Game.netty.handlers
{
    class JumpRequestHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] args)
        {
            var player = gameSession.Player;
            var portals = gameSession.Player.Range.Objects.ToList().Where(x => x.Value is Jumpgate);
            var portalArray = portals as KeyValuePair<int, Object>[] ?? portals.ToArray();
            if (portalArray.Count() > 1)
            {
                Jumpgate closestJumpgate = null;
                foreach (var portal in portalArray)
                {
                    var _portal = portal.Value as Jumpgate;
                    if (closestJumpgate == null)
                    {
                        closestJumpgate = _portal;
                    }
                    else
                    {
                        if (_portal != null && closestJumpgate.Position.DistanceTo(player.Position) >
                            _portal.Position.DistanceTo(player.Position))
                            closestJumpgate = _portal;
                    }
                }
                closestJumpgate?.click(player);
            }
            else
            {
                foreach (var portal in portalArray)
                {
                    (portal.Value as Jumpgate)?.click(player);
                }
            }
        }

    }
}
