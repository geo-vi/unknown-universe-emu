using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class BonusBox : Collectable
    {
        public static List<Tuple<string, int>> REWARDS = new List<Tuple<string, int>>();
        public static int SPAWN_COUNT = 0;
        public static int PVP_SPAWN_COUNT = 0;

        private bool Respawning { get; }
        public BonusBox(int id, string hash, Vector pos, bool respawning = false) : base(id, hash, Types.BONUS_BOX, pos)
        {
            Respawning = respawning;
        }

        public override void Dispose(Spacemap map)
        {
            GameClient.SendToSpacemap(map, netty.commands.new_client.DisposeBoxCommand.write(Hash, true));
            GameClient.SendToSpacemap(map, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
            map.RemoveObject(this);
            Disposed = true;
            if (Respawning)
                Respawn(map);
        }

        protected override void Reward(Player player)
        {
            var random = new System.Random();
            var randomRewardIndex = random.Next(0, REWARDS.Count - 1);
            var rewardListIndex = REWARDS[randomRewardIndex];
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