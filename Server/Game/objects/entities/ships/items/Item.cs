using Server.Game.objects.enums;

namespace Server.Game.objects.entities.ships.items
{
    /// <summary>
    /// Used for stuff like for stuff like ammo, booty keys, generally unequipable items and every other that exists on player_equipment is EquipmentItem
    /// </summary>
    class Item
    {
        public int Id { get; }

        public int TypeId { get; }

        public string LootId { get; }
        
        public string Name { get; set; }
        
        public bool Activated { get; set; }

        public int Level { get; }

        public int Amount { get; private set; }

        public int PriceCredits { get; set; }

        public int PriceUridium { get; set; }

        public int Uses { get; set; }

        public int Damage { get; set; }

        public int Shield { get; set; }

        public int Speed { get; set; }
        
        public string Category { get; set; }

        public GeneralItemCategories GeneralCategory
        {
            get
            {
                var category = (GeneralItemCategories) TypeId;
                return category;
            }
        }

        public Item(int id, int typeId, string lootId, int level, int amount)
        {
            Id = id;
            TypeId = typeId;
            LootId = lootId;
            Level = level;
            Amount = amount;
        }

        public void SetAmount(int amount)
        {
            Amount = amount;
        }
    }
}