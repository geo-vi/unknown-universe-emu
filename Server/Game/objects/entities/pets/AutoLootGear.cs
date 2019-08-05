using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.maps.objects;

namespace Server.Game.objects.entities.pets
{
    class AutoLootGear : PetGear
    {
        public Box SelectedBox { get; set; }
        
        public AutoLootGear(PetGears type, Item item, bool enabled = true) : base(type, item, enabled)
        {
        }
    }
}