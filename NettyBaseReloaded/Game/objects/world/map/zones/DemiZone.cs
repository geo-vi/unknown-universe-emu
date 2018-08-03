using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.zones
{
    class DemiZone : Zone
    {
        public DemiZone(int id, Vector botLeft, Vector topRight, Faction faction) : base(id, botLeft, topRight, faction) { }
    }
}
