using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.commands
{
    class InfoCommand : Command
    {
        public InfoCommand() : base("info", "Info about server")
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args == null || args.Length < 2)
                return;
            switch (args[1])
            {
                case "players":
                    StringBuilder worldSessions = new StringBuilder();
                    foreach (var worldSession in World.StorageManager.GameSessions)
                        worldSessions.Append($"{worldSession.Key}:{worldSession.Value.Player.Name} ");
                    StringBuilder chatSessions = new StringBuilder();
                    foreach (var chatSession in Chat.Chat.StorageManager.ChatSessions)
                        worldSessions.Append($"{chatSession.Key}:{chatSession.Value.Player.Name} ");
                    Console.WriteLine($"World sessions ({World.StorageManager.GameSessions.Count}): {worldSessions}\nChat sessions ({Chat.Chat.StorageManager.ChatSessions.Count}): {chatSessions}");
                    break;
                case "player":
                    var id = int.Parse(args[2]);
                    var player = World.StorageManager.GetGameSession(id)?.Player;
                    if (player == null) return;
                    Console.WriteLine(player.Name + "'s Statistics ::");
                    Console.WriteLine($"HP: {player.CurrentHealth}/{player.MaxHealth}, SHD: {player.CurrentShield}/{player.MaxShield}, NH:{player.CurrentNanoHull}/{player.MaxNanoHull}");
                    Console.WriteLine($"Range:  entities->{player.Range.Entities.Count}, collectables->{player.Range.Collectables.Count}, objects->{player.Range.Objects.Count}, zones->{player.Range.Zones.Count}");
                    Console.WriteLine($"Visuals: {player.Visuals.Count}");
                    Console.WriteLine($"State: InDemiZone->{player.State.InDemiZone}, InPortalArea->{player.State.InPortalArea}," +
                                      $" Jumping->{player.State.Jumping}, Home->{player.State.IsOnHomeMap()}, InRadiation->{player.State.InRadiationArea}, " +
                                      $"GroupInit-> {player.State.GroupInitialized}");
                    break;
                case "map":
                    var mapId = int.Parse(args[2]);
                    var map = World.StorageManager.Spacemaps[mapId];
                    Console.WriteLine(map.Name + "'s Statistics ::");
                    Console.WriteLine($@"Entities: {map.Entities.Count} (Npcs: {map.Entities.Count(x => x.Value is Npc)}, Players: {map.Entities.Count(x => x.Value is Player)}), Objects: {map.Objects.Count} (Collectables: {map.Objects.Count(x => x.Value is Collectable)}, Portals: {map.Objects.Count(x => x.Value is Jumpgate)})");
                    break;
                case "server":
                    var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    var ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                    break;
            }
        }

        public override void Execute(ChatSession session, string[] args = null) => Execute(args);
    }
}
