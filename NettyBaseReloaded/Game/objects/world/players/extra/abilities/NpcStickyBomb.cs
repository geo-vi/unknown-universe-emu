using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class NpcStickyBomb : Ability
    {
        public NpcStickyBomb(Player player) : base(player, Abilities.NPC_ABILITY_HALLOWEEN_STICKYBOMB)
        {
        }

        public override void Tick()
        {
            
        }

        public override void execute()
        {
        }
    }
}
