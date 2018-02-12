using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class RelayStation : AttackableAsset
    {
        public RelayStation(int id, Vector position, Spacemap map) : base(id, "RelayStation", AssetTypes.RELAY_STATION, Faction.NONE, Global.StorageManager.Clans[0], 0, 0, position, map, false, false, false, 100000, 100000, 0, 0, 0, 0, 0, 0)
        {
        }
    }
}
