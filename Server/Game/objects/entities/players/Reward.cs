using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.players
{
    class Reward
    {
        public ConcurrentBag<Item> BagOfItems = new ConcurrentBag<Item>();

        public ConcurrentBag<RewardInformation> InformationRewards = new ConcurrentBag<RewardInformation>();
        
        public Reward()
        {   
        }

        public Reward(Dictionary<RewardTypes, int> rewardDictionary)
        {
            foreach (var reward in rewardDictionary)
            {
                InformationRewards.Add(new RewardInformation(reward.Key, reward.Value));
            }
        }

        public void CreateItem(Item item)
        {
            BagOfItems.Add(item);
        }
    }
}