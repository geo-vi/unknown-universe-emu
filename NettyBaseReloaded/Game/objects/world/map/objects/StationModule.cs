using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class StationModule : Object, IClickable
    {
        public short Type { get; private set; }

        public StationModule(int id, Vector pos, short type) : base(id, pos)
        {
            Type = type;
        }
        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
        }
    }
}
