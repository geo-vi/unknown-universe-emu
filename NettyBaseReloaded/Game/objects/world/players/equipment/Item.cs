using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Item
    {
        public int Id { get; set; }

        public string LootId { get; set; }

        public int Amount { get; set; }

        public Item(int id, string lootId, int amount)
        {
            Id = id;
            LootId = lootId;
            Amount = amount;
        }

        public Item(string lootId, int amount)
        {
            Id = -1;
            LootId = lootId;
            Amount = amount;
        }
    }
}
