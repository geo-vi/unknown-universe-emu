using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class Asteroid : Asset, IClickable
    {
        public Asteroid(int id, string name, Vector pos) : base(id, name, AssetTypes.ASTEROID, Faction.NONE,
            Global.StorageManager.Clans[0], 1, 0, pos, false, false, false)
        {
            
        }

        public void click(Character character)
        {
            
        }
    }
}
