using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Collectable
    {
        public string Hash { get; set; }

        public Vector Position { get; set; }

        public Types Type { get; set; }

        public Collectable(string hash, Types type, Vector pos)
        {
            Hash = hash;
            Type = type;
            Position = pos;
        }

        public abstract void Reward();

        public abstract void Dispose();
    }
}
