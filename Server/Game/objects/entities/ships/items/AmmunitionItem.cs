using Server.Game.managers;

namespace Server.Game.objects.entities.ships.items
{
    class AmmunitionItem : Item
    {
        public AmmunitionItem(string lootId, int amount) : base(-1, -1, lootId, -1, amount)
        {
        }
    }
}