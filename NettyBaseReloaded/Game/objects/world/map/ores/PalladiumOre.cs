using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map.ores
{
    class PalladiumOre : Ore
    {
        public PalladiumOre(int id, string hash, OreTypes type, Vector pos, Spacemap map, int[] limits) : base(id, hash, type, pos, map, limits)
        {
        }

        public override void Collect(Character character)
        {
            base.Collect(character);
            Reward();
        }

        public void Reward()
        {

        }
    }
}
