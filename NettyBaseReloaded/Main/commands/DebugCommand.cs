using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Main.commands
{
    class DebugCommand : Command
    {
        public DebugCommand() : base("debug", "Debug command")
        {
            
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                if (args == null || args.Length < 2)
                {
                    if (Properties.Server.DEBUG)
                    {
                        Properties.Server.DEBUG = false;
                        Properties.Game.PRINTING_COMMANDS = false;
                        Properties.Game.PRINTING_LEGACY_COMMANDS = false;
                        Properties.Game.DEBUG_ENTITIES = false;
                        Properties.Game.PRINTING_CONNECTIONS = false;
                        Console.WriteLine("Debug::Session ended");
                    }
                    else
                    {
                        Properties.Server.DEBUG = true;
                        Console.WriteLine("Debug::Session started");
                    }

                    return;
                }

                if (!Properties.Server.DEBUG)
                {
                    Console.WriteLine("Access Denied!");
                }

                Player player = null;
                var playerId = 0;
                if (args.Length > 2)
                    playerId = int.Parse(args[2]);

                switch (args[1])
                {
                    case "commands":
                    case "printcmd":
                    case "printcmds":
                        if (Properties.Game.PRINTING_COMMANDS)
                        {
                            Properties.Game.PRINTING_COMMANDS = false;
                            Console.WriteLine("Debug::Stopped printing commands");
                            break;
                        }

                        Properties.Game.PRINTING_COMMANDS = true;
                        Console.WriteLine("Debug::Commands should now print");
                        break;
                    case "packets":
                    case "printpacket":
                    case "printpackets":
                        if (Properties.Game.PRINTING_LEGACY_COMMANDS)
                        {
                            Properties.Game.PRINTING_LEGACY_COMMANDS = false;
                            Console.WriteLine("Debug::Stopped printing legacy commands");
                            break;
                        }

                        Properties.Game.PRINTING_LEGACY_COMMANDS = true;
                        Console.WriteLine("Debug::Legacy commands should now print");
                        break;
                    case "range":
                    case "entities":
                        if (Properties.Game.DEBUG_ENTITIES)
                        {
                            Properties.Game.DEBUG_ENTITIES = false;
                            Console.WriteLine("Debug::Stopped printing range entities");
                            break;
                        }

                        Properties.Game.DEBUG_ENTITIES = true;
                        Console.WriteLine("Debug::Range entities should now print");
                        break;
                    case "connections":
                        if (Properties.Game.PRINTING_CONNECTIONS)
                        {
                            Properties.Game.PRINTING_CONNECTIONS = false;
                            Console.WriteLine("Debug::Stopped printing connections");
                            break;
                        }

                        Properties.Game.PRINTING_CONNECTIONS = true;
                        Console.WriteLine("Debug::Player connections are now printing");
                        break;
                    case "send":
                        Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(playerId), args[3]);
                        break;
                    case "listen":
                        var session = World.StorageManager.GetGameSession(playerId);
                        //session.Client.Listening = !session.Client.Listening;
                        break;
                    case "near":
                        player = World.StorageManager.GetGameSession(playerId).Player;
                        var entities =
                            player.Spacemap.Entities.Where(x => x.Value.Position.DistanceTo(player.Position) <= 2000);
                        foreach (var entry in entities)
                        {
                            Console.WriteLine(entry.Value.Name + " " + entry.Value.FactionId + " " +
                                              entry.Value.Position.ToPacket() + " sel me" +
                                              (entry.Value.SelectedCharacter == player) + " ");
                        }

                        break;
                    case "state":
                        player = World.StorageManager.GetGameSession(playerId).Player;
                        Console.WriteLine(player.EntityState + ":Controller Active: " + player.Controller.Active +
                                          ", StopController: " + player.Controller.StopController +
                                          ", Checked Classes: " + player.Controller.CheckedClasses.Count);
                        Console.WriteLine("Attacking: " + player.Controller.Attack.Attacking + ", Position: " +
                                          player.Position.ToPacket() + " Range: E:" + player.Range.Entities.Count +
                                          " O: " + player.Range.Objects.Count + " C: " +
                                          player.Range.Collectables.Count + " Z: " + player.Range.Zones.Count);
                        Console.WriteLine("tickers: player: " + Global.TickManager.Exists(player) + " ; controller: " +
                                          Global.TickManager.Exists(player.Controller));
                        Console.WriteLine("cooldowns: " + player.Cooldowns.CooldownDictionary.Count);
                        Console.WriteLine("session: " + player.GetGameSession() + ", active:" +
                                          player.GetGameSession().Active + ", " +
                                          World.StorageManager.GetGameSession(playerId));
                        break;
                    case "activeattackers":
                        player = World.StorageManager.GetGameSession(playerId).Player;
                        foreach (var activeAttacker in player.Controller.Attack.GetActiveAttackers())
                        {
                            Console.WriteLine(activeAttacker.Name + " : " + activeAttacker.Spacemap.Name + " " +
                                              activeAttacker.Position.ToPacket() + " distance: " +
                                              activeAttacker.Position.DistanceTo(player.Position) + " state: " +
                                              activeAttacker.EntityState + " controller: " +
                                              activeAttacker.Controller.Active);
                        }

                        break;
                    case "allselected":
                        player = World.StorageManager.GetGameSession(playerId).Player;

                        foreach (var spacemap in World.StorageManager.Spacemaps.Where(x =>
                            x.Value.Entities.Any(y => y.Value.SelectedCharacter == player)))
                        {
                            foreach (var entity in spacemap.Value.Entities.Values.Where(x =>
                                x.SelectedCharacter == player))
                            {
                                Console.WriteLine(entity.Name + " : " + entity.Spacemap.Name + " " +
                                                  entity.Position.ToPacket() + " distance: " +
                                                  entity.Position.DistanceTo(player.Position) + " controller: " +
                                                  entity.Controller.Active);
                            }
                        }

                        break;
                    case "mapinfo":
                        player = World.StorageManager.GetGameSession(playerId).Player;
                        Console.WriteLine(player.Spacemap.Id + ":" + player.Spacemap.Name + " - Info");
                        Console.WriteLine("Entities: " + player.Spacemap.Entities.Count + ", Objects: " +
                                          player.Spacemap.Objects.Count);
                        foreach (var entity in player.Spacemap.Entities.Values)
                        {
                            Console.WriteLine("ID: " + entity.Id + ";" + entity.Name + ";tick: (character)" +
                                              Global.TickManager.Exists(entity) + ",(controller)" +
                                              Global.TickManager.Exists(entity.Controller) + "," + entity.Position);
                        }

                        break;
                    case "equipment":
                        player = World.StorageManager.GetGameSession(playerId).Player;
                        Console.WriteLine("Equipment of " + player.Name);
                        foreach (var eq in player.Equipment.EquipmentItems)
                        {
                            Console.WriteLine(eq.Value.Id + "::" + eq.Value.Item.LootId + "[" + eq.Value.Item.Id + "]");
                        }
                        break;
                }
            }
            catch (Exception) 
            {
            
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var id = session.Player.Id;
            var sessionId = session.Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId &&
                worldSession.Player.RankId == Rank.ADMINISTRATOR)
            {
                Execute(args);
            }

        }
    }
}
