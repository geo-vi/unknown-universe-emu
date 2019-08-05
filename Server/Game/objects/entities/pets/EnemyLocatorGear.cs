using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.pets
{
    class EnemyLocatorGear : PetGear
    {
        public Npc Selected { get; set; }
        
        public EnemyLocatorGear(PetGears type, Item item, bool enabled = true) : base(type, item, enabled)
        {
        }
    }
}