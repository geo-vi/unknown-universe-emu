using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class AimCpu : Extra
    {
        public override int Amount => Player.Information.Cargo.Xenomit / 10;

        public AimCpu(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
        }

        public override void execute()
        {
        }
    }
}
