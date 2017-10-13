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
                //map.Value.SpawnNpcs();
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
                map.HashedObjects.Add(hash, null);
            }
            Console.WriteLine($"Created {HASHES-1} hashes.");
        }
    }
}
