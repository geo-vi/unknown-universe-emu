namespace Server.Game.objects.entities.ships.equipment
{
    class EquipmentItem : Item
    {
        public EquipmentItem(int id, int typeId, string lootId, bool activated, int level, int amount) : base(id, typeId, lootId, activated, level, amount)
        {
        }
    }
}