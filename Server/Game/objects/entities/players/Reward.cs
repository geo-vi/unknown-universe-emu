using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities.ships.equipment;

namespace Server.Game.objects.entities.players
{
    class Reward
    {
        public ConcurrentBag<Item> BagOfItems = new ConcurrentBag<Item>();

        public Reward()
        {   
        }

        public void CreateItem(Item item)
        {
            BagOfItems.Add(item);
        }
    }
}