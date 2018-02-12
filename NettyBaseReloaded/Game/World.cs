using System;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.collectables;
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
                map.Value.SpawnNpcs();
                if (map.Key == 1)
                {
                    map.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                }
                if (map.Key == 5)
                {
                    map.Value.CreateStation(Faction.EIC, new Vector(19200, 1000));
                }
                if (map.Key == 9)
                {
                    map.Value.CreateStation(Faction.VRU, new Vector(19200, 11800));
                    //map.Value.CreatePirateStation(new Vector(2000, 5650));
                    //map.Value.CreateAsteroid("Metroid", new Vector(5000, 2500));
                    //map.Value.CreatePOI(new POI("Poi1", objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(7680, 9216), new Vector(8192, 9216), new Vector(8192, 9728), new Vector(7680, 9728) }));
                    //map.Value.CreateLoW(new Vector(4000,4000));
                }
                if (map.Key == 16)
                {
                    map.Value.CreateLoW(new Vector(20800, 12800));
                }
                if (map.Key == 20)
                {
                    map.Value.CreateStation(Faction.MMO, new Vector(1000, 12800 / 2));
                }
                if (map.Key == 24)
                {
                    map.Value.CreateStation(Faction.EIC, new Vector(20800 / 2, 1000));
                }
                if (map.Key == 28)
                {
                    map.Value.CreateStation(Faction.VRU, new Vector(20800 - 1000, 12800 / 2));
                }
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
                else if (map.Key != 200 && map.Key != 54 && map.Key != 53 && map.Key != 52 && map.Key != 51)
                {
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value, 1000, 19800, 1000, 11800));
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
            if (Properties.Server.DEBUG)
                Console.WriteLine($"Created {HASHES-1} hashes.");
        }
    }
}
