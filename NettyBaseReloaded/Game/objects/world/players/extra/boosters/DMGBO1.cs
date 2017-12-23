using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class DMGBO1 : Booster
    {
        public DMGBO1(Player player, DateTime finishTime) : base(player, finishTime, Boosters.DMG_B01, Types.DAMAGE) { }

        public override double GetBoost()
        {
            return 1.1;
        }

        public override double GetSharedBoost()
        {
            return 0;
        }
    }
}
