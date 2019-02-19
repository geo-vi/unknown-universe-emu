using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class QR01 : Booster
    {
        public QR01(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.QR_01, Types.QUESTREWARD)
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
