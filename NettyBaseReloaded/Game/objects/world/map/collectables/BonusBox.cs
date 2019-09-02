using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables.rewards;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Networking;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class BonusBox : Collectable
    {
        public static List<PotentialReward> REWARDS = new List<PotentialReward>();
        public static int SPAWN_COUNT = 0;
        public static int PVP_SPAWN_COUNT = 0;

        public static int GetSpawnCount(Spacemap map)
        {
            return map.Pvp ? PVP_SPAWN_COUNT : SPAWN_COUNT;
        }

        private bool Respawning { get; }

        /// <summary>
        /// Bot detection system!
        /// </summary>
        public bool IsHoneyBox;

        public Reward CustomReward;

        public BonusBox(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits, bool respawning = false, bool isHoneyBox = false, Reward reward = null) : base(id, hash, type, pos, map, limits, isHoneyBox)
        {
            Respawning = respawning;
            IsHoneyBox = isHoneyBox;
            CustomReward = reward;
        }

        public override void Collect(Character character)
        {
            base.Collect(character);
            if (Type == Types.BONUS_BOX && character is Player player)
            {
                player.Statistics.CollectBox();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Respawning)
                Respawn();
        }

        protected override void Reward(Player player)
        {
            try
            {
                Reward reward;

                if (CustomReward != null)
                {
                    reward = CustomReward;
                }
                else
                {
                    RewardType type;
                    var orderedRewards = REWARDS.OrderBy(x => x.Chance).ToArray();
                    var random = RandomInstance.getInstance(this);
                    //todo
                    var n = random.NextDouble();
                    var potentialReward = orderedRewards.FirstOrDefault(x => x.Chance > n) ?? orderedRewards.Last();
                    if (Enum.TryParse(potentialReward.LootId, true, out type))
                    {
                        reward = new Reward(type, potentialReward.Amount);
                    }
                    else
                    {
                        type = RewardType.ITEM;
                        if (potentialReward.LootId.StartsWith("ammunition"))
                            type = RewardType.AMMO;
                        reward = new Reward(type, Item.Find(potentialReward.LootId), potentialReward.Amount);
                    }
                }
                
                if (player.BoostedBoxRewards == 1)
                    reward.ParseRewards(player, 2);
                else reward.ParseRewards(player);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public override bool PetCanCollect(Player owner)
        {
            return !IsHoneyBox;
        }
    }
}