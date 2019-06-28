namespace Server.Game.objects.entities.ships.equipment
{
    class Item
    {
        public int Id;

        public int TypeId;

        public string LootId;
        
        public bool Activated;

        public int Level;

        public int Amount;

        public Item(int id, int typeId, string lootId, bool activated, int level, int amount)
        {
            Id = id;
            TypeId = typeId;
            LootId = lootId;
            Activated = activated;
            Level = level;
            Amount = amount;
        }
    }
}