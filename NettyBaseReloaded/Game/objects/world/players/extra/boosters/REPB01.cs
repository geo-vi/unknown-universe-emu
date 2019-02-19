using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class REPB01 : Booster
    {
        public REPB01(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.REP_B01, Types.REPAIR)
        {
        }

        public override double GetBoost()
        {
            return 0.1;
        }

        public override double GetSharedBoost()
        {
            return 0;
        }
    }
}
