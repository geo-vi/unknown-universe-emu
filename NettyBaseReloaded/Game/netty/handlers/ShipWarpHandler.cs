using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipWarpHandler : IHandler
    {
        private Player Player;
        private Hangar TargetHangar;

        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            Packet.Builder.ShipWarpNotAllowedCommand(gameSession);
            return;

            var request = new ShipWarpRequest();
            request.readCommand(buffer);

            Player = gameSession.Player;
            var shipType = request.shipType;
            TargetHangar = Player.Equipment.Hangars.FirstOrDefault(x => x.Value.Ship.Id == shipType).Value;
            if (TargetHangar != null)
            {
                if (Player.Moving || Player.Controller.Attack.Attacking ||
                    Player.Controller.Attack.GetActiveAttackers().Count > 0)
                {
                    Packet.Builder.ShipWarpNotAllowedCommand(gameSession);
                    return;
                }
                Task.Factory.StartNew(HangarSwitchLoop);
            }
        }

        private async void HangarSwitchLoop()
        {
            var map = Player.Spacemap;
            Vector startVector = MovementController.ActualPosition(Player);
            GameSession session = null;
            var endTime = DateTime.Now.AddSeconds(10);
            Player.Visuals.TryAdd(ShipVisuals.SHIP_WARP, new VisualEffect(Player, ShipVisuals.SHIP_WARP, endTime));
            while (Player.Controller.Active && !Player.Controller.StopController)
            {
                session = Player.GetGameSession();
                if (session == null || Player.Position != startVector || Player.Moving || Player.Controller.Attack.Attacking || Player.Controller.Attack.GetActiveAttackers().Count > 0)
                {
                    Packet.Builder.ShipWarpCanceledCommand(session);
                    return; // cancel
                }
                if (DateTime.Now >= endTime) break;
                Packet.Builder.LegacyModule(session, "0|A|STM|msg_swcountdown_p|%SECONDS%|" + -(DateTime.Now - endTime).Seconds);
                await Task.Delay(1000);
            }
            Packet.Builder.ShipWarpCompletedCommand(session);
            TargetHangar.Spacemap = map;
            TargetHangar.Position = startVector;
            Player.Equipment.ChangeHangar(TargetHangar);
        }
    }
}
