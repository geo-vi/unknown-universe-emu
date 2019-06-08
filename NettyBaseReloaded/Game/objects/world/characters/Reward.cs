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

        #region Constructors 

        public Reward()
        {

        }

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

        #endregion

        public double TotalAddedCre = 0;
        public double TotalAddedUri = 0;
        public double TotalAddedExp = 0;
        public double TotalAddedHon = 0;

        public void ParseRewards(Player player, int boost = 1)
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
                    else if (Rewards[paramNum + 2] is int)
                    {
                        amount = (int) Rewards[paramNum + 2];
                        RewardPlayer(player, rewardType, amount * boost);
                    }
                }

                paramNum++;
            }

            PerformBulkUpdate(player);
        }

        public void RewardPlayer(Player player, RewardType type, int amount)
        {
            if (amount == 0) return;

            switch (type)
            {
                case RewardType.CREDITS:
                    amount = RewardMultiplyer(type, amount, player);
                    TotalAddedCre = amount;
                    break;
                case RewardType.URIDIUM:
                    amount = RewardMultiplyer(type, amount, player);
                    TotalAddedUri = amount;
                    break;
                case RewardType.EXPERIENCE:
                    amount = RewardMultiplyer(type, amount, player);
                    amount += (int) (amount * player.BoostedExpReward);
                    TotalAddedExp = amount;
                    break;
                case RewardType.HONOR:
                    amount = RewardMultiplyer(type, amount, player);
                    amount += (int) (amount * player.BoostedHonorReward);
                    TotalAddedHon = amount;
                    break;
                case RewardType.ORE:
                    break;
                case RewardType.GALAXY_GATES_ENERGY:
                    amount = RewardMultiplyer(type, amount, player);
                    Packet.Builder.LegacyModule(player.GetGameSession(),
                        "0|A|STM|log_msg_gather_extra-energy_p|%COUNT%|" + amount);
                    World.DatabaseManager.AddGGEnergy(player, amount);
                    break;
                case RewardType.BOOSTER:
                    // amount => hours
                    break;
            }
        }

        public void PerformBulkUpdate(Player player)
        {
            if (TotalAddedCre > 0 || TotalAddedUri > 0 || TotalAddedExp > 0 || TotalAddedHon > 0)
            {
                player.Information.UpdateInfoBulk(TotalAddedCre, TotalAddedUri, TotalAddedExp, TotalAddedHon);
                var gameSession = player.GetGameSession();
                if (TotalAddedCre > 0)
                {
                    Packet.Builder.LegacyModule(gameSession,
                        "0|LM|ST|CRE|" + TotalAddedCre + "|" + player.Information.Credits.Get());
                }

                if (TotalAddedUri > 0)
                {
                    Packet.Builder.LegacyModule(gameSession,
                        "0|LM|ST|URI|" + TotalAddedUri + "|" + player.Information.Uridium.Get());
                }

                if (TotalAddedExp > 0)
                {
                    Packet.Builder.LegacyModule(gameSession,
                        "0|LM|ST|EP|" + TotalAddedExp + "|" + player.Information.Experience.Get() + "|" +
                        player.Information.Level.Id);
                    if (player.Pet != null && player.Pet.Controller.Active)
                    {
                        Packet.Builder.PetExperiencePointsUpdateCommand(gameSession, player.Pet.Experience,
                            player.Pet.GetMaxExp());
                    }
                }

                if (TotalAddedHon > 0)
                {
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                        "0|LM|ST|HON|" + TotalAddedHon + "|" + player.Information.Honor.Get());
                }
            }
        }

        public int RewardMultiplyer(RewardType type, int amount, Player player)
        {
            if (type == RewardType.EXPERIENCE)
            {
                amount = (int) (amount * player.Hangar.Ship.GetExpBonus(player));
            }

            if (type == RewardType.HONOR) amount = (int) (amount * player.Hangar.Ship.GetHonorBonus(player));
            return amount * Properties.Game.REWARD_MULTIPLYER; // TODO: Reward levels
        }

        public void RewardPlayer(Player player, RewardType type, Item item, int amount)
        {
            switch (type)
            {
                case RewardType.AMMO:
                    // BAT / ROK
                    if (item.LootId.StartsWith("ammunition_laser"))
                    {
                        amount = RewardMultiplyer(type, amount, player);
                        player.Information.Ammunitions[item.LootId].Add(amount);
                        Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                            "0|LM|ST|BAT|" + AmmoConverter.GetLootAmmoId(item.LootId) + "|" + amount);
                    }
                    else if (item.LootId.StartsWith("ammunition_rocket"))
                    {
                        amount = RewardMultiplyer(type, amount, player);
                        player.Information.Ammunitions[item.LootId].Add(amount);
                        Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                            "0|LM|ST|ROK|" + AmmoConverter.GetLootAmmoId(item.LootId) + "|" + amount);
                    }

                    break;
                case RewardType.ITEM:
                    switch (item.Category)
                    {
                        //case EquippedItemCategories.BOOSTER:
                        //    World.DatabaseManager.AddBooster(player, item);
                        //    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id), "0|LM|ST|LOT|" + item.LootId + "|" + amount);
                        //    break;
                        default:
                            World.DatabaseManager.AddEquipmentItem(player, item);
                            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),
                                "0|LM|ST|LOT|" + item.LootId + "|" + amount);
                            break;
                    }

                    break;
            }
        }

        public Reward Multiply(double multiplier)
        {
            if (multiplier == 1 || multiplier == 0) return this;
            var reward = new Reward();
            foreach (var rewardValue in Rewards)
            {
                if (rewardValue is int value)
                {
                    reward.Rewards.Add(Convert.ToInt32(
                        value * multiplier));
                }
                else reward.Rewards.Add(rewardValue);
            }

            return reward;
        }
    }
}

