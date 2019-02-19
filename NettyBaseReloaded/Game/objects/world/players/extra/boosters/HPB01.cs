using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class HPB01 : Booster
    {
        public HPB01(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.HP_B01, Types.MAXHP)
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
