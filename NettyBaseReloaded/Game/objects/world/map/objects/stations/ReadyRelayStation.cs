using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.objects.stations
{
    class ReadyRelayStation : Station
    {
        public ReadyRelayStation(int id, Vector pos) : base(id, new List<StationModule>(), Faction.NONE, pos)
        {
        }
    }
}
