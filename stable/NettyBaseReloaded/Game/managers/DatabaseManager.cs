using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.managers
{
    class DatabaseManager
    {
        public void BasicLoads()
        {
            LoadServerInfo();
            LoadShips();
            LoadSpacemaps();
            LoadLevels();
        }

        public void SaveAll()
        {
            
        }

        public void LoadServerInfo()
        {

        }

        public void LoadShips()
        {
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM server_ships");

                MySqlDataReader reader = new MySQLManager().Execute(query);



                int count = 0;
                while (reader.Read())
                {
                    //Informations
                    int ship_id = Convert.ToInt32(reader["ship_id"]);
                    string ship_name = Convert.ToString(reader["name"]);
                    string ship_lootId = Convert.ToString(reader["ship_lootid"]);
                    int ship_hp = Convert.ToInt32(reader["ship_hp"]);
                    int ship_nano = Convert.ToInt32(reader["nanohull"]);
                    int ship_speed = Convert.ToInt32(reader["base_speed"]);
                    int ship_shield = Convert.ToInt32(reader["shield"]);
                    int ship_shieldAbsorb = Convert.ToInt32(reader["shieldAbsorb"]);
                    int ship_minDamage = Convert.ToInt32(reader["minDamage"]);
                    int ship_maxDamage = Convert.ToInt32(reader["maxDamage"]);
                    bool ship_neutral = Convert.ToBoolean(Convert.ToInt32(reader["isNeutral"]));
                    int ship_laserColor = Convert.ToInt32(reader["laserID"]);
                    int ship_batteries = Convert.ToInt32(reader["batteries"]);
                    int ship_rockets = Convert.ToInt32(reader["rockets"]);
                    int ship_cargo = Convert.ToInt32(reader["cargo"]);
                    int ship_ai = Convert.ToInt32(reader["isNeutral"]);

                    //Rewards
                    int ship_exp = Convert.ToInt32(reader["experience"]);
                    int ship_honor = Convert.ToInt32(reader["honor"]);
                    int ship_cre = Convert.ToInt32(reader["credits"]);
                    int ship_uri = Convert.ToInt32(reader["uridium"]);

                    var rewards = new Dictionary<RewardType, int>();
                    rewards.Add(RewardType.EXPERIENCE, ship_exp);
                    rewards.Add(RewardType.HONOR, ship_honor);
                    rewards.Add(RewardType.CREDITS, ship_cre);
                    rewards.Add(RewardType.URIDIUM, ship_uri);
                    var ship_rewards = new Reward(rewards);
                    
                    ////@ToDo: Dropables
                    var ship_drops = JsonConvert.DeserializeObject<DropableRewards>(reader["dropJSON"].ToString());

                    ////add to Storage
                    World.StorageManager.Ships.Add(ship_id, new Ship(
                        ship_id,
                        ship_name,
                        ship_lootId,
                        ship_hp,
                        ship_nano,
                        ship_speed,
                        ship_shield,
                        ship_shieldAbsorb,
                        ship_minDamage,
                        ship_maxDamage,
                        ship_neutral,
                        ship_laserColor,
                        ship_batteries,
                        ship_rockets,
                        ship_cargo,
                        ship_rewards,
                        ship_drops,
                        ship_ai
                        ));
                    count++;
                }

                reader.Close();



                if (Properties.Server.DEBUG) Out.WriteLine("Loaded successfully " + count + " ships from DB.", "GAME", ConsoleColor.Green);
            }
            catch (Exception)
            {
                Out.WriteLine("Failed to load ships...","DB",ConsoleColor.Red);
            }
        }

        public void LoadSpacemaps()
        {
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM server_maps");

                MySqlDataReader reader = new MySQLManager().Execute(query);



                int count = 0;

                World.StorageManager.Spacemaps.Add(0,new Spacemap(0, "0-1", Faction.NONE, false, true, 0, null, null));
                while (reader.Read())
                {
                    int map_id = Convert.ToInt32(reader["ID"]);

                    string map_name = Convert.ToString(reader["NAME"]);
                    var map_faction = (Faction)Convert.ToInt32(reader["FACTION_ID"]);

                    bool map_pvp = Convert.ToBoolean(0);
                    bool map_starter = Convert.ToBoolean(Convert.ToInt32(reader["IS_STARTER_MAP"]));

                    var map_level = Convert.ToInt32(reader["LEVEL"]);

                    var npcs = JsonConvert.DeserializeObject<List<BaseNpc>>(reader["NPCS"].ToString());

                    var portals = JsonConvert.DeserializeObject<List<PortalBase>>(reader["PORTALS"].ToString());

                    World.StorageManager.Spacemaps.Add(map_id, new Spacemap(map_id, map_name, map_faction, map_pvp, map_starter, map_level, npcs, portals));
                    count++;
                }

                reader.Close();

                if (Properties.Game.PVP_MODE)
                {
                    var mainMap = World.StorageManager.Spacemaps[16];
                    mainMap.CreateStation(Faction.MMO, new Vector(2000,12800));
                    mainMap.CreateStation(Faction.EIC, new Vector(39600,2000));
                    mainMap.CreateStation(Faction.VRU, new Vector(39600,24600));
                    mainMap.CreatePirateStation(new Vector(21570,12825));

//                    var mapZone = new NpcZone(mainMap.GetNextZoneId(), new Vector(0, 25600), new Vector(41600, 0));
                    //mainMap.CreateNpcs(new BaseNpc(114, 8));
                    //mainMap.CreateNpcs(new BaseNpc(111, 36));


                    mainMap.CreatePOI(new POI("Middle14", Types.NO_ACCESS, Designs.ASTEROIDS_MIXED_WITH_SCRAP, Shapes.RECTANGLE, new List<Vector>{new Vector(7680,9216),new Vector(8192,9216),new Vector(8192,9728),new Vector(7680,9728)}));
//                    mainMap.CreatePOI(new POI("Middle10", Types.NO_ACCESS, Designs.SCRAP, Shapes.RECTANGLE, new List<Vector>{new Vector(19360, 12520), new Vector(19890, 12520), new Vector(19360, 12600), new Vector(19890, 12600)}));
                    //mainMap.CreatePOI(new POI("Middle13", Types.NO_ACCESS, Designs.ASTEROIDS, Shapes.RECTANGLE, new List<Vector>{new Vector(19630, 12390), new Vector(19710, 12390), new Vector(19630, 12510), new Vector(19710, 12510)}));
//                    mainMap.CreatePOI(new POI("Middle12", Types.NO_ACCESS, Designs.NEBULA, Shapes.RECTANGLE, new List<Vector>{new Vector(19500, 12610), new Vector(19580, 12610), new Vector(19500, 13180), new Vector(19580, 13180)}));
//                    mainMap.CreatePOI(new POI("Middle15", Types.NO_ACCESS, Designs.SCRAP, Shapes.RECTANGLE, new List<Vector>{new Vector(18960, 12750), new Vector(19490, 12750), new Vector(18960, 12830), new Vector(19490, 12830)}));
//                    mainMap.CreatePOI(new POI("Middle14", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(18820, 13100), new Vector(19490, 13100), new Vector(18820, 13180), new Vector(19490, 13180)}));
                }
                else
                {
                    foreach (var spacemap in World.StorageManager.Spacemaps)
                    {
                        if (spacemap.Key == 1)
                        {
                            spacemap.Value.CreateStation(Faction.MMO, new Vector(1000, 1000));
                            //spacemap.Value.Assets.Add(-1, new QuestGiver(-1, new Vector(2000, 2000), Faction.MMO));
                        }
                        if (spacemap.Key == 5)
                        {
                            spacemap.Value.CreateStation(Faction.EIC, new Vector(20800 - 1000, 1000));
                            //spacemap.Value.Assets.Add(-2, new QuestGiver(-2, new Vector(28000 - 2000, 2000), Faction.MMO));
                        }
                        if (spacemap.Key == 9)
                        {
                            spacemap.Value.CreateStation(Faction.VRU, new Vector(20800 - 1000, 12800 - 1000));
                            //spacemap.Value.Assets.Add(-3, new QuestGiver(-3, new Vector(20800 - 2000, 12800 - 2000), Faction.MMO));
                        }
                        if (spacemap.Key == 20)
                        {
                            spacemap.Value.CreateStation(Faction.MMO, new Vector(1000, 12800 / 2));
                        }
                        if (spacemap.Key == 24)
                        {
                            spacemap.Value.CreateStation(Faction.EIC, new Vector(20800 - 1000, 12800 / 2));
                        }
                        if (spacemap.Key == 28)
                        {
                            spacemap.Value.CreateStation(Faction.VRU, new Vector(20800 - 1000, 12800 / 2));
                        }

                        spacemap.Value.LoadObjects();
                        spacemap.Value.SpawnNpcs();
                    }
                }

                if (Properties.Server.DEBUG) Out.WriteLine("Loaded successfully " + count + " spacemaps from DB.", "GAME", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Out.WriteLine("Failed to load spacemaps...", "DB", ConsoleColor.Red);
                Debug.WriteLine(e);
            }
        }

        public void LoadLevels()
        {
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM server_levels_player");

                MySqlDataReader reader = new MySQLManager().Execute(query);



                int count = 0;
                while (reader.Read())
                {
                    var Id = Convert.ToInt32(reader["ID"]);
                    var Exp = Convert.ToInt64(reader["EXP"]);
                    World.StorageManager.Levels.PlayerLevels.Add(Id, new Level(Id,Exp));
                    count++;
                }

                reader.Close();

                query = new MySqlCommand("SELECT * FROM server_levels_drone");

                reader = new MySQLManager().Execute(query);

                while (reader.Read())
                {
                    var Id = Convert.ToInt32(reader["ID"]);
                    var Exp = Convert.ToInt64(reader["EXP"]);
                    World.StorageManager.Levels.DroneLevels.Add(Id, new Level(Id, Exp));
                    count++;
                }

                reader.Close();



                if (Properties.Server.DEBUG) Out.WriteLine("Loaded successfully " + count + " levels from DB.", "GAME", ConsoleColor.Green);

            }
            catch (Exception)
            {
                Out.WriteLine("Failed to load levels...", "DB", ConsoleColor.Red);
            }
        }

        public Hangar LoadHangar(Player player)
        {
            Hangar hangar = null;
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id + " AND ACTIVE=1");
                MySqlDataReader reader = new MySQLManager().Execute(query);



                while (reader.Read())
                {
                    var ship = World.StorageManager.Ships[Convert.ToInt32(reader["SHIP_DESIGN"])];
                    var pos = new Vector(Convert.ToInt32(reader["SHIP_X"]), Convert.ToInt32(reader["SHIP_Y"]));
                    var mapId = Convert.ToInt32(reader["SHIP_MAP_ID"]);
                    var hp = Convert.ToInt32(reader["SHIP_HP"]);
                    var nano = Convert.ToInt32(reader["SHIP_NANO"]);
                    hangar = new Hangar(ship, null, pos, World.StorageManager.Spacemaps[mapId], hp, nano, new Dictionary<string, Item>());
                }
                reader.Close();

                Logger.Logger.WritingManager.Write("[MYSQL] Loaded hangar for User. [ID:" + player.Id + "]");
            }
            catch (Exception)
            {

            }
            return hangar;
        }

        public Configuration[] LoadConfig(Player player)
        {
            Configuration[] builder = null;
            try
            {

                MySqlCommand query = new MySqlCommand("SELECT * FROM player_ship_config WHERE PLAYER_ID=" + player.Id);

                MySqlDataReader reader = new MySQLManager().Execute(query);



                while (reader.Read())
                {
                    int dmg1 = Convert.ToInt32(reader["CONFIG_1_DMG"]);
                    int velocity1 = Convert.ToInt32(reader["CONFIG_1_SPEED"]);
                    string extras1 = reader["CONFIG_1_EXTRAS"].ToString();
                    int shield1 = Convert.ToInt32(reader["CONFIG_1_SHIELD"]);
                    double absorb1 = Convert.ToDouble(reader["CONFIG_1_SHIELDABSORB"]) / 10000;
                    int lcount1 = Convert.ToInt32(reader["CONFIG_1_LASERCOUNT"]);

                    if (extras1 == "") extras1 = "[]";
                    var config1 = new Configuration(1, dmg1, velocity1, shield1, shield1, absorb1, lcount1, JsonConvert.DeserializeObject<List<Item>>(extras1).ToDictionary(x => x.LootId));

                    int dmg2 = Convert.ToInt32(reader["CONFIG_2_DMG"]);
                    int velocity2 = Convert.ToInt32(reader["CONFIG_2_SPEED"]);
                    string extras2 = reader["CONFIG_2_EXTRAS"].ToString();
                    int shield2 = Convert.ToInt32(reader["CONFIG_2_SHIELD"]);
                    double absorb2 = Convert.ToDouble(reader["CONFIG_2_SHIELDABSORB"]) / 10000;
                    int lcount2 = Convert.ToInt32(reader["CONFIG_2_LASERCOUNT"]);

                    if (extras2 == "") extras2 = "[]";
                    var config2 = new Configuration(2, dmg2, velocity2, shield2, shield2, absorb2, lcount2, JsonConvert.DeserializeObject<List<Item>>(extras2).ToDictionary(x => x.LootId));

                    builder = new Configuration[2]
                    {
                        config1,
                        config2
                    };
                }
                reader.Close();

                Logger.Logger.WritingManager.Write("[MYSQL] Loaded configurations for User. [ID:" + player.Id + "]");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in LoadConfig()");
                Debug.WriteLine(e.StackTrace);
            }
            return builder;
        }

        public List<Drone> LoadDrones(Player player)
        {
            List<Drone> drones = new List<Drone>();
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM player_drones WHERE PLAYER_ID=" + player.Id);
                MySqlDataReader reader = new MySQLManager().Execute(query);




                while (reader.Read())
                {
                    int droneId = Convert.ToInt32(reader["ID"]);
                    int type = Convert.ToInt32(reader["DRONE_TYPE"]);
                    int level = Convert.ToInt32(reader["LEVEL"]);
                    int exp = Convert.ToInt32(reader["EXPERIENCE"]);

                    drones.Add(new Drone(droneId, player.Id, (DroneType)(type + 1), World.StorageManager.Levels.DroneLevels[level], exp, 0));
                }
                reader.Close();

                Logger.Logger.WritingManager.Write("[MYSQL] Loaded drones for User. [ID:" + player.Id + "]");
            }
            catch (Exception)
            {
                Debug.WriteLine("Error in LoadDrones()");
            }
            return drones;
        }

        public Ammo LoadAmmo(Player player)
        {
            Ammo ammo = null;
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM player_ammo WHERE PLAYER_ID=" + player.Id);

                MySqlDataReader reader = new MySQLManager().Execute(query);




                while (reader.Read())
                {
                    var lcb = Convert.ToInt32(reader["LCB_10"]);
                    var mcb25 = Convert.ToInt32(reader["MCB_25"]);
                    var mcb50 = Convert.ToInt32(reader["MCB_50"]);
                    var sab50 = Convert.ToInt32(reader["SAB_50"]);
                    var ucb100 = Convert.ToInt32(reader["UCB_100"]);
                    var cbo100 = Convert.ToInt32(reader["CBO_100"]);
                    var rsb75 = Convert.ToInt32(reader["RSB_75"]);
                    var job100 = Convert.ToInt32(reader["JOB_100"]);
                    var r310 = Convert.ToInt32(reader["R_310"]);
                    var plt2021 = Convert.ToInt32(reader["PLT_2021"]);
                    var plt2026 = Convert.ToInt32(reader["PLT_2026"]);
                    var plt3030 = Convert.ToInt32(reader["PLT_3030"]);
                    var eco10 = Convert.ToInt32(reader["ECO_10"]);
                    var emp = Convert.ToInt32(reader["EMP_01"]);
                    //ammo = new Ammo(lcb, mcb25, mcb50, sab50, ucb100, cbo100, rsb75, job100, r310, plt2021, plt2026, plt3030, eco10, emp);
                }
                reader.Close();

            }
            catch (Exception)
            {
                Debug.WriteLine("Error in LoadAmmo()");
            }
            return ammo;
        }

        public Player GetAccount(int playerId)
        {
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM player_data, player_extra_data, player_hangar WHERE player_hangar.PLAYER_ID = player_data.PLAYER_ID AND player_extra_data.PLAYER_ID = player_data.PLAYER_ID AND player_hangar.ACTIVE=1 AND player_data.PLAYER_ID = " + playerId);

                MySqlDataReader reader = new MySQLManager().Execute(query);




                while (reader.Read())
                {
                    var shipId = Convert.ToInt32(reader["SHIP_DESIGN"]);
                    var mapId = Convert.ToInt32(reader["SHIP_MAP_ID"]);
                    var levelId = Convert.ToInt32(reader["LVL"]);


                    if (!World.StorageManager.Spacemaps.ContainsKey(mapId))
                    {
                        Console.WriteLine("PROBLEM -> MAPID " + mapId + " DOESN'T EXIST!");
                        reader.Close();
                        return null;
                    }

                    if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(levelId))
                    {
                        Console.WriteLine("PROBLEM -> LEVELID " + levelId + " DOESN'T EXIST!");
                        reader.Close();
                        return null;
                    }

                    return new Player(
                        playerId,
                        reader["PLAYER_NAME"].ToString(),
                        "",
                        new Hangar(
                            World.StorageManager.Ships[shipId],
                            new List<Drone>(),
                            new Vector(
                                Convert.ToInt32(reader["SHIP_X"]),
                                Convert.ToInt32(reader["SHIP_Y"])),
                            World.StorageManager.Spacemaps[mapId],
                            Convert.ToInt32(reader["SHIP_HP"]),
                            Convert.ToInt32(reader["SHIP_NANO"]),
                            new Dictionary<string, Item>()),
                        //null,
                        Global.StorageManager.Clans[0],
                        (Faction)Convert.ToInt32(reader["FACTION_ID"]),
                        new Reward(0,0),
                        //new BasicRewards(0, 0, 0, 0, 0),
                        new DropableRewards(0, 0, 0, 0, 0, 0, 0, 0),
                        (Rank)Convert.ToInt32(reader["RANK"]),
                        World.StorageManager.Levels.PlayerLevels[levelId],
                        Convert.ToInt64(reader["EXP"]),
                        Convert.ToInt64(reader["HONOR"]),
                        Convert.ToDouble(reader["CREDITS"]),
                        Convert.ToDouble(reader["URIDIUM"]),
                        Convert.ToSingle(reader["JACKPOT"]),
                        Convert.ToInt32(reader["GG_RINGS"]),
                        Convert.ToBoolean(Convert.ToInt32(reader["PREMIUM"])),
                        new RocketLauncher(playerId, new List<int> { 2 }, 5, 0),
                        JsonConvert.DeserializeObject<Cargo>(reader["CARGO"].ToString()),
                        JsonConvert.DeserializeObject<StatsStorage>(reader["STATS"].ToString())
                        );
                }
                reader.Close();

            }
            catch (Exception e)
            {
                Out.WriteLine("Something went wrong loading player accounts", "ERROR", ConsoleColor.Red);

                Out.WriteLine(e.StackTrace);

                Debug.WriteLine(e.Message, "Debug Error", ConsoleColor.Red);
            }
            return null;
        }

        public List<Hangar> GetHangars(Player player)
        {
            List<Hangar> hangars = new List<Hangar>();
            try
            {
                MySqlCommand query = new MySqlCommand("SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id);
                MySqlDataReader reader = new MySQLManager().Execute(query);

                while (reader.Read())
                {
                    int shipId = Convert.ToInt32(reader["SHIP_ID"]);
                    int x = Convert.ToInt32(reader["SHIP_X"]);
                    int y = Convert.ToInt32(reader["SHIP_Y"]);
                    int hp = Convert.ToInt32(reader["SHIP_HP"]);
                    int nano = Convert.ToInt32(reader["SHIP_NANO"]);
                    bool active = Convert.ToBoolean(reader["ACTIVE"]);
                    int mapId = Convert.ToInt32(reader["SHIP_MAP_ID"]);

                    hangars.Add(new Hangar(World.StorageManager.Ships[shipId], player.Drones, new Vector(x, y),
                        World.StorageManager.Spacemaps[mapId], hp, nano, new Dictionary<string, Item>(), active));
                }
                reader.Close();
                return hangars;
            }
            catch (Exception)
            {
                Debug.WriteLine("Error in LoadHangars()");
            }
            return hangars;
        }

        public void BasicSave(Player player)
        {
            try
            {
                MySqlCommand query =
                  new MySqlCommand("UPDATE player_data, player_extra_data, player_hangar"
                                + " SET "
                                + "player_data.LVL = " + player.Level.Id + ", player_data.EXP = " + player.Experience + ", "
                                + "player_data.HONOR = " + player.Honor + ", player_data.CREDITS = " + player.Credits + ", player_data.URIDIUM = " + player.Uridium + ", "
                                + "player_extra_data.CARGO = '" + player.Cargo + "', player_extra_data.STATS = '" + player.Stats + "', "
                                + "player_hangar.SHIP_MAP_ID = " + player.Spacemap.Id + ", player_hangar.SHIP_X = " + player.Position.X + ", player_hangar.SHIP_Y = " + player.Position.Y + " "
                                + "WHERE player_data.PLAYER_ID = " + player.Id + " AND player_extra_data.PLAYER_ID = " + player.Id + " AND player_hangar.PLAYER_ID = " + player.Id + " AND player_hangar.ACTIVE = 1");

                new MySQLManager().Execute(query);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in doing basic save ID-> {0}", player.Id);
            }

        }

        public void BasicRefresh(Player player)
        {
            try
            {
                MySqlCommand query =
                    new MySqlCommand("SELECT CREDITS, URIDIUM FROM player_data WHERE PLAYER_ID=" + player.Id + "");
                var reader = new MySQLManager().Execute(query);


                double dbCre = 0, dbUri = 0;
                while (reader.Read())
                {
                    dbCre = Convert.ToDouble(reader["CREDITS"]);
                    dbUri = Convert.ToDouble(reader["URIDIUM"]);
                }
                reader.Close();
                player.UserStorage.Refresh(dbCre, dbUri);

            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong during BasicRefresh() for player " + player.Id);
            }
        }

        public bool CheckWhitelist(int id)
        {
            return false;
        }

    }
}
