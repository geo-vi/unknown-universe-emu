using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets.cbs
{
    class Module : Asset
    {
        public Module(int id, string name, Faction faction, Clan clan, Vector position, Spacemap map) : base(id, name, AssetTypes.SATELLITE, faction, clan, 1, 0, position, map, false, false, false)
        {

        }
    }
}
