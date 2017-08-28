using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class Asteroid : Asset, IClickable
    {
        public Asteroid(int id, string name, Vector pos) : base(id, name, AssetTypeModule.ASTEROID, (int) Faction.NONE,
            "", id, 1, 0, pos, -1, false, false, false)
        {
            
        }

        public void click(Character character)
        {
            
        }
    }
}
