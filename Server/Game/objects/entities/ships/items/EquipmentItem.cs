using System.Collections.Generic;
using System.Linq;

namespace Server.Game.objects.entities.ships.items
{
    class EquipmentItem : Item
    {
        public int EquipmentItemId { get; }

        public EquippedItem OnConfig1 { get; set; }

        public EquippedItem OnConfig2 { get; set; }

        public EquippedItem OnDroneId1 { get; set; }

        public EquippedItem OnDroneId2 { get; set; }

        public EquippedItem OnPet1 { get; set; }

        public EquippedItem OnPet2 { get; set; }
        
        public IEnumerable<int> HangarIds => OnConfig1.Hangars.Concat(OnConfig2.Hangars).Concat(OnDroneId1.Hangars)
            .Concat(OnDroneId2.Hangars).Concat(OnPet1.Hangars).Concat(OnPet2.Hangars);

        public bool Equipped => !OnConfig1.Equipped && !OnConfig2.Equipped && !OnDroneId1.Equipped &&
                                !OnDroneId2.Equipped && !OnPet1.Equipped
                                && !OnPet2.Equipped;

        public EquipmentItem(int eqId, int itemId, int typeId, string lootId, bool activated, int level, int amount, 
            EquippedItem onConfig1, EquippedItem onConfig2, EquippedItem onDrones1, EquippedItem onDrones2,
            EquippedItem onPet1, EquippedItem onPet2) : base(itemId, typeId, lootId, level, amount)
        {
            EquipmentItemId = eqId;
            OnConfig1 = onConfig1;
            OnConfig2 = onConfig2;
            OnDroneId1 = onDrones1;
            OnDroneId2 = onDrones2;
            OnPet1 = onPet1;
            OnPet2 = onPet2;
            Activated = activated;
        }
    }
}