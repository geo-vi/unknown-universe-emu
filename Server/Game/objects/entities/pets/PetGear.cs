using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities.pets
{
    abstract class PetGear
    {
        public bool Active { get; set; }

        public bool Enabled { get; private set; }
        
        public PetGears Type { get; private set; }

        public Item Item { get; private set; }

        protected PetGear(PetGears type, Item item, bool enabled = true)
        {
            Type = type;
            Item = item;
            Enabled = enabled;
        }
    }
}