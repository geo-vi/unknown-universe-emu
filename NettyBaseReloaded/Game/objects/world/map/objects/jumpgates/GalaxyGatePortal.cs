using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class GalaxyGatePortal : Jumpgate
    {
        private int GalaxyGateId; 

        public GalaxyGatePortal(Player owner, int id, int ggId, Vector pos, Spacemap map, Vector destinationPos, int destinationMapId, PortalGraphics gfx) : base(id, Faction.NONE, pos, map, destinationPos, destinationMapId, true, 0, 0, gfx)
        {
            Owner = owner;
            GalaxyGateId = ggId;
        }

        public override void click(Character character)
        {
            if (character is Player player)
            {
                Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|Work in progress");
            }
        }
    }
}
