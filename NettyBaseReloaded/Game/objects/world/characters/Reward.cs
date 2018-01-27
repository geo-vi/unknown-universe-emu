using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Reward
    {
        public List<object> Rewards = new List<object>();

        public Reward(RewardType type, int amount)
        {
            Rewards.Add((short) 0);
            Rewards.Add(type);
            Rewards.Add(amount);
        }

        public Reward(RewardType type, Item item, int amount)
        {
            Rewards.Add((short) 0);
            Rewards.Add(type);
            Rewards.Add(item);
            Rewards.Add(amount);
        }

        public Reward(Dictionary<RewardType, int> rewards)
        {
            short count = (short) Rewards.Count;
            foreach (var row in rewards)
            {
                Rewards.Add(count);
                Rewards.Add(row.Key);
                Rewards.Add(row.Value);
                count++;
            }
        }

        public void ParseRewards(Player player)
        {
            int paramNum = 0;
            RewardType rewardType;
            Item item;
            int amount = 0;
            foreach (var reward in Rewards)
            {
                if (reward is short)
                {
                    rewardType = (RewardType) Rewards[paramNum + 1];
                    if (Rewards[paramNum + 2] is Item)
                    {
                        item = (Item) Rewards[paramNum + 2];
                        amount = (int) Rewards[paramNum + 3];
                        RewardPlayer(player, rewardType, item, amount);
                    }
                    else
                    {
                        amount = (int) Rewards[paramNum + 2];
                        RewardPlayer(player, rewardType, amount);
                    }
                }
                paramNum++;
            }
        }

        public void RewardPlayer(Player player, RewardType type, int amount)
        {
            if (amount == 0) return;
            switch (type)
            {
                case RewardType.CREDITS:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Information.Credits.Add(amount);
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                        "0|LM|ST|CRE|" + amount + "|" + player.Information.Credits.Get());
                    break;
                case RewardType.URIDIUM:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Information.Uridium.Add(amount);
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                        "0|LM|ST|URI|" + amount + "|" + player.Information.Uridium.Get());
                    break;
                case RewardType.EXPERIENCE:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Information.Experience.Add(amount);
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                        "0|LM|ST|EP|" + amount + "|" + player.Information.Experience.Get() + "|" + player.Information.Level.Id);
                    break;
                case RewardType.HONOR:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Information.Honor.Add(amount);
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                        "0|LM|ST|HON|" + amount + "|" + player.Information.Honor.Get());
                    break;
                case RewardType.ORE:
                    break;
                case RewardType.GALAXY_GATES_ENERGY:
                    amount = RewardMultiplyer(type, amount, player);
                    break;
                case RewardType.BOOSTER:
                    // amount => hours
                    break;
            }
        }

        public int RewardMultiplyer(RewardType type, int amount, Player player)
        {
            if (type == RewardType.EXPERIENCE) amount = (int)(amount * player.Hangar.Ship.GetExpBonus(player));
            if (type == RewardType.HONOR) amount = (int)(amount * player.Hangar.Ship.GetHonorBonus(player));
            return amount * Properties.Game.REWARD_MULTIPLYER; // TODO: Reward levels
        }

        public void RewardPlayer(Player player, RewardType type, Item item, int amount)
        {
            switch (type)
            {
                case RewardType.AMMO:
                    // BAT / ROK
                    if (item.LootId.Contains("ammunition_laser"))
                    {
                        amount = RewardMultiplyer(type, amount, player);
                        player.Information.Ammunitions[item.LootId].Add(amount);
                        Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                            "0|LM|ST|BAT|" + Converter.GetLootAmmoId(item.LootId) + "|" + amount);
                    }
                    else if (item.LootId.Contains("ammunition_rocket"))
                    {
                        amount = RewardMultiplyer(type, amount, player);
                        player.Information.Ammunitions[item.LootId].Add(amount);
                        Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                            "0|LM|ST|ROK|" + Converter.GetLootAmmoId(item.LootId) + "|" + amount);
                    }
                    break;
                case RewardType.ITEM:
                    break;
            }
        }
    }
}

