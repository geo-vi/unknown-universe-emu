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

        public virtual bool PulseActive => true;

        public virtual int MineType => -1;

        protected Mine(int id, string hash, Vector pos, Spacemap map) : base(id, pos, map)
        {
            Hash = hash;
        }

        public override void execute(Character character)
        {
            Console.WriteLine("bombing.");
        }
    }
}
