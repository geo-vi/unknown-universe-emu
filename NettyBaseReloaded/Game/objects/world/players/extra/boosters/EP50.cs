using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class EP50 : Booster
    {
        public EP50(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.EP50, Types.EP)
        {
        }

        public override double GetBoost()
        {
            return 0.5;
        }

        public override double GetSharedBoost()
        {
            return 0;
        }
    }
}
