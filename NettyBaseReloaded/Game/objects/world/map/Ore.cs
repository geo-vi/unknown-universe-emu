using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map
{
    class Ore : Object
    {
        public string Hash { get; }

        public OreTypes Type { get; set; }

        public Ore(int id, string hash, OreTypes type, Vector pos, Spacemap map) : base(id, pos, map)
        {
            Hash = hash;
            Type = type;
        }

        public override void execute(Character character)
        {
        }
    }
}
