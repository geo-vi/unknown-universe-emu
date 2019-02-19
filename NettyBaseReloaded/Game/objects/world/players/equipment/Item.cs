using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.equipment.item;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Item
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        public EquippedItemCategories Category => (EquippedItemCategories) TypeId;

        public string LootId { get; set; }

        public int Amount { get; set; }

        public Item(int id, int typeId, string lootId, int amount)
        {
            Id = id;
            TypeId = typeId;
            LootId = lootId;
            Amount = amount;
        }

        public Item(int id, string lootId, int amount)
        {
            Id = id;
            LootId = lootId;
            Amount = amount;
        }

        public static Item Find(int id)
        {
            if (World.StorageManager.Items.ContainsKey(id))
            {
                var item = World.StorageManager.Items[id];
                return item;
            }

            return null;
        }

        public static Item Find(string lootId)
        {
            var item = World.StorageManager.Items.FirstOrDefault(x => x.Value.LootId == lootId);
            return item.Value;
        }
    }
}
