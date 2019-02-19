using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.extra.abilities
{
    class VenomSingularity : Ability
    {
        public VenomSingularity(Player player) : base(player, Abilities.SHIP_ABILITY_VENOM_SINGULARITY)
        {
        }

        public override void Tick()
        {
        }

        public override void execute()
        {
            Packet.Builder.LegacyModule(Player.GetGameSession(), "0|A|STD|Not yet implemented");
        }
    }
}
