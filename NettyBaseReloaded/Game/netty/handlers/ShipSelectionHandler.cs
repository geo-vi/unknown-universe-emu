using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipSelectionHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            int targetId = 0;
            if (!gameSession.Player.UsingNewClient)
            {
                var cmd = new commands.old_client.requests.ShipSelectionRequest();
                cmd.readCommand(buffer);
                targetId = cmd.targetId;
            }
            else
            {
                var cmd = new commands.new_client.requests.ShipSelectionRequest();
                cmd.readCommand(buffer);
                targetId = cmd.selectedId;
            }

            var spacemap = gameSession.Player.Spacemap;
            if (spacemap.Entities.ContainsKey(targetId))
            {
                var entity = spacemap.Entities[targetId];
                if (entity.Position.DistanceTo(gameSession.Player.Position) > 1250)
                {
                    //AUTOLOCK
                }
                else
                {
                    if (!entity.Targetable)
                    {
                        Packet.Builder.LegacyModule(gameSession, "0|A|STM|msg_own_targeting_harmed");
                        return;
                    }

                    gameSession.Player.Selected = entity;
                    Packet.Builder.ShipSelectionCommand(gameSession, entity);
                    if (gameSession.Player.RankId == Rank.ADMINISTRATOR)
                    {
                        var msg =
                            $"0|A|STD|{entity.Id}#{entity.Name}\nd.{Math.Round(entity.Position.DistanceTo(gameSession.Player.Position))} [{entity.Position.ToString()}]";
                        if (entity is Npc n)
                        {
                            msg += "\n" + n.Controller.GetAI().Name;
                        }
                        Packet.Builder.LegacyModule(gameSession, msg);
                    }
                }
            }
            else if (spacemap.Objects.ContainsKey(targetId))
            {
                var targetObject = spacemap.Objects[targetId];
                if (targetObject is AttackableAsset attackable)
                {
                    gameSession.Player.Selected = attackable.Core;
                    Packet.Builder.AssetInfoCommand(gameSession, attackable);
                }
            }
        }
    }
}
