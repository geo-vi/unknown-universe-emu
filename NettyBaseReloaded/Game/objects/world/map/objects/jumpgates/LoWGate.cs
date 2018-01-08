using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class LoWGate : Jumpgate
    {
        public LoWGate(int id, Vector pos) : base(id, Faction.NONE, pos, new Vector(1000, 11800), 200, true, 0, 0, 34)
        {
        }
    }
}
