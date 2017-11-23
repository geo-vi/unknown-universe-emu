using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Collectable : Object
    {
        public string Hash { get; set; }

        public Vector Position { get; set; }

        public Types Type { get; set; }

        public Collectable(int id, string hash, Types type, Vector pos) : base(id, pos)
        {
            Hash = hash;
            Type = type;
            Position = pos;
        }

        public abstract void Collect(Player player);

        protected abstract void Reward(Player player);

        public abstract void Dispose(Spacemap map);

        protected void Respawn(Spacemap map)
        {

        }

        public override void execute(Character character)
        {
        }
    }
}
