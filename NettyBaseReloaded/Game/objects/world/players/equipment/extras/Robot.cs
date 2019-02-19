using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class Robot : Extra
    {
        public override int Level => GetLevel();

        public Robot(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public int GetLevel()
        {
            switch (EquipmentItem.Item.Id)
            {
                case 55:
                    return 1;
                case 56:
                    return 2;
                case 57:
                    return 3;
                case 58:
                    return 4;
            }
            return 0;
        }

        public override void execute()
        {
            Player.Controller.Repairing = true;
            Player.Controller.CPUs.Activate(CPU.Types.ROBOT);
        }
    }
}
