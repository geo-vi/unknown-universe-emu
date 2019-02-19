using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class SHDB02 : Booster
    {
        public SHDB02(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.SHD_B02, Types.SHIELD)
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
