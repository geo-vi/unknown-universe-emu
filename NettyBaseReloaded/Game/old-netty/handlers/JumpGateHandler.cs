using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class JumpGateHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            if (gameSession == null) return;

            var nearestPortal = getNearestPortal(gameSession);
            if (nearestPortal != null && nearestPortal.Position.DistanceTo(gameSession.Player.Position) < 1000)
            {
                gameSession.Player.Controller.Miscs._Jump.Initiate(nearestPortal.DestinationMapId, nearestPortal.Destination, nearestPortal.Id);
            }

        }

        private Jumpgate getNearestPortal(GameSession gameSession)
        {
            var portalsOrdered =
                gameSession.Player.Spacemap.Portals.Values.OrderBy(
                    x => x.Position.DistanceTo(gameSession.Player.Position));

            return portalsOrdered.FirstOrDefault();
        }

    }
}
