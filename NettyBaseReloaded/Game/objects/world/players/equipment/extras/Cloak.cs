using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class Cloak : Extra
    {
        public override int Level => GetLevel();

        public Cloak(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public int GetLevel()
        {
            switch (EquipmentItem.Item.Id)
            {
                case 73:
                    return 2;
                case 74:
                    return 1;
            }

            return 0;
        }

        public override void execute()
        {
            base.execute();
            Player.Controller.CPUs.Activate(CPU.Types.CLOAK);
        }
    }
}
