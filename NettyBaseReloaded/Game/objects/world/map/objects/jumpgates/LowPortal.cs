using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.gg;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class LowPortal : Jumpgate
    {
        public LowPortal(int id, Vector pos, Spacemap map, int vw) : base(id, Faction.NONE, pos, map, new Vector(1000, 11800), 200, true, 0, 0, PortalGraphics.GROUP_GATE_1)
        {
            DestinationVirtualWorldId = vw;
        }

        public override void click(Character character)
        {
            var player = character as Player;
            if (player == null) return;
            if (!Working && DisabledMessage != "")
            {
                Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|" + DisabledMessage);
                return;
            }

            if (player.Group == null)
            {
                //TODO Send message player not in group
                return;
            }

            // If any other group member has the gate
            var groupMemberWithGateInitiated =
                player.Group.Members.FirstOrDefault(x => x.Value.OwnedGates.Any(y => y.Value is LowGate));

            if (groupMemberWithGateInitiated.Value != null)
            {
                var low = groupMemberWithGateInitiated.Value.OwnedGates.FirstOrDefault(x => x.Value is LowGate);
                if (low.Value.VWID != 0 && low.Value.VirtualMap != null)
                {
                    player.Controller.Miscs.Jump(low.Value.Spacemap.Id, Destination, Id, low.Value.VWID);
                    low.Value.PendingPlayers.TryAdd(player.Id, player);
                }
            }
            else
            {
                var low = new LowGate(0, World.StorageManager.Spacemaps[200]);
                low.DefineOwner(player);
                low.InitiateVirtualWorld();
                player.Controller.Miscs.Jump(low.Spacemap.Id, Destination, Id, low.VWID);
                low.PendingPlayers.TryAdd(player.Id, player);
            }
        }
    }
}
