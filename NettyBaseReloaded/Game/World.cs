using System;
using System.Collections.Generic;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.pois;
using Types = NettyBaseReloaded.Game.objects.world.map.collectables.Types;

namespace NettyBaseReloaded.Game
{
    class World
    {    
        public static managers.StorageManager StorageManager = new StorageManager();
        public static managers.DatabaseManager DatabaseManager = new DatabaseManager();
        public static DebugLog Log = new DebugLog("world");

        public static void InitiateManagers()
        {
            Packet.Handler.AddCommands();
            DatabaseManager.Initiate();
            InitiateWorld();
        }

        private static void InitiateWorld()
        {
            foreach (var map in StorageManager.Spacemaps)
            {
                map.Value.LoadObjects();
                CreateHashes(map.Value);

                if (map.Key == 16)
                {
                    map.Value.CreateStation(Faction.MMO, new Vector(4000, 4000));
                    map.Value.CreateStation(Faction.EIC, new Vector(37600, 4000));
                    map.Value.CreateStation(Faction.VRU, new Vector(37600, 21600));
                    #region POIs
                    map.Value.CreatePOI(new POI("poi_01", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(25700, 19700),
                            new Vector(25800, 22200),
                            new Vector(25700, 22200),
                            new Vector(25800, 19700)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_02", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(25600, 22700),
                            new Vector(25700, 24300),
                            new Vector(25600, 24300),
                            new Vector(25700, 22700)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_03", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(25600, 24700),
                            new Vector(25700, 25500),
                            new Vector(25600, 25500),
                            new Vector(25700, 24700)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_04", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(24800, 24900),
                            new Vector(25600, 25200),
                            new Vector(24800, 25200),
                            new Vector(25600, 24900)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_05", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(24800, 22900),
                            new Vector(24900, 25100),
                            new Vector(24800, 25100),
                            new Vector(24900, 22900)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_06", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(24800, 22700),
                            new Vector(25600, 22900),
                            new Vector(24800, 22900),
                            new Vector(25600, 22700)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_07", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(21100, 20200),
                            new Vector(25700, 20500),
                            new Vector(21100, 20500),
                            new Vector(25700, 20200)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_08", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(24800, 21000),
                            new Vector(25100, 22800),
                            new Vector(24800, 22800),
                            new Vector(25100, 21000)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_09", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(20800, 19200),
                            new Vector(21100, 20500),
                            new Vector(20800, 20500),
                            new Vector(21100, 19200)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_10", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(21000, 19200),
                            new Vector(24900, 19300),
                            new Vector(21000, 19300),
                            new Vector(24900, 19200)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_11", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(17400, 19400),
                            new Vector(20100, 19900),
                            new Vector(17400, 19900),
                            new Vector(20100, 19400)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_12", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(17400, 20400),
                            new Vector(20800, 20600),
                            new Vector(17400, 20600),
                            new Vector(20800, 20400)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_13", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14400, 19500),
                            new Vector(16900, 19700),
                            new Vector(14400, 19700),
                            new Vector(16900, 19500)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_14", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14400, 20200),
                            new Vector(16900, 20400),
                            new Vector(14400, 20400),
                            new Vector(16900, 20200)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_15", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(13400, 19100),
                            new Vector(13600, 20800),
                            new Vector(13400, 20800),
                            new Vector(13600, 19100)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_16", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(12400, 20700),
                            new Vector(14600, 21000),
                            new Vector(12400, 21000),
                            new Vector(14600, 20700)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_17", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14400, 20300),
                            new Vector(14600, 20700),
                            new Vector(14400, 20700),
                            new Vector(14600, 20300)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_18", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(7800, 19500),
                            new Vector(11500, 19700),
                            new Vector(7800, 19700),
                            new Vector(11500, 19500)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_19", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(12300, 19400),
                            new Vector(13400, 19600),
                            new Vector(12300, 19600),
                            new Vector(13400, 19400)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_20", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(7800, 20100),
                            new Vector(12700, 20700),
                            new Vector(7800, 20700),
                            new Vector(12700, 20100)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_21", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(7700, 18500),
                            new Vector(8000, 19600),
                            new Vector(7700, 19600),
                            new Vector(8000, 18500)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_22", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(5800, 18400),
                            new Vector(8000, 18800),
                            new Vector(5800, 18800),
                            new Vector(8000, 18400)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_23", objects.world.map.pois.Types.NO_ACCESS,
                        Designs.SIMPLE, Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(5600, 17100),
                            new Vector(5800, 18700),
                            new Vector(5600, 18700),
                            new Vector(5800, 17100)
                        }, true, false, "scrap_pirates"));
                    map.Value.CreatePOI(new POI("poi_24", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(5100, 17100),
                            new Vector(8100, 17700),
                            new Vector(5100, 17700),
                            new Vector(8100, 17100)
                        }));
                    map.Value.CreatePOI(new POI("poi_25", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(7300, 15300),
                            new Vector(8100, 17100),
                            new Vector(7300, 17100),
                            new Vector(8100, 15300)
                        }));
                    map.Value.CreatePOI(new POI("poi_26", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(6300, 14400),
                            new Vector(8100, 14700),
                            new Vector(6300, 14700),
                            new Vector(8100, 14400)
                        }));
                    map.Value.CreatePOI(new POI("poi_27", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(4900, 13600),
                            new Vector(6200, 16300),
                            new Vector(4900, 16300),
                            new Vector(6200, 13600)
                        }));
                    map.Value.CreatePOI(new POI("poi_28", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(6000, 16600),
                            new Vector(6300, 17100),
                            new Vector(6000, 17100),
                            new Vector(6300, 16600)
                        }));
                    map.Value.CreatePOI(new POI("poi_29", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(5100, 16300),
                            new Vector(5600, 16800),
                            new Vector(5100, 16800),
                            new Vector(5600, 16300)
                        }));
                    map.Value.CreatePOI(new POI("poi_30", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(1500, 15800),
                            new Vector(4400, 16200),
                            new Vector(1500, 16200),
                            new Vector(4400, 15800)
                        }));
                    map.Value.CreatePOI(new POI("poi_31", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(3900, 14900),
                            new Vector(4300, 15800),
                            new Vector(3900, 15800),
                            new Vector(4300, 14900)
                        }));
                    map.Value.CreatePOI(new POI("poi_32", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(1400, 14500),
                            new Vector(3200, 14800),
                            new Vector(1400, 14800),
                            new Vector(3200, 14500)
                        }));
                    map.Value.CreatePOI(new POI("poi_33", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(0, 13000),
                            new Vector(1400, 14800),
                            new Vector(0, 14800),
                            new Vector(1400, 13000)
                        }));
                    map.Value.CreatePOI(new POI("poi_34", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(2400, 13000),
                            new Vector(4400, 14100),
                            new Vector(2400, 14100),
                            new Vector(4400, 13000)
                        }));
                    map.Value.CreatePOI(new POI("poi_35", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(3700, 14000),
                            new Vector(4200, 14500),
                            new Vector(3700, 14500),
                            new Vector(4200, 14000)
                        }));
                    map.Value.CreatePOI(new POI("poi_36", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(800, 14800),
                            new Vector(1000, 16200),
                            new Vector(800, 16200),
                            new Vector(1000, 14800)
                        }));
                    map.Value.CreatePOI(new POI("poi_37", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14400, 1200),
                            new Vector(14700, 1500),
                            new Vector(14400, 1500),
                            new Vector(14700, 1200)
                        }));
                    map.Value.CreatePOI(new POI("poi_38", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14500, 2500),
                            new Vector(14700, 2800),
                            new Vector(14500, 2800),
                            new Vector(14700, 2500)
                        }));
                    map.Value.CreatePOI(new POI("poi_39", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(14300, 3400),
                            new Vector(14600, 3700),
                            new Vector(14300, 3700),
                            new Vector(14600, 3400)
                        }));
                    map.Value.CreatePOI(new POI("poi_40", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(15600, 3700),
                            new Vector(16300, 3900),
                            new Vector(15600, 3900),
                            new Vector(16300, 3700)
                        }));
                    map.Value.CreatePOI(new POI("poi_41", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(17900, 3600),
                            new Vector(19000, 3800),
                            new Vector(17900, 3800),
                            new Vector(19000, 3600)
                        }));
                    map.Value.CreatePOI(new POI("poi_42", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(20100, 2000),
                            new Vector(20300, 2900),
                            new Vector(20100, 2900),
                            new Vector(20300, 2000)
                        }));
                    map.Value.CreatePOI(new POI("poi_43", objects.world.map.pois.Types.NO_ACCESS, Designs.SIMPLE,
                        Shapes.RECTANGLE,
                        new List<Vector>
                        {
                            new Vector(19600, 600),
                            new Vector(20000, 1100),
                            new Vector(19600, 1100),
                            new Vector(20000, 600)
                        }));
                    #endregion
                    map.Value.CreateLoW(new Vector(17300, 1900));
                    map.Value.CreatePalladiumField();
                    map.Value.CreateAdvertisementBanner(0, new Vector(15800, 13500));
                    map.Value.CreateHiddenPortal(42, 15800, 13500, 10400, 12800);
                    map.Value.CreateAsteroid("Shock", new Vector(28000, 7000));
                    map.Value.CreateQuestGiver(Faction.NONE, new Vector(18500, 2500));
                }

                if (map.Key == 42) map.Value.CreatePortal(16, 10400, 12800, 0,0);
                if (map.Key == 200)
                {
                    // Load LoW
                    map.Value.CreateHealthStation(new Vector(10400, 6400));
                    map.Value.CreateRelayStation(new Vector(2500, 2000));
                    map.Value.CreateRelayStation(new Vector(6200, 11700));
                    map.Value.CreateRelayStation(new Vector(18300, 10900));
                    map.Value.CreateRelayStation(new Vector(18200, 4000));
                }

                if (map.Key == 42)
                {
                    // TODO Add >?? x4 Spawn
                }
                else if (map.Key != 200 && map.Key != 54 && map.Key != 53 && map.Key != 52 && map.Key != 51 && map.Key != 16)
                {
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        //map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value, map.Value.Limits[0].X, map.Value.Limits[0].Y, map.Value.Limits[1].X, map.Value.Limits[1].Y));
                    }
                }
            }
            Log.Write("Loaded World");
        }

        private static void CreateHashes(Spacemap map)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[4];
            const int HASHES = 1000;

            for (int entry = 0; entry < HASHES; entry++)
            {
                NEWHASH:
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[Random.Next(chars.Length)];
                }

                var hash = new String(stringChars);
                if (map.HashedObjects.ContainsKey(hash))
                    goto NEWHASH;
                map.HashedObjects.TryAdd(hash, null);
            }
            map.Objects.TryAdd(0, null);
            if (Properties.Server.DEBUG)
                Console.WriteLine($"Created {HASHES-1} hashes.");
        }
    }
}
