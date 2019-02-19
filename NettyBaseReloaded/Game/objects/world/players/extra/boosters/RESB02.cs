using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class RESB02 : Booster
    {
        public RESB02(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.RES_B02, Types.RESOURCE)
        {
        }

        public override double GetBoost()
        {
            return 0.25;
        }

        public override double GetSharedBoost()
        {
            return 0.1;
        }
    }
}
