using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.zones
{
    class PalladiumZone : Zone
    {
        public PalladiumZone(int id, Vector topLeft, Vector botRight) : base(id, topLeft, botRight, Faction.NONE)
        {
        }
    }
}
