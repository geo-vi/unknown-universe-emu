using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Billboard : Object
    {
        public short Advertiser;

        public Billboard(int id, Vector pos, Spacemap map, short advertiser, int range = 1000) : base(id, pos, map, range)
        {
            Advertiser = advertiser;
        }

        public override void execute(Character character)
        {
            //TODO: Show advertisement
        }
    }
}
