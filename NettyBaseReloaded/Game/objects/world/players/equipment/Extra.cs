using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Game.objects.world.players.equipment.item;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    abstract class Extra
    {
        /// <summary>
        /// Player
        /// </summary>
        protected Player Player { get; set; }

        /// <summary>
        /// Equipped Item behind Extra
        /// </summary>
        public EquipmentItem EquipmentItem { get; set; }

        /// <summary>
        /// Extra active
        /// </summary>
        public bool Active = false;

        public virtual int Amount
        {
            get => EquipmentItem.ItemAmount;
            set => SetAmount(value);
        }

        public virtual int Level => 1;

        protected Extra(Player player, EquipmentItem equipmentItem)
        {
            Player = player;
            EquipmentItem = equipmentItem;
        }

        public virtual void initiate()
        {

        }

        public virtual void execute()
        {
            if (Amount == 0)
            {
                Player.Extras.Remove(EquipmentItem.Id);
                Player.UpdateConfig();
            }
        }

        public virtual void SetAmount(int newValue)
        {
            if (newValue <= 0)
            {
                EquipmentItem.Remove();
            }
            EquipmentItem.SetItemAmount(newValue);
        }
    }
}
