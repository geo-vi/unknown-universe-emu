using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.gg;

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
            var player = character as Player;
            if (player == null) return;

            if (player.OwnedGates.Any())
            {
                var gate = player.OwnedGates.FirstOrDefault().Value;
                gate.WaitingPhaseEnd = DateTime.Now;
                gate.PlayerJoinMap(player);
                return;
            }

            switch (GalaxyGateId)
            {
                case 1:
                    var alphaWave = player.Gates.GetAlphaWave();
                    var alpha = new AlphaGate(World.StorageManager.Spacemaps[51], alphaWave);
                    alpha.DefineOwner(player);
                    alpha.InitiateVirtualWorld();
                    player.Controller.Miscs.Jump(alpha.Spacemap.Id, Destination, Id, alpha.VWID);
                    alpha.PendingPlayers.TryAdd(player.Id, player);
                    break;
            }
        }
    }
}
