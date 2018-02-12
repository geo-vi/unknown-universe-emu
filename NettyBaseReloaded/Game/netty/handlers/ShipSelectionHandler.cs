using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;

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
                var entityEntries = spacemap.Entities.Where(
                    entry => entry.Value.Id == targetId /*&& gameSession.Player.InRange(entry.Value)*/);

                if (!entityEntries.Any())
                {
                    var selectedAsset = gameSession.Player.Spacemap.Objects.FirstOrDefault(x => x.Key == targetId).Value as AttackableAsset;
                    if (selectedAsset != null)
                    {
                        gameSession.Player.Selected = selectedAsset.Core;
                        Packet.Builder.AssetInfoCommand(gameSession, selectedAsset);
                    }
                    return;
                }

                foreach (
                    var entry in entityEntries) // temp
                {
                    if (entry.Value is Player)
                    {
                        if (!entry.Value.Targetable)
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
