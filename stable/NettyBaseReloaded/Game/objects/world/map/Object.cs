using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map
{ 
    abstract class Object
    {
        public int Id { get; }
        public Vector Position { get; set; }
        public int Range { get; set; }

        public Object(int id, Vector pos, int range = 1000)
        {
            Id = id;
            Position = pos;
            Range = range;
        }

        public abstract void execute(Character character);
    }
}
