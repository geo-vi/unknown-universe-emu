using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipSelectionHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            int targetId = 0;
            if (!gameSession.Player.UsingNewClient)
            {
                var cmd = new commands.old_client.requests.ShipSelectionRequest();
                cmd.readCommand(bytes);
                targetId = cmd.targetId;
            }
            else
            {
                var cmd = new commands.new_client.requests.ShipSelectionRequest();
                cmd.readCommand(bytes);
                targetId = cmd.selectedId;
            }

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
                        if (!entry.Value.Controller.Attack.Targetable)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|STM|msg_own_targeting_harmed");
                            return;
                        }
                    }
                    gameSession.Player.Selected = entry.Value;
                    Packet.Builder.ShipSelectionCommand(gameSession, entry.Value);
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
