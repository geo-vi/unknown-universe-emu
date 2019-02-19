using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class PM00 : Mine
    {
        public override int MineType => 5;

        public PM00(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void Effect()
        {
            Damage.Area(Spacemap, Position, 1000, 25, Damage.Types.MINE, DamageType.PERCENTAGE);
            Task.Delay(5000).ContinueWith(t => Respawn());
        }
    }
}
