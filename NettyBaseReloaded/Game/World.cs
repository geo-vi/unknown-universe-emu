using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.packet;
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
                map.Value.SpawnNpcs();
                if (map.Key == 10)
                {
                    map.Value.CreateNpc(StorageManager.Ships[84]);
                }
                //if (map.Key == 12)
                //{
                //    for (int i = 0; i < 150; i++)
                //    {
                //        map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(1000, 19800, 1000, 11800));
                //    }
                //    map.Value.CreateNpc(StorageManager.Ships[80], AILevels.MOTHERSHIP, 30);
                //}
                if (map.Key == 1)
                {
                    map.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                }
                if (map.Key == 6)
                {
                    map.Value.CreateStation(Faction.EIC, new Vector(19200, 1000));
                }
                if (map.Key == 9)
                {
                    map.Value.CreateStation(Faction.VRU, new Vector(19200, 11800));
                    //map.Value.CreatePirateStation(new Vector(2000, 5650));
                    //map.Value.CreateAsteroid("Metroid", new Vector(5000, 2500));
                    //map.Value.CreatePOI(new POI("Poi1", objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(7680, 9216), new Vector(8192, 9216), new Vector(8192, 9728), new Vector(7680, 9728) }));
                }
                if (map.Key == -1)
                {
                    // TODO Add PVP Spawn
                }
                else
                {
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(1000, 19800, 1000, 11800));
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
            Console.WriteLine($"Created {HASHES-1} hashes.");
        }
    }
}
