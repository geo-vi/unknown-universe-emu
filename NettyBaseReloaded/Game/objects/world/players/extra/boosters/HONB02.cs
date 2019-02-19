using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class HONB02 : Booster
    {
        public HONB02(int id, Player player, DateTime finishTime, Boosters boosterType, Types type) : base(id, player, finishTime, boosterType, type)
        {
        }

        public override double GetBoost()
        {
            return 0.1;
        }

        public override double GetSharedBoost()
        {
            return 0.05;
        }
    }
}
