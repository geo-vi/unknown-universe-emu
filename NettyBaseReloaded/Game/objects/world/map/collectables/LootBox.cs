using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class LootBox : Collectable
    {
        private Reward BoxReward;

        public LootBox(int id, string hash, Types type, Vector pos, Spacemap map, Reward reward, int disposeTimeMs) : base(id, hash, type, pos, map, null, false)
        {
            Temporary = true;
            DelayedDispose(disposeTimeMs);
            BoxReward = reward;
        }

        protected override void Reward(Player player)
        {
            BoxReward.ParseRewards(player);
        }

        public override bool PetCanCollect(Player owner)
        {
            return true;
        }
    }
}
