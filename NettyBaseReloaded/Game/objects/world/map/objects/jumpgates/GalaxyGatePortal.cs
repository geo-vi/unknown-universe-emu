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

            //Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|Gates are currently getting tested live on the server by admins. " +
            //                                                     "Please be patient ))");
            //return;
            
            if (player.OwnedGates.Any())
            {
                var gate = player.OwnedGates.FirstOrDefault().Value;
                gate.WaitingPhaseEnd = DateTime.Now;
                player.MoveToMap(gate.Spacemap, Destination, gate.VWID);
                gate.PlayerJoinMap(player);
                return;
            }

            switch ((GalaxyGates) GalaxyGateId)
            {
                case GalaxyGates.ALPHA:
                    if (!player.Gates.AlphaReady) return;
                    var alphaWave = player.Gates.GetWave(GalaxyGateId) - 1;
                    var alpha = new AlphaGate(World.StorageManager.Spacemaps[51], alphaWave);
                    alpha.DefineOwner(player);
                    alpha.InitiateVirtualWorld();
                    player.Controller.Miscs.Jump(alpha.Spacemap.Id, Destination, Id, alpha.VWID);
                    alpha.PendingPlayers.TryAdd(player.Id, player);
                    break;
                case GalaxyGates.BETA:
                    if (!player.Gates.BetaReady) return;
                    var betaWave = player.Gates.GetWave(GalaxyGateId) - 1;
                    var beta = new BetaGate(World.StorageManager.Spacemaps[52], betaWave);
                    beta.DefineOwner(player);
                    beta.InitiateVirtualWorld();
                    player.Controller.Miscs.Jump(beta.Spacemap.Id, Destination, Id, beta.VWID);
                    beta.PendingPlayers.TryAdd(player.Id, player);
                    break;
                case GalaxyGates.GAMMA:
                    if (!player.Gates.GammaReady) return;
                    var gammaWave = player.Gates.GetWave(GalaxyGateId) - 1;
                    var gamma = new GammaGate(World.StorageManager.Spacemaps[53], gammaWave);
                    gamma.DefineOwner(player);
                    gamma.InitiateVirtualWorld();
                    player.Controller.Miscs.Jump(gamma.Spacemap.Id, Destination, Id, gamma.VWID);
                    gamma.PendingPlayers.TryAdd(player.Id, player);
                    break;
                default:
                    Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|Gates are currently getting tested live on the server by admins. Please be patient ))");
                    break;
            }
        }
    }
}
