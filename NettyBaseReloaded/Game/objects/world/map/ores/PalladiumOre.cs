using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world.map.ores
{
    class PalladiumOre : Ore
    {
        public PalladiumOre(int id, string hash, OreTypes type, Vector pos, Spacemap map, Vector[] limits) : base(id, hash, type, pos, map, limits)
        {
        }

        public override void Collect(Character character)
        {
            base.Collect(character);
            Reward(character as Player);
        }

        public void Reward(Player player)
        {
            if (player == null) return;
            var random = new System.Random();
            var randomRewardIndex = random.Next(0, BonusBox.REWARDS.Count - 1);
            var rewardListIndex = BonusBox.REWARDS[randomRewardIndex];
            var lootId = rewardListIndex.Item1;
            var amount = rewardListIndex.Item2;
            RewardType type;
            Reward reward;
            if (RewardType.TryParse(lootId, true, out type))
            {
                reward = new Reward(type, amount);
            }
            else
            {
                if (lootId.Contains("ammunition"))
                    type = RewardType.AMMO;
                reward = new Reward(type, new Item(-1, lootId, amount), amount);
            }
            reward.ParseRewards(player);
        }
    }
}
