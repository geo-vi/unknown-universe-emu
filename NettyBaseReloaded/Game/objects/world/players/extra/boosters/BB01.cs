using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class BB01 : Booster
    {
        public BB01(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.BB_01, Types.BONUSBOXES)
        {
        }

        public override double GetBoost()
        {
            return 1;
        }

        public override double GetSharedBoost()
        {
            return 0;
        }
    }
}
