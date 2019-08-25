using System;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.pets
{
    class KamikazeGear : PetGear
    {
        public int RangeOfDamage { get; private set; }
        
        public int Damage { get; private set; }
        
        public AbstractAttackable SelectedTarget { get; set; }
        
        public DateTime EstimatedTimeOfExplosion { get; set; }
        
        public KamikazeGear(PetGears type, Item item) : base(type, item)
        {
        }
    }
}