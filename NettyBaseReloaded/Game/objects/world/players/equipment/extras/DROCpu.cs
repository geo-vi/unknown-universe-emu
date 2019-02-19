using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class DROCpu : Extra
    {
        public override int Level => GetLevel();

        public DROCpu(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public int GetLevel()
        {
            switch (EquipmentItem.Item.Id)
            {
                case 84:
                    return 1;
                case 85:
                    return 2;
            }

            return 0;
        }
    }
}
