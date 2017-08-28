using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.packet;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class DropableRewards
    {
        public int Prometium { get; set; }
        public int Endurium { get; set; }
        public int Terbium { get; set; }
        public int Prometid { get; set; }
        public int Duranium { get; set; }
        public int Xenomit { get; set; }
        public int Promerium { get; set; }
        public int Seprom { get; set; }

        public DropableRewards(int Prometium, int Endurium,
            int Terbium, int Prometid, int Duranium, int Xenomit, int Promerium, int Seprom)
        {
            this.Prometium = Prometium;
            this.Endurium = Endurium;
            this.Terbium = Terbium;
            this.Prometid = Prometid;
            this.Duranium = Duranium;
            this.Xenomit = Xenomit;
            this.Promerium = Promerium;
            this.Seprom = Seprom;
        }

        //public List<OreCountModule> ToOreList()
        //{
        //    var oreList = new List<OreCountModule>
        //    {
        //        new OreCountModule(new OreTypeModule(OreTypeModule.PROMETIUM), Prometium),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.ENDURIUM), Endurium),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.TERBIUM), Terbium),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.XENOMIT), Xenomit),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.PROMETID), Prometid),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.DURANIUM), Duranium),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.PROMERIUM), Promerium),
        //        new OreCountModule(new OreTypeModule(OreTypeModule.SEPROM), Seprom)
        //    };
        //    return oreList;
        //}
    }

    class Reward
    {
        public List<object> Rewards = new List<object>();

        public Reward(RewardType type, int amount)
        {
            Rewards.Add((short)0);
            Rewards.Add(type);
            Rewards.Add(amount);
        }

        public Reward(RewardType type, Item item, int amount)
        {
            Rewards.Add((short)0);
            Rewards.Add(type);
            Rewards.Add(item);
            Rewards.Add(amount);
        }

        public Reward(Dictionary<RewardType, int> rewards)
        {
            short count = (short)Rewards.Count;
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
                    rewardType = (RewardType)Rewards[paramNum + 1];
                    if (Rewards[paramNum + 2] is Item)
                    {
                        item = (Item)Rewards[paramNum + 2];
                        amount = (int)Rewards[paramNum + 3];
                        RewardPlayer(player, rewardType, item, amount);
                    }
                    else
                    {
                        amount = (int)Rewards[paramNum + 2];
                        RewardPlayer(player, rewardType, amount);
                    }
                }
                paramNum++;
            }
        }

        public void RewardPlayer(Player player, RewardType type, int amount)
        {
            switch (type)
            {
                case RewardType.CREDITS:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Credits += amount;
                    Console.WriteLine("0|LM|ST|CRE|" + amount + "|" + player.Credits);
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id), "0|LM|ST|CRE|" + amount + "|" + player.Credits);
                    break;
                case RewardType.URIDIUM:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Uridium += amount;
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id), "0|LM|ST|URI|" + amount + "|" + player.Uridium);
                    break;
                case RewardType.EXPERIENCE:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Experience += amount;
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id), "0|LM|ST|EP|" + amount + "|" + player.Experience + "|" + player.Level.Id);
                    break;
                case RewardType.HONOR:
                    amount = RewardMultiplyer(type, amount, player);
                    player.Honor += amount;
                    Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(player.Id),"0|LM|ST|HON|" + amount + "|" + player.Honor);
                    break;
                case RewardType.ORE:
                    break;
                case RewardType.GALAXY_GATES_ENERGY:
                    amount = RewardMultiplyer(type, amount, player);
                    break;
                case RewardType.AMMO:
                    break;
                case RewardType.BOOSTER:
                    // amount => hours
                    break;
            }
        }

        public int RewardMultiplyer(RewardType type, int amount, Player player)
        {
            if (type == RewardType.GALAXY_GATES_ENERGY)
            {
                if (player.Level.Id < 20) return amount*2;
                return amount;
            }
            return amount * player.GetRewardLevel();
        }

        public void RewardPlayer(Player player, RewardType type, Item item, int amount)
        {
            
        }
    }
}
