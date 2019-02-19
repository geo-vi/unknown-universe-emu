using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class RESB01 : Booster
    {
        public RESB01(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.RES_B01, Types.RESOURCE)
        {
        }

        public override double GetBoost()
        {
            return 0.25;
        }

        public override double GetSharedBoost()
        {
            return 0;
        }
    }
}
