using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.mines;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main;
using Types = NettyBaseReloaded.Game.objects.world.map.collectables.Types;

namespace NettyBaseReloaded.Game
{
    class World
    {
        public static int[] BonusBoxMaps = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28};

        public static managers.StorageManager StorageManager = new StorageManager();
        public static managers.DatabaseManager DatabaseManager = new DatabaseManager();
        public static managers.PortalSystemManager PortalSystemManager = new PortalSystemManager();
        public static managers.ServerManager ServerManager = new ServerManager();

        public static void InitiateManagers()
        {
            DateTime timeStarted = DateTime.Now;
            Packet.Handler.AddCommands();
            DatabaseManager.Initiate();
            Task.Factory.StartNew(InitiateWorld);
            ServerManager.Start();
            Out.WriteLog(DateTime.Now - timeStarted + " : World loaded.");
        }

        private static async void InitiateWorld() //todo create ores
        {
            foreach (var map in StorageManager.Spacemaps)
            {
                map.Value.SpawnNpcs();
                map.Value.LoadObjects();
                CreateHashes(map.Value);

                switch (map.Key)
                {
                    case 1: // 1-1
                        map.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(2500, 2500));
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(14000,3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        break;
                    case 2:
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        break;
                    case 3: // 1-3
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        map.Value.CreateLoW(new Vector(2000, 2000));
                        // cbs @10400, 3000
                        //map.Value.CreateAsteroid("Jizz", new Vector(10400, 3000));
                        // todo: make cbs load from db
                        break;
                    case 4:
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(16300, 6600));
                        break;
                    case 5: // 2-1
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        map.Value.CreateStation(Faction.EIC, new Vector(19800, 2000));
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(17800, 4500));
                        break;
                    case 6:
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        break;
                    case 7:
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        map.Value.CreateLoW(new Vector(2000, 2000));
                        //todo: add LOW
                        break;
                    case 8:
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(10000, 10500));
                        break;
                    case 9: // 3-1
                        map.Value.CreateStation(Faction.VRU, new Vector(19800, 11800));
                        map.Value.CreateQuestGiver(Faction.VRU, new Vector(17800, 9800));
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.PROMETIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        break;
                    case 10:
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.ENDURIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        break;
                    case 11:
                        for (int i = 0; i < 20; i++)
                        {
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(14000, 3000), new Vector(18000, 9000)), new Vector[] { new Vector(14000, 3000), new Vector(18000, 9000) });
                            map.Value.CreateOre(OreTypes.TERBIUM, Vector.Random(map.Value, new Vector(4000, 6000), new Vector(7000, 9000)), new Vector[] { new Vector(4000, 6000), new Vector(7000, 9000) });
                        }
                        map.Value.CreateLoW(new Vector(18500, 2000));
                        //todo: add LoW
                        break;
                    case 12:
                        map.Value.CreateQuestGiver(Faction.VRU, new Vector(10800, 3000));
                        break;
                    case 17:
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(17000, 6600));
                        break;
                    case 18: // 1-6
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 20:
                        map.Value.CreateStation(Faction.MMO, new Vector(2000, 6400));
                        map.Value.CreateQuestGiver(Faction.MMO, new Vector(3000, 6400));
                        break;
                    case 21:
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(3000, 10500));
                        break;
                    case 22: // 2-6
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 24:
                        map.Value.CreateStation(Faction.EIC, new Vector(10400, 2000));
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(10400, 3000));
                        break;
                    case 25:
                        map.Value.CreateQuestGiver(Faction.VRU, new Vector(3000, 2000));
                        break;
                    case 26: // 3-6 148/26 ;  152/94   ,    71/80 , 61/36
                        map.Value.CreateCubikon(new Vector(14800, 2600));
                        map.Value.CreateCubikon(new Vector(15200, 9400));
                        map.Value.CreateCubikon(new Vector(7100, 8000));
                        map.Value.CreateCubikon(new Vector(6100, 3600));
                        break;
                    case 28:
                        map.Value.CreateStation(Faction.VRU, new Vector(18800, 6400));
                        map.Value.CreateQuestGiver(Faction.EIC, new Vector(17800, 6400));
                        break;
                    case 29:
                        map.Value.CreatePirateGate(Faction.MMO, new Vector(11500, 13000), 91, new Vector(38000, 7000), false);
                        map.Value.CreatePirateGate(Faction.EIC, new Vector(24400, 20800), 91, new Vector(38600, 12500), false);
                        map.Value.CreatePirateGate(Faction.VRU, new Vector(24400, 6400), 91, new Vector(38000, 18500), false);
                        break;
                    case 42: // ???
                        for (var i = 0; i < 100; i++)
                        {
                            map.Value.CreateNpc(World.StorageManager.Ships[80], AILevels.MOTHERSHIP, true, 10,
                                Vector.Random(map.Value, new Vector(0, 0), new Vector(20800, 12800)));
                        }

                        for (var i = 0; i < 150; i++)
                        {
                            map.Value.CreateNpc(World.StorageManager.Ships[81], AILevels.AGGRESSIVE, true, 10,
                                Vector.Random(map.Value, new Vector(0, 0), new Vector(20800, 12800)));
                        }

                        for (int i = 0; i <= 500; i++)
                        {
                            map.Value.CreateUcbBox(Types.BONUS_BOX, Vector.Random(map.Value), new[] { map.Value.Limits[0], map.Value.Limits[1] });
                        }
                        break;
                    case 91:
                        #region POI
                        map.Value.CreatePOI(new POI("Labyrinth1", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24786, 5040), new Vector(25353, 7360) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth2", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18387, 5440), new Vector(19035, 7360) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth3", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17091, 5600), new Vector(17820, 7520) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth4", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19602, 5840), new Vector(22113, 6560) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth5", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23571, 6080), new Vector(24300, 7920) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth6", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15876, 6320), new Vector(16524, 8320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth7", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26406, 6400), new Vector(27216, 8320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth8", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20817, 6960), new Vector(23328, 7680) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth9", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27621, 7600), new Vector(29565, 8320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth10", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14742, 7680), new Vector(15309, 10720) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth11", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17091, 7680), new Vector(19683, 8320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth12", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24624, 7840), new Vector(26001, 8320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth13", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22437, 8160), new Vector(23085, 9840) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth14", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20898, 8320), new Vector(22032, 8960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth15", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19764, 8640), new Vector(20412, 10240) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth16", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15633, 8720), new Vector(18954, 9440) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth17", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24786, 8800), new Vector(28755, 9440) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth18", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23490, 9280), new Vector(24300, 12560) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth19", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17982, 9760), new Vector(18954, 12160) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth20", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24786, 9840), new Vector(25515, 11280) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth21", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15795, 9920), new Vector(17496, 10480) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth22", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26082, 9920), new Vector(26730, 12960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth23", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27216, 9920), new Vector(29565, 10880) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth24", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19440, 10560), new Vector(22032, 11200) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth25", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(16929, 10880), new Vector(17496, 13200) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth26", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13527, 11040), new Vector(16362, 11680) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth27", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27621, 11520), new Vector(28188, 13040) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth28", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24867, 11760), new Vector(25515, 15200) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth29", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15066, 12160), new Vector(15876, 14080) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth30", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13770, 12800), new Vector(14337, 15040) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth31", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23328, 12960), new Vector(24300, 14400) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth32", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26244, 13440), new Vector(28512, 14000) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth33", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(16362, 13600), new Vector(18792, 14240) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth34", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19116, 14080), new Vector(19764, 16640) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth35", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27783, 14480), new Vector(28512, 16880) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth36", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15147, 14560), new Vector(18630, 15120) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth37", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26082, 14640), new Vector(27216, 16080) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth38", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22275, 14800), new Vector(24300, 15520) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth39", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20331, 15040), new Vector(21951, 15920) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth40", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17172, 15360), new Vector(17820, 17920) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth41", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18225, 15440), new Vector(18792, 16800) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth42", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13203, 15600), new Vector(16281, 16080) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth43", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23328, 15840), new Vector(25272, 16560) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth44", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(21465, 16240), new Vector(22275, 18480) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth45", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14823, 16640), new Vector(15471, 19200) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth46", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15876, 16640), new Vector(16686, 17600) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth47", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25677, 16640), new Vector(27216, 17280) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth48", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22761, 16960), new Vector(23814, 18560) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth49", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24300, 16960), new Vector(25029, 19840) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth50", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19035, 17040), new Vector(21141, 17600) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth51", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28107, 17360), new Vector(28755, 19680) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth52", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25434, 17680), new Vector(27621, 18320) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth53", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(15876, 17920), new Vector(16605, 20480) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth54", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20007, 17920), new Vector(20898, 20800) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth55", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17091, 18320), new Vector(19602, 18960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth56", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25839, 18880), new Vector(26568, 20560) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth57", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(21303, 18960), new Vector(23895, 19600) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth58", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17091, 19440), new Vector(17820, 21120) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth59", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22194, 20080), new Vector(25272, 20560) }, true, false, ""));
                        #endregion
                        map.Value.CreatePirateGate(Faction.MMO, new Vector(4600, 6800), 92, new Vector(18000, 6500), false);
                        map.Value.CreatePirateGate(Faction.EIC, new Vector(2300, 13400), 92, new Vector(19000, 10000), false);
                        map.Value.CreatePirateGate(Faction.VRU, new Vector(4600, 20600), 92, new Vector(19000, 3000), false);
                        map.Value.CreateMineField();
                        map.Value.CreatePalladiumField();
                        break;
                    case 92: // 5-2
                        #region POI + Asset
                        map.Value.CreatePirateStation(new Vector(10400, 6800));
                        map.Value.CreatePOI(new POI("Middle11", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(6400, 7168), new Vector(6912, 7168), new Vector(6912, 7424), new Vector(6400, 7424) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle10", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(5632, 6656), new Vector(6400, 6656), new Vector(6400, 6912), new Vector(5632, 6912) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle13", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(7168, 8448), new Vector(7424, 8448), new Vector(7424, 8960), new Vector(7168, 8960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle12", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(6144, 8192), new Vector(6400, 8192), new Vector(6400, 8960), new Vector(6144, 8960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle15", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(8960, 8960), new Vector(9216, 8960), new Vector(9216, 9472), new Vector(8960, 9472) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle14", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(7680, 9216), new Vector(8192, 9216), new Vector(8192, 9728), new Vector(7680, 9728) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle17", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(5376, 9216), new Vector(5888, 9216), new Vector(5888, 9728), new Vector(5376, 9728) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle16", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(9472, 9472), new Vector(10240, 9472), new Vector(10240, 9728), new Vector(9472, 9728) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle19", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(11776, 9472), new Vector(12288, 9472), new Vector(12288, 9984), new Vector(11776, 9984) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle18", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(10752, 9728), new Vector(11264, 9728), new Vector(11264, 10496), new Vector(10752, 10496) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle20", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(12800, 9728), new Vector(13312, 9728), new Vector(13312, 9984), new Vector(12800, 9984) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle22", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14080, 8448), new Vector(14336, 8448), new Vector(14336, 8960), new Vector(14080, 8960) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation3", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(9984, 7168), new Vector(10752, 7168), new Vector(10752, 7424), new Vector(9984, 7424) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle21", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13056, 8704), new Vector(13568, 8704), new Vector(13568, 9216), new Vector(13056, 9216) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation4", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(10752, 7168), new Vector(11264, 7168), new Vector(11264, 7168), new Vector(10752, 7168) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle24", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14336, 7168), new Vector(14848, 7168), new Vector(14848, 7680), new Vector(14336, 7680) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation5", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(10752, 7168), new Vector(11008, 7168), new Vector(11008, 7168), new Vector(10752, 7168) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle23", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13568, 7680), new Vector(14080, 7680), new Vector(14080, 7936), new Vector(13568, 7936) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation6", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(10752, 7168), new Vector(11008, 7168), new Vector(11008, 7168), new Vector(10752, 7168) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle9", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(6144, 5632), new Vector(6912, 5632), new Vector(6912, 6144), new Vector(6144, 6144) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle26", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14592, 5376), new Vector(15104, 5376), new Vector(15104, 5632), new Vector(14592, 5632) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation7", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9984, 7424), new Vector(10240, 7424), new Vector(10240, 7552), new Vector(9984, 7552) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle25", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14080, 6400), new Vector(14592, 6400), new Vector(14592, 6656), new Vector(14080, 6656) }, true, false, ""));
                        //map.Value.CreatePOI(new POI("mapAssetRangeZone-150000144", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.CIRCLE, new List<Vector> { new Vector(9990,6622),  }, 580, 580, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation8", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(9472, 6144), new Vector(9728, 6144), new Vector(9728, 7324), new Vector(9472, 7324) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle28", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(14080, 4096), new Vector(14336, 4096), new Vector(14336, 4608), new Vector(14080, 4608) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation9", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9216, 6400), new Vector(9472, 6400), new Vector(9472, 7068), new Vector(9216, 7068) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle27", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13824, 4864), new Vector(14336, 4864), new Vector(14336, 5376), new Vector(13824, 5376) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle29", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(13056, 3584), new Vector(13568, 3584), new Vector(13568, 4096), new Vector(13056, 4096) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation13", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(9728, 7083), new Vector(11264, 7083), new Vector(11264, 7168), new Vector(9728, 7168) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation14", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(9728, 7168), new Vector(9984, 7168), new Vector(9984, 7324), new Vector(9728, 7324) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation15", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(9728, 5888), new Vector(10240, 5888), new Vector(10240, 6315), new Vector(9728, 6315) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation16", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9728, 5888), new Vector(9728, 5888), new Vector(9728, 6144), new Vector(9728, 6144) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation17", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9472, 6144), new Vector(9728, 6144), new Vector(9728, 6144), new Vector(9472, 6144) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation18", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(10240, 5888), new Vector(10496, 5888), new Vector(10496, 6315), new Vector(10240, 6315) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation19", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9984, 5888), new Vector(10240, 5888), new Vector(10240, 5888), new Vector(9984, 5888) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle31", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(11264, 3072), new Vector(11776, 3072), new Vector(11776, 3584), new Vector(11264, 3584) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle30", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(12544, 2816), new Vector(12800, 2816), new Vector(12800, 3328), new Vector(12544, 3328) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle33", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(9216, 3328), new Vector(9472, 3328), new Vector(9472, 4096), new Vector(9216, 4096) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation10", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9472, 6144), new Vector(9472, 6144), new Vector(9472, 6400), new Vector(9472, 6400) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle32", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(10240, 2816), new Vector(10496, 2816), new Vector(10496, 3584), new Vector(10240, 3584) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation11", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9216, 6400), new Vector(9216, 6400), new Vector(9216, 6912), new Vector(9216, 6912) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle35", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(7936, 3584), new Vector(8192, 3584), new Vector(8192, 4096), new Vector(7936, 4096) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation12", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.POLYGON, new List<Vector> { new Vector(9216, 6656), new Vector(9216, 6656), new Vector(9216, 6656), new Vector(9216, 6656) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle34", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(8448, 2816), new Vector(8960, 2816), new Vector(8960, 3328), new Vector(8448, 3328) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle37", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(6656, 4352), new Vector(7168, 4352), new Vector(7168, 4864), new Vector(6656, 4864) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle36", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(6912, 3584), new Vector(7424, 3584), new Vector(7424, 3840), new Vector(6912, 3840) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Middle38", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(5888, 4608), new Vector(6400, 4608), new Vector(6400, 4864), new Vector(5888, 4864) }, true, false, ""));
                        //map.Value.CreatePOI(new POI("Equippable Zone", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(10500,6750),  }, 1500, 1500, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation20", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(10496, 5888), new Vector(10752, 5888), new Vector(10752, 6315), new Vector(10496, 6315) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation21", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(10752, 5888), new Vector(11008, 5888), new Vector(11008, 6315), new Vector(10752, 6315) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation22", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(11264, 6144), new Vector(11424, 6144), new Vector(11424, 6315), new Vector(11264, 6315) }, true, false, ""));
                        map.Value.CreatePOI(new POI("SpaceStation23", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.NONE, Shapes.RECTANGLE, new List<Vector> { new Vector(11008, 6144), new Vector(11264, 6144), new Vector(11264, 6315), new Vector(11008, 6315) }, true, false, ""));
                        #endregion
                        map.Value.CreatePirateGate(Faction.EIC, new Vector(2100, 10800), 93, new Vector(39000, 10000), false);
                        map.Value.CreatePirateGate(Faction.VRU, new Vector(600, 6600), 93, new Vector(38000, 6500), false);
                        map.Value.CreatePirateGate(Faction.MMO, new Vector(2100, 3400), 93, new Vector(39000, 3500), false);

                        break;
                    case 93: //5-3
                        #region POI
                        map.Value.CreatePOI(new POI("Labyrinth1", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(16284, 177), new Vector(16815, 2537) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth2", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17169, 177), new Vector(19116, 649) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth3", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19883, 177), new Vector(20414, 2301) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth4", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20886, 177), new Vector(23128, 708) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth5", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23659, 177), new Vector(24190, 1711) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth6", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27907, 177), new Vector(28497, 1652) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth7", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(29146, 236), new Vector(31329, 590) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth8", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24485, 472), new Vector(27494, 885) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth9", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(30031, 944), new Vector(30621, 3481) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth10", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28969, 1121), new Vector(29441, 3835) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth11", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17287, 1180), new Vector(19529, 1593) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth12", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25252, 1180), new Vector(25901, 3835) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth13", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26137, 1947), new Vector(28556, 2537) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth14", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17169, 2065), new Vector(17818, 4543) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth15", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18113, 2065), new Vector(19470, 2773) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth16", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20001, 2655), new Vector(20591, 3953) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth17", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19057, 3127), new Vector(19529, 4248) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth18", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27730, 3127), new Vector(28202, 4366) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth19", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18231, 3245), new Vector(18644, 5959) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth20", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26491, 3422), new Vector(27140, 6254) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth21", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(31093, 3953), new Vector(31565, 6903) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth22", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24544, 4130), new Vector(26196, 4661) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth23", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28556, 4130), new Vector(30562, 4602) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth24", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18939, 4484), new Vector(21240, 4897) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth25", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17287, 5015), new Vector(17818, 7257) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth26", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27376, 5015), new Vector(30798, 5369) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth27", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25370, 5074), new Vector(26137, 5900) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth28", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19057, 5192), new Vector(19470, 6313) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth29", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19942, 5192), new Vector(22302, 5841) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth30", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23010, 5251), new Vector(24957, 5723) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth31", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(1652, 5664), new Vector(2655, 6726) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth32", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(30385, 5723), new Vector(30857, 7080) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth33", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28025, 5841), new Vector(30031, 6431) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth34", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(295, 5900), new Vector(1180, 7139) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth35", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3245, 6195), new Vector(4130, 7493) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth36", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20886, 6254), new Vector(23482, 6785) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth37", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24013, 6254), new Vector(26137, 6785) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth38", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18290, 6549), new Vector(20296, 7021) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth39", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27081, 6549), new Vector(27553, 9263) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth40", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20591, 7257), new Vector(21004, 9853) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth41", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27848, 7375), new Vector(31565, 8024) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth42", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17287, 7611), new Vector(20178, 8260) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth43", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3894, 7906), new Vector(5015, 9263) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth44", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(30090, 8319), new Vector(30562, 10856) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth45", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18231, 8555), new Vector(18762, 11269) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth46", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28320, 8555), new Vector(28851, 10207) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth47", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17169, 8614), new Vector(17818, 9971) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth48", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18998, 8614), new Vector(20296, 9204) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth49", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(30916, 9263), new Vector(31506, 12154) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth50", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19824, 9499), new Vector(20237, 11505) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth51", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3186, 9676), new Vector(3953, 10738) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth52", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24367, 9794), new Vector(26609, 10325) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth53", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20709, 10148), new Vector(21122, 11564) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth54", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17523, 10384), new Vector(17995, 12390) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth55", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27199, 10443), new Vector(29736, 11210) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth56", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19116, 10620), new Vector(19529, 13393) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth57", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26078, 10679), new Vector(26609, 13098) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth58", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3953, 10974), new Vector(4661, 12036) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth59", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23718, 10974), new Vector(25724, 11682) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth60", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28733, 11505), new Vector(30562, 12213) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth61", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18349, 11800), new Vector(18821, 13511) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth62", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(27435, 11800), new Vector(28143, 14986) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth63", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19883, 11859), new Vector(22125, 12390) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth64", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22597, 11859), new Vector(23010, 13688) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth65", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24839, 12154), new Vector(25606, 14455) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth66", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3481, 12272), new Vector(4602, 13216) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth67", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28674, 12626), new Vector(32214, 13216) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth68", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(19942, 12685), new Vector(20296, 14809) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth69", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(21476, 12980), new Vector(21889, 15753) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth70", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26019, 13452), new Vector(27081, 13924) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth71", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(4071, 13629), new Vector(4897, 14750) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth72", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(17641, 13747), new Vector(19588, 14278) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth73", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(28497, 13865), new Vector(41064, 15753) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth74", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22066, 14042), new Vector(24426, 14632) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth75", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(26668, 14278), new Vector(27140, 16166) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth76", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(23718, 14986), new Vector(26314, 15576) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth77", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(22479, 15045), new Vector(23423, 15635) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth78", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3186, 15104), new Vector(4071, 16166) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth79", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(18880, 15340), new Vector(21063, 15753) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth80", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(24839, 15930), new Vector(25429, 17405) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth81", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(20650, 16048), new Vector(24367, 16756) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth82", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3894, 16461), new Vector(4425, 17700) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth83", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(25724, 16461), new Vector(29264, 17169) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth84", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(3658, 18113), new Vector(4838, 19293) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth85", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(59, 18939), new Vector(826, 20001) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth86", Game.objects.world.map.pois.Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector> { new Vector(1180, 19234), new Vector(2242, 20119) }, true, false, ""));
                        map.Value.CreatePOI(new POI("Labyrinth87", Game.objects.world.map.pois.Types.NO_ACCESS,
                            Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE,
                            new List<Vector> {new Vector(2537, 19588), new Vector(3599, 20709)}, true, false, ""));
                        #endregion
                        map.Value.CreatePirateGate(Faction.MMO, new Vector(1300, 9600), 16, Vector.Random(World.StorageManager.Spacemaps[16]), false, true);
                        map.Value.CreatePirateGate(Faction.EIC, new Vector(1300, 13600), 16, Vector.Random(World.StorageManager.Spacemaps[16]), false, true);
                        map.Value.CreatePirateGate(Faction.VRU, new Vector(1300, 17600), 16, Vector.Random(World.StorageManager.Spacemaps[16]), false, true);

                        map.Value.CreateMineField();
                        map.Value.CreatePalladiumField();
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
                if (BonusBoxMaps.Contains(map.Key))
                {
                    var bbCount = BonusBox.SPAWN_COUNT;
                    if (map.Value.Pvp) bbCount = BonusBox.PVP_SPAWN_COUNT;
                    for (int i = 0; i <= bbCount; i++)
                    {
                        map.Value.CreateBox(Types.BONUS_BOX, Vector.Random(map.Value), new [] { map.Value.Limits[0], map.Value.Limits[1]});
                    }
                    for (int i = 0; i <= PirateBooty.SPAWN_COUNT; i++)
                    {
                        map.Value.CreatePirateBooty(Types.PIRATE_BOOTY_BOX, Vector.Random(map.Value), new[] { map.Value.Limits[0], map.Value.Limits[1] });
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
            var stringChars = new char[5];
            const int HASHES = 800;

            int created = 0;
            var randomInstance = RandomInstance.getInstance(map);
            for (int entry = 0; entry < HASHES; entry++)
            {
                NEWHASH:
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[randomInstance.Next(chars.Length)];
                }

                var hash = new string(stringChars);
                if (map.HashedObjects.ContainsKey(hash) || StorageManager.HoneyBoxes.Contains(hash))
                    goto NEWHASH;
                map.HashedObjects[hash] = null;
                created++;
            }

            foreach (var honeyBox in StorageManager.HoneyBoxes)
            {
                map.HashedObjects[honeyBox] = new FakeHoneyBox(honeyBox);
                created++;
            }

            map.Objects.TryAdd(0, null);
            Debug.WriteLine($"Created {created} hashes.");
        }
    }
}
