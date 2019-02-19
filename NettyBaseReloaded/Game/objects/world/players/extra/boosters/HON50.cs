using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class HON50 : Booster
    {
        public HON50(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.HON50, Types.HONOUR)
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
