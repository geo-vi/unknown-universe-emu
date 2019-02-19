using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class AutoRocketLauncher : Extra
    {
        public AutoRocketLauncher(Player player, EquipmentItem equipment) : base(player, equipment)
        {
        }

        public override void execute()
        {
            Player.Controller.CPUs.Activate(CPU.Types.AUTO_ROCKLAUNCHER);
        }
    }
}
