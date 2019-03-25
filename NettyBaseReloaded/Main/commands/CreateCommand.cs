using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.pois;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Main.commands
{
    class CreateCommand : Command
    {
        public CreateCommand() : base("create", "The God creator command") { }

        private static List<Vector> _savedCords = new List<Vector>();

        public override void Execute(string[] args = null)
        {
            try
            {
                if (args?.Length > 1)
                {
                    if (args[1] == "lowgate")
                    {
                        var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                        playerSession.Player.Spacemap.CreateLoW(playerSession.Player.Position);
                    }
                    if (args[1] == "relay")
                    {
                        var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                        playerSession.Player.Spacemap.CreateRelayStation(playerSession.Player.Position);
                    }
                    if (args[1] == "save")
                    {
                        var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                        if (_savedCords.Count == 0)
                        {
                            _savedCords.Add(playerSession.Player.Position);
                            Console.WriteLine("Saved point 1");
                        }
                        else if (_savedCords.Count == 1)
                        {
                            var firstPoint = _savedCords[0];
                            var point = playerSession.Player.Position;

                            var vectorPointDifference1 = new Vector(firstPoint.X, point.Y);
                            var vectorPointDifference2 = new Vector(point.X, firstPoint.Y);
                            _savedCords.Add(point);
                            _savedCords.Add(vectorPointDifference1);
                            _savedCords.Add(vectorPointDifference2);
                            Console.WriteLine($"Finished saving!\n{JsonConvert.SerializeObject(_savedCords)}");
                        }
                        else if (args.Length > 3) _savedCords.Clear();
                    }
                    if (args[1] == "poi")
                    {
                        var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));
                        string poiId;
                        if (args.Length > 3)
                        {
                            poiId = args[3];
                        }
                        else poiId = "poi_" + playerSession.Player.Spacemap.POIs.Count;
                        playerSession.Player.Spacemap.CreatePOI(new POI(poiId, Types.NO_ACCESS,
                            Designs.SIMPLE, Shapes.RECTANGLE, _savedCords, true, false, ""));
                        Console.WriteLine($"{poiId} Created @{playerSession.Player.Spacemap.Name}, {JsonConvert.SerializeObject(_savedCords)}");
                        _savedCords.Clear();
                    }
                    if (args[1] == "dump")
                    {
                        var playerSession = World.StorageManager.GetGameSession(int.Parse(args[2]));

                        var file = File.Create(Directory.GetCurrentDirectory() + "/saved.poi");
                        var bytes = Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(playerSession.Player.Spacemap.POIs));
                        file.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error -> createCommand");
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            
        }
    }
}