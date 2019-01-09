using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Main;
using Types = NettyBaseReloaded.Game.objects.world.map.collectables.Types;

namespace NettyBaseReloaded.Game
{
    class World
    {    
        public static managers.StorageManager StorageManager = new StorageManager();
        public static managers.DatabaseManager DatabaseManager = new DatabaseManager();
        public static managers.PortalSystemManager PortalSystemManager = new PortalSystemManager();

        public static void InitiateManagers()
        {
            DateTime timeStarted = DateTime.Now;
            Packet.Handler.AddCommands();
            DatabaseManager.Initiate();
            Task.Factory.StartNew(InitiateWorld);
            Out.WriteLog(DateTime.Now - timeStarted + " : World loaded.");
        }

        private static async void InitiateWorld() //todo create ores
        {
            foreach (var map in StorageManager.Spacemaps)
            {
                map.Value.LoadObjects();
                CreateHashes(map.Value);

                switch (map.Key)
                {
                    case 1: // 1-1
                        map.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(2500, 2500));
                        map.Value.CreateOre(OreTypes.PROMETIUM, new Vector(500,500), new Vector[] {new Vector(0,0), new Vector(1000,1000), });
                        break;
                    case 3: // 1-3
                        map.Value.CreateLoW(new Vector(2000, 2000));
                        // cbs @10400, 3000
                        //map.Value.CreateAsteroid("Jizz", new Vector(10400, 3000));
                        // todo: make cbs load from db
                        break;
                    case 5: // 2-1
                        map.Value.CreateStation(Faction.EIC, new Vector(19800, 2000));
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(17800, 4500));
                        break;
                    case 9: // 3-1
                        map.Value.CreateStation(Faction.VRU, new Vector(19800, 11800));
                        map.Value.CreateQuestGiver(Faction.VRU, new Vector(17800, 9800));
                        break;
                    case 18: // 1-6
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 20:
                        map.Value.CreateStation(Faction.MMO, new Vector(2000, 6400));
                        break;
                    case 22: // 2-6
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 24:
                        map.Value.CreateStation(Faction.EIC, new Vector(14000, 2000));
                        break;
                    case 26: // 3-6 148/26 ;  152/94   ,    71/80 , 61/36
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 28:
                        map.Value.CreateStation(Faction.VRU, new Vector(18800, 6400));
                        break;
                    case 42: // ???
                        map.Value.CreatePortal(16, 10400, 6400, 0, 0);
                        break;
                    case 200: // low
                        map.Value.CreateHealthStation(new Vector(10400, 6400));
                        map.Value.CreateRelayStation(new Vector(2500, 2000));
                        map.Value.CreateRelayStation(new Vector(6200, 11700));
                        map.Value.CreateRelayStation(new Vector(18300, 10900));
                        map.Value.CreateRelayStation(new Vector(18200, 4000));
                        break;
                }
                //map.Value.CreateAdvertisementBanner(0, new Vector(15800, 13500));
                map.Value.SpawnNpcs();
                
                if (map.Key != 200 && map.Key != 54 && map.Key != 53 && map.Key != 52 && map.Key != 51 && map.Key != 16)
                {
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value), new [] { map.Value.Limits[0], map.Value.Limits[1]});
                    }
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        map.Value.CreateBox(Types.WINTER_GIFT_BOX, Vector.Random(map.Value), new[] { map.Value.Limits[0], map.Value.Limits[1] });
                    }
                }
            }
            Out.WriteLog("Loaded World");
            StreamMessageToWorld("World fully loaded, have fun playing.");
            DatabaseManager.LoadEvents();
            while (true)
            {
                Global.QueryManager.UpdateOnlinePlayers();
                await Task.Delay(5000);
            }
        }

        public static void StreamMessageToWorld(string msg)
        {
            foreach (var gameSession in StorageManager.GameSessions.Values)
            {
               if (gameSession != null)
                   Packet.Builder.LegacyModule(gameSession, "0|A|STD|" + msg);
            }
        }

        private static void CreateHashes(Spacemap map)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[4];
            const int HASHES = 1000;

            var randomInstance = RandomInstance.getInstance(map);
            for (int entry = 0; entry < HASHES; entry++)
            {
                NEWHASH:
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[randomInstance.Next(chars.Length)];
                }

                var hash = new String(stringChars);
                if (map.HashedObjects.ContainsKey(hash))
                    goto NEWHASH;
                map.HashedObjects.TryAdd(hash, null);
            }
            map.Objects.TryAdd(0, null);
            Debug.WriteLine($"Created {HASHES-1} hashes.");
        }
    }
}
