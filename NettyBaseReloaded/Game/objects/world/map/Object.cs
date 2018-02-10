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
        public Spacemap Spacemap { get; set; }
        public int Range { get; set; }

        public int VirtualWorldId { get; set; }

        protected Object(int id, Vector pos, Spacemap map, int range = 1000)
        {
            Id = id;
            Position = pos;
            Spacemap = map;
            Range = range;
        }

        public abstract void execute(Character character);

        public virtual void Tick()
        {
            
        }

        public virtual void Destroy()
        {
        }
    }
}
