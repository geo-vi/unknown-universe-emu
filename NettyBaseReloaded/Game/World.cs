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
            DateTime timeStarted = DateTime.Now;
            Packet.Handler.AddCommands();
            DatabaseManager.Initiate();
            Task.Factory.StartNew(InitiateWorld);
            Out.WriteLine(DateTime.Now - timeStarted + " : World loaded.");
        }

        private static void InitiateWorld()
        {
            foreach (var map in StorageManager.Spacemaps)
            {
                map.Value.LoadObjects();

                switch (map.Key)
                {
                    case 1:
                        map.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(2500, 2500));
                        break;
                    case 5:
                        map.Value.CreateStation(Faction.EIC, new Vector(19800, 2000));
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(17800, 4500));
                        break;
                    case 9:
                        map.Value.CreateStation(Faction.VRU, new Vector(19800, 11800));
                        map.Value.CreateQuestGiver(Faction.VRU, new Vector(17800, 9800));
                        break;
                    default:
                        if (map.Key > 12)
                            map.Value.DisablePortals();
                        else map.Value.DisablePortals(new List<int>{13,14,15});
                        break;
                }
                map.Value.CreateAdvertisementBanner(0, new Vector(15800, 13500));
                map.Value.SpawnNpcs();

                if (map.Key == 42)
                {
                    map.Value.CreatePortal(16, 10400, 6400, 0, 0);
                    //map.Value.CreateNpc(StorageManager.Ships[80], AILevels.MOTHERSHIP, true, 60, new Vector(1500, 2500));
                    //for (int i = 0; i <= BonusBox.PVP_SPAWN_COUNT; i++)
                    //{
                    //    map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value, map.Value.Limits[0].X, map.Value.Limits[0].Y, map.Value.Limits[1].X, map.Value.Limits[1].Y), new[] { map.Value.Limits[0].X, map.Value.Limits[0].Y, map.Value.Limits[1].X, map.Value.Limits[1].Y });
                    //}
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
                else if (map.Key != 200 && map.Key != 54 && map.Key != 53 && map.Key != 52 && map.Key != 51 && map.Key != 16)
                {
                    for (int i = 0; i <= BonusBox.SPAWN_COUNT; i++)
                    {
                        //map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value), new[] {map.Value.Limits[0], map.Value.Limits[1]});
                    }
                }
            }
            Log.Write("Loaded World");
        }
    }
}
