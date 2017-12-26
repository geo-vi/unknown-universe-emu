using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.player;

namespace NettyBaseReloaded.Game.objects.world.players.equipment.extras
{
    class Turbo : Extra
    {
        public Turbo(Player player, int itemId, string lootId, int amount) : base(player, itemId, lootId, amount)
        {
            Active = true;
        }

        public override void execute()
        {
        }
    }
}
