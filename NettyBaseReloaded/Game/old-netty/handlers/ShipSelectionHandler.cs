using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipSelectionHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            if (gameSession == null) return;

            var targetId = parser.Int();

            var spacemap = gameSession.Player.Spacemap;
            try
            {
                foreach (
                    var entry in
                    spacemap.Entities.Where(
                        entry => entry.Value.Id == targetId && gameSession.Player.InRange(entry.Value)))
                {
                    if (entry.Value is Player)
                    {
                        if (!entry.Value.Controller.Targetable)
                        {
                            gameSession.Client.Send(PacketBuilder.LegacyModule("0|A|STM|msg_own_targeting_harmed").Bytes);
                            return;
                        }
                    }
                    gameSession.Player.Selected = entry.Value;
                    gameSession.Client.Send(PacketBuilder.ShipSelectionCommand(entry.Value));
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

        }
    }
}
