using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Mine : Object
    {
        public string Hash { get; set; }

        public Mine(int id, string hash, Vector pos) : base(id, pos)
        {
            Hash = hash;
        }
    }
}
