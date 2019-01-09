﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class EquipmentCommand : Command
    {
        public EquipmentCommand() : base("equipment", "", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var gameSession = session.GetEquivilentGameSession();
            if (gameSession != null)
            {
                var player = gameSession.Player;
                if (player.State.InEquipmentArea)
                {
                    player.Hangar = World.DatabaseManager.LoadHangar(player);
                    player.Hangar.Configurations = World.DatabaseManager.LoadConfig(player);
                    player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
                    foreach (var playerEntity in player.Spacemap.Entities.Where(x => x.Value is Player))
                    {
                        var entitySession = World.StorageManager.GetGameSession(playerEntity.Value.Id);
                        if (entitySession != null)
                            Packet.Builder.DronesCommand(entitySession, player);
                    }
                    player.Refresh();
                    Chat.packet.Packet.Builder.SystemMessage(session, "Refreshed your equipment.");
                }
                else Chat.packet.Packet.Builder.SystemMessage(session, "Please enter equipment zone.");
            }
        }
    }
}
