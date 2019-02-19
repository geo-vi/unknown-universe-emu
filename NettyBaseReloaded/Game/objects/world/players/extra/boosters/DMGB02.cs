using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.players.extra.boosters
{
    class DMGB02 : Booster
    {
        public DMGB02(int id, Player player, DateTime finishTime) : base(id, player, finishTime, Boosters.DMG_B02, Types.DAMAGE)
        {
        }

        private void CheckForBoost(Player player, bool isRemove)
        {
            Booster boost = null;
            if (isRemove) player?.InheritedBoosters.TryRemove(Player, out boost);
            else if (player != null && !player.InheritedBoosters.ContainsKey(Player))  player.InheritedBoosters.TryAdd(Player, this);
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
