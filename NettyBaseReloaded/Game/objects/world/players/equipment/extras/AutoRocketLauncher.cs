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
        public AutoRocketLauncher(Player player, int itemId, string lootId, int amount) : base(player, itemId, lootId, amount)
        {
        }

        public override void execute()
        {
            base.execute();
            Player.Controller.CPUs.Activate(CPU.Types.AUTO_ROCKLAUNCHER);
        }
    }
}
