using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class HONB02 : Booster
    {
        public HONB02(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.HON_B02, Types.HONOUR)
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
