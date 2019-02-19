using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players.equipment.item;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class EquipmentItem
    {
        public int Id;

        public Player Player;

        public Item Item;

        public int Level;

        public EquippedItem OnConfig1;

        public EquippedItem OnConfig2;

        public EquippedItem OnDroneId1;

        public EquippedItem OnDroneId2;

        public EquippedItem OnPet1;

        public EquippedItem OnPet2;

        public int ItemAmount { get; private set; }

        public IEnumerable<int> HangarIds => OnConfig1.Hangars.Concat(OnConfig2.Hangars).Concat(OnDroneId1.Hangars)
            .Concat(OnDroneId2.Hangars).Concat(OnPet1.Hangars).Concat(OnPet2.Hangars);

        public bool Equipped => !OnConfig1.Equipped && !OnConfig2.Equipped && !OnDroneId1.Equipped &&
                                !OnDroneId2.Equipped && !OnPet1.Equipped
                                && !OnPet2.Equipped;

        public EquipmentItem(int id, Player player, Item item, int level, EquippedItem onConfig1, EquippedItem onConfig2,
            EquippedItem onDroneId1, EquippedItem onDroneId2, EquippedItem onPet1, EquippedItem onPet2, int itemAmount)
        {
            Id = id;
            Player = player;
            Item = item;
            Level = level;
            OnConfig1 = onConfig1;
            OnConfig2 = onConfig2;
            OnDroneId1 = onDroneId1;
            OnDroneId2 = onDroneId2;
            OnPet1 = onPet1;
            OnPet2 = onPet2;
            ItemAmount = itemAmount;
        }

        public void SetItemAmount(int newAmount)
        {
            ItemAmount = newAmount;
            World.DatabaseManager.UpdateEquipmentItem(this);
        }

        public void Remove()
        {
            World.DatabaseManager.DeleteEquipmentItem(this);
        }
    }
}
