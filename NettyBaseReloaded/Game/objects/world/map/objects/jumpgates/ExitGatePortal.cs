using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class ExitGatePortal : Jumpgate
    {
        private GalaxyGate GalaxyGate;
        public ExitGatePortal(GalaxyGate galaxyGate, int id, Vector pos, Spacemap map, Vector destinationPos, int destinationMapId) : base(id, Faction.NONE, pos, map, destinationPos, destinationMapId, true, 0, 0, PortalGraphics.STANDARD_GATE)
        {
            GalaxyGate = galaxyGate;
        }

        public override void click(Character character)
        {
            if (character is Player player)
            {
                GalaxyGate?.CheckAndRemove(player);
            }
        }
    }
}
