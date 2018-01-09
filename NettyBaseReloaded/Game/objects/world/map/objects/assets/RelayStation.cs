using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class RelayStation : Asset
    {
        public RelayStation(int id, Vector position) : base(id, "RelayStationTest1", AssetTypes.RELAY_STATION, Faction.NONE, Global.StorageManager.Clans[0], 0, 0, position, false, false, false)
        {
        }
    }
}
