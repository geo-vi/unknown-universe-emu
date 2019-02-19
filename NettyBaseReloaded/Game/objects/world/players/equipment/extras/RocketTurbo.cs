using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class RocketTurbo : Extra
    {
        public RocketTurbo(Player player, EquipmentItem equipmentItem) : base(player, equipmentItem)
        {
            Active = true;
        }

        public override void execute()
        {
        }
    }
}
