using MySql.Data.MySqlClient;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.extra;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Logger;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.Types;
using NettyBaseReloaded.Game.objects.world.events;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using Types = NettyBaseReloaded.Game.objects.world.map.pois.Types;

namespace NettyBaseReloaded.Game.managers
{
    class DatabaseManager : DBManagerUtils
    {
        public override void Initiate()
        {
            LoadServerInfo();
            LoadShips();
            LoadSpacemaps();
            LoadLevels();
            LoadCollectableRewards();
            LoadEvents();
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
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_ships");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            //Informations
                            int ship_id = intConv(reader["ship_id"]);
                            string ship_name = stringConv(reader["name"]);
                            string ship_lootId = stringConv(reader["ship_lootid"]);
                            int ship_hp = intConv(reader["ship_hp"]);
                            int ship_nano = intConv(reader["nanohull"]);
                            int ship_speed = intConv(reader["base_speed"]);
                            int ship_shield = intConv(reader["shield"]);
                            int ship_shieldAbsorb = intConv(reader["shieldAbsorb"]);
                            int ship_minDamage = intConv(reader["minDamage"]);
                            int ship_maxDamage = intConv(reader["maxDamage"]);
                            bool ship_neutral = Convert.ToBoolean(intConv(reader["isNeutral"]));
                            int ship_laserColor = intConv(reader["laserID"]);
                            int ship_batteries = intConv(reader["batteries"]);
                            int ship_rockets = intConv(reader["rockets"]);
                            int ship_cargo = intConv(reader["cargo"]);
                            int ship_ai = intConv(reader["isNeutral"]);

                            //Rewards
                            int ship_exp = intConv(reader["experience"]);
                            int ship_honor = intConv(reader["honor"]);
                            int ship_cre = intConv(reader["credits"]);
                            int ship_uri = intConv(reader["uridium"]);

                            var rewards = new Dictionary<RewardType, int>();
                            rewards.Add(RewardType.EXPERIENCE, ship_exp);
                            rewards.Add(RewardType.HONOR, ship_honor);
                            rewards.Add(RewardType.CREDITS, ship_cre);
                            rewards.Add(RewardType.URIDIUM, ship_uri);
                            var ship_rewards = new Reward(rewards);

                            ////@ToDo: Dropables
                            DropableRewards ship_drops = JsonConvert.DeserializeObject<DropableRewards>(reader["dropJSON"].ToString());

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
                        }
                    }

                }

                Log.Write("Loaded successfully " + World.StorageManager.Ships.Count + " ships from DB.");
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager","Failed to load ships...", e);
            }
        }

        public void LoadSpacemaps()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_maps");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            int map_id = intConv(row["ID"]);
                            string map_name = stringConv(row["NAME"]);
                            var map_faction = (Faction)intConv(row["FACTION_ID"]);

                            bool map_pvp = Convert.ToBoolean(0);
                            bool map_starter = Convert.ToBoolean(intConv(row["IS_STARTER_MAP"]));

                            var map_level = intConv(row["LEVEL"]);

                            var npcs = JsonConvert.DeserializeObject<List<BaseNpc>>(row["NPCS"].ToString());

                            var portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["PORTALS"].ToString());

                            World.StorageManager.Spacemaps.Add(map_id, new Spacemap(map_id, map_name, map_faction, map_pvp, map_starter, map_level, npcs, portals));
                        }
                    }
                }

                World.StorageManager.Spacemaps.Add(200, new Spacemap(200, "Lord of War", Faction.NONE, false, true, 0, new List<BaseNpc>(), new List<PortalBase>()) {POIs = new Dictionary<string, POI>
                {
                    {"bot_left", new POI("bot_left", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(3000, 15000), new Vector(3000, 4000), new Vector(4500, 4000), new Vector(4500, 15000) })},
                    {"bot_mid", new POI("bot_mid", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(11000, 10000), new Vector(11000, 8300), new Vector(4500, 8300), new Vector(4500, 10000) })},
                    {"top_mid", new POI("top_mid", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(5000, 2500), new Vector(5000, -2000), new Vector(11000, -2000), new Vector(11000, 2500) })},
                    {"top_right1", new POI("top_right1", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(14500, 2500), new Vector(16000, 2500), new Vector(16000, 5500), new Vector(14500, 5500) })},
                    {"top_right2", new POI("top_right2", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(16000, 5300), new Vector(22500, 5300), new Vector(22500, 7000), new Vector(16000, 7000) })},
                    {"bot_right", new POI("bot_right", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE, new List<Vector>{new Vector(14500, 17000), new Vector(14500, 9000), new Vector(13000, 9000), new Vector(13000, 17000) })},
                }, RangeDisabled = true
                });
                World.StorageManager.Spacemaps.Add(255, new Spacemap(255, "0-1", Faction.NONE, false, true, 0, null, null));

                Log.Write($"Loaded successfully {World.StorageManager.Spacemaps.Count} ships from DB.");
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Error loading maps", e);
            }
        }

        public void LoadLevels()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_levels_player");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var Id = intConv(row["ID"]);
                            var Exp = longConv(row["EXP"]);
                            World.StorageManager.Levels.PlayerLevels.Add(Id, new Level(Id, Exp));
                        }
                    }


                    queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_levels_drone");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var Id = intConv(row["ID"]);
                            var Exp = longConv(row["EXP"]);
                            World.StorageManager.Levels.DroneLevels.Add(Id, new Level(Id, Exp));
                        }
                    }

                    Log.Write(
                        "Loaded successfully " + World.StorageManager.Levels.PlayerLevels.Count +
                        " player levels from DB.");
                    Log.Write(
                        "Loaded successfully " + World.StorageManager.Levels.DroneLevels.Count +
                        " drone levels from DB.");
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Error loading levels", e);
            }

        }

        public void LoadCollectableRewards()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_collectables");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var id = intConv(row["ID"]);
                            var rewards = JsonConvert.DeserializeObject<List<Tuple<string, int>>>(row["REWARDS"].ToString());
                            var spawn_count = intConv(row["SPAWN_COUNT"]);
                            var pvp_spawn_count = intConv(row["PVP_SPAWN_COUNT"]);
                            switch (id)
                            {
                                case 2:
                                    BonusBox.REWARDS = rewards;
                                    BonusBox.SPAWN_COUNT = spawn_count;
                                    BonusBox.PVP_SPAWN_COUNT = pvp_spawn_count;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Error loading collectables", e);
            }
        }

        public void LoadTitles()
        {

        }

        //"SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id + " AND ACTIVE=1"
        public Hangar LoadHangar(Player player)
        {
            Hangar hangar = null;
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id +
                                                    " AND ACTIVE=1");

                    var ship = World.StorageManager.Ships[intConv(queryRow["SHIP_DESIGN"])];
                    var pos = new Vector(intConv(queryRow["SHIP_X"]), intConv(queryRow["SHIP_Y"]));
                    var mapId = intConv(queryRow["SHIP_MAP_ID"]);
                    var hp = intConv(queryRow["SHIP_HP"]);
                    var nano = intConv(queryRow["SHIP_NANO"]);

                    hangar = new Hangar(ship, null, pos, World.StorageManager.Spacemaps[mapId], hp, nano,
                        new Dictionary<string, Item>());
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Error loading player hangar [ID: " + player.Id + "]", e);
            }

            return hangar;
        }

        //"SELECT * FROM player_ship_config WHERE PLAYER_ID=" + player.Id
        public Configuration[] LoadConfig(Player player)
        {
            Configuration[] builder = null;
            try
            {
                using(var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT * FROM player_ship_config WHERE PLAYER_ID=" + player.Id);

                    int dmg1 = intConv(queryRow["CONFIG_1_DMG"]);
                    int velocity1 = intConv(queryRow["CONFIG_1_SPEED"]);
                    string extras1 = queryRow["CONFIG_1_EXTRAS"].ToString();
                    int shield1 = intConv(queryRow["CONFIG_1_SHIELD"]);
                    int shieldLeft1 = intConv(queryRow["CONFIG_1_SHIELD_LEFT"]);
                    int absorb1 = intConv(queryRow["CONFIG_1_SHIELDABSORB"]);
                    int lcount1 = intConv(queryRow["CONFIG_1_LASERCOUNT"]);
                    int ltypes1 = intConv(queryRow["CONFIG_1_LASER_TYPES"]);
                    string rlTypes1 = queryRow["CONFIG_1_HEAVY"].ToString();

                    if (rlTypes1 == "") rlTypes1 = "[]";
                    if (extras1 == "") extras1 = "[]";
                    if (velocity1 == 0) velocity1 = player.Hangar.Ship.Speed;
                    var config1 = new Configuration(player, 1, dmg1, velocity1, shield1, shieldLeft1, absorb1, lcount1,
                        ltypes1, JsonConvert.DeserializeObject<int[]>(rlTypes1),
                        JsonConvert.DeserializeObject<List<Item>>(extras1).ToDictionary(x => x.LootId));

                    int dmg2 = intConv(queryRow["CONFIG_2_DMG"]);
                    int velocity2 = intConv(queryRow["CONFIG_2_SPEED"]);
                    string extras2 = queryRow["CONFIG_2_EXTRAS"].ToString();
                    int shield2 = intConv(queryRow["CONFIG_2_SHIELD"]);
                    int shieldLeft2 = intConv(queryRow["CONFIG_2_SHIELD_LEFT"]);
                    int absorb2 = intConv(queryRow["CONFIG_2_SHIELDABSORB"]);
                    int lcount2 = intConv(queryRow["CONFIG_2_LASERCOUNT"]);
                    int ltypes2 = intConv(queryRow["CONFIG_2_LASER_TYPES"]);
                    string rlTypes2 = queryRow["CONFIG_2_HEAVY"].ToString();
                    if (velocity2 == 0) velocity2 = player.Hangar.Ship.Speed;
                    if (rlTypes2 == "") rlTypes2 = "[]";
                    if (extras2 == "") extras2 = "[]";
                    var config2 = new Configuration(player, 2, dmg2, velocity2, shield2, shieldLeft2, absorb2, lcount2,
                        ltypes2, JsonConvert.DeserializeObject<int[]>(rlTypes2),
                        JsonConvert.DeserializeObject<List<Item>>(extras2).ToDictionary(x => x.LootId));

                    builder = new Configuration[2]
                    {
                        config1,
                        config2
                    };
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed loading player configuration [ID: " + player.Id + "]", e);
            }
            return builder;
        }

        //SELECT * FROM player_drones WHERE PLAYER_ID=" + player.Id
        public List<Drone> LoadDrones(Player player)
        {
            List<Drone> drones = new List<Drone>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_drones WHERE PLAYER_ID=" + player.Id);

                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int droneId = intConv(reader["ID"]);
                            int type = intConv(reader["DRONE_TYPE"]);
                            int level = intConv(reader["LEVEL"]);
                            int exp = intConv(reader["EXPERIENCE"]);
                            int design = intConv(reader["DESIGN"]);

                            drones.Add(new Drone(droneId, player.Id, (DroneType) (type + 1),
                                World.StorageManager.Levels.DroneLevels[level], exp, 0,design));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed loading player drones [ID: " + player.Id + "]", e);
            }
            return drones;
        }

        public void LoadEvents()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_events");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int eventId = intConv(reader["ID"]);
                            string name = reader["NAME"].ToString();
                            EventTypes type = (EventTypes)intConv(reader["TYPE"]);
                            bool active = Convert.ToBoolean(intConv(reader["ACTIVE"]));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed loading events", e);
            }
        }

        public Player GetAccount(int playerId)
        {
            Player player = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    string sql =
                        "SELECT * FROM player_data, player_extra_data, player_hangar WHERE player_hangar.PLAYER_ID = player_data.PLAYER_ID AND player_extra_data.PLAYER_ID = player_data.PLAYER_ID AND player_hangar.ACTIVE=1 AND player_data.PLAYER_ID = " +
                        playerId;
                    var querySet = mySqlClient.ExecuteQueryReader(sql);

                    while (querySet.Read())
                    {
                        var name = stringConv(querySet["PLAYER_NAME"]);
                        var shipId = intConv(querySet["SHIP_DESIGN"]);
                        var ship = World.StorageManager.Ships[shipId];
                        var position = new Vector(intConv(querySet["SHIP_X"]), intConv(querySet["SHIP_Y"]));
                        var mapId = intConv(querySet["SHIP_MAP_ID"]);
                        var levelId = intConv(querySet["LVL"]);

                        if (!World.StorageManager.Spacemaps.ContainsKey(mapId))
                        {
                            Console.WriteLine("PROBLEM -> MAPID " + mapId + " DOESN'T EXIST!");
                            return null;
                        }

                        if (!World.StorageManager.Levels.PlayerLevels.ContainsKey(levelId))
                        {
                            Console.WriteLine("PROBLEM -> LEVELID " + levelId + " DOESN'T EXIST!");
                            return null;
                        }

                        var spacemap = World.StorageManager.Spacemaps[mapId];
                        var currentHealth = intConv(querySet["SHIP_HP"]);
                        var currentNanohull = intConv(querySet["SHIP_NANO"]);
                        var hangar = new Hangar(ship, new List<Drone>(), position, spacemap, currentHealth, currentNanohull, new Dictionary<string, Item>());
                        var factionId = (Faction) intConv(querySet["FACTION_ID"]);
                        var rank = (Rank)(intConv(querySet["RANK"]));
                        var sessionId = stringConv(querySet["SESSION_ID"]);
                        var clan = Global.StorageManager.Clans[0];
                        //var clan = playerId == 5036 ? Global.StorageManager.Clans[2] : Global.StorageManager.Clans[1];

                        player = new Player(playerId, name, clan, hangar,currentHealth,currentNanohull, factionId, position, spacemap, null, sessionId, rank, false);
                    }
                    querySet.Dispose();
                }

            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed getting player account [ID: " + playerId + "]", e);
            }
            return player;
        }

        internal Dictionary<string, Ammunition> LoadAmmunition(Player player)
        {
            var ammoDictionary = new Dictionary<string, Ammunition>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM player_ammo WHERE PLAYER_ID=" + player.Id);

                    ammoDictionary.Add("ammunition_laser_lcb-10", new Ammunition(player, "ammunition_laser_lcb-10", intConv(queryRow["LCB_10"])));
                    ammoDictionary.Add("ammunition_laser_mcb-25", new Ammunition(player, "ammunition_laser_mcb-25", intConv(queryRow["MCB_25"])));
                    ammoDictionary.Add("ammunition_laser_mcb-50", new Ammunition(player, "ammunition_laser_mcb-50", intConv(queryRow["MCB_50"])));
                    ammoDictionary.Add("ammunition_laser_ucb-100", new Ammunition(player, "ammunition_laser_ucb-100", intConv(queryRow["UCB_100"])));
                    ammoDictionary.Add("ammunition_laser_sab-50", new Ammunition(player, "ammunition_laser_sab-50", intConv(queryRow["SAB_50"])));
                    ammoDictionary.Add("ammunition_laser_rsb-75", new Ammunition(player, "ammunition_laser_rsb-75", intConv(queryRow["RSB_75"])));
                    ammoDictionary.Add("ammunition_laser_cbo-100", new Ammunition(player, "ammunition_laser_cbo-100", intConv(queryRow["CBO_100"])));
                    ammoDictionary.Add("ammunition_laser_job-100", new Ammunition(player, "ammunition_laser_job-100", intConv(queryRow["JOB_100"])));
                    ammoDictionary.Add("ammunition_rocket_r-310", new Ammunition(player, "ammunition_rocket_r-310", intConv(queryRow["R_310"])));
                    ammoDictionary.Add("ammunition_rocket_plt-2026", new Ammunition(player, "ammunition_rocket_plt-2026", intConv(queryRow["PLT_2026"])));
                    ammoDictionary.Add("ammunition_rocket_plt-2021", new Ammunition(player, "ammunition_rocket_plt-2021", intConv(queryRow["PLT_2021"])));
                    ammoDictionary.Add("ammunition_rocket_plt-3030", new Ammunition(player, "ammunition_rocket_plt-3030", intConv(queryRow["PLT_3030"])));
                    ammoDictionary.Add("ammunition_specialammo_pld-8", new Ammunition(player, "ammunition_specialammo_pld-8", intConv(queryRow["PLD_8"])));
                    ammoDictionary.Add("ammunition_specialammo_dcr-250", new Ammunition(player, "ammunition_specialammo_dcr-250", intConv(queryRow["DCR_250"])));
                    ammoDictionary.Add("ammunition_specialammo_wiz-x", new Ammunition(player, "ammunition_specialammo_wiz-x", intConv(queryRow["WIZ_X"])));
                    ammoDictionary.Add("ammunition_rocket_bdr-1211", new Ammunition(player, "ammunition_rocket_bdr-1211", intConv(queryRow["BDR_1211"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_hstrm-01", new Ammunition(player, "ammunition_rocketlauncher_hstrm-01", intConv(queryRow["HSTRM_01"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_ubr-100", new Ammunition(player, "ammunition_rocketlauncher_ubr-100", intConv(queryRow["UBR_100"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_eco-10", new Ammunition(player, "ammunition_rocketlauncher_eco-10", intConv(queryRow["ECO_10"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_sar-01", new Ammunition(player, "ammunition_rocketlauncher_sar-01", intConv(queryRow["SAR_01"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_sar-02", new Ammunition(player, "ammunition_rocketlauncher_sar-02", intConv(queryRow["SAR_02"])));
                    ammoDictionary.Add("ammunition_mine_acm-01", new Ammunition(player, "ammunition_mine_acm-01", intConv(queryRow["ACM_01"])));
                    ammoDictionary.Add("equipment_extra_cpu_ish-01", new Ammunition(player, "equipment_extra_cpu_ish-01", 100));
                    ammoDictionary.Add("ammunition_mine_smb-01", new Ammunition(player, "ammunition_mine_smb-01", 100));
                    ammoDictionary.Add("ammunition_specialammo_emp-01", new Ammunition(player, "ammunition_specialammo_emp-01", 100));
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed getting player ammuntion [ID: " + player.Id + "]", e);
                World.StorageManager.GetGameSession(player.Id)?.Disconnect(GameSession.DisconnectionType.ERROR);
            }
            return ammoDictionary;
        }

        public Dictionary<int,Hangar> LoadHangars(Player player)
        {
            Dictionary<int, Hangar> hangars = new Dictionary<int, Hangar>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id);

                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            var id = intConv(reader["HANGAR_COUNT"]);
                            int shipId = Convert.ToInt32(reader["SHIP_DESIGN"]);
                            int x = Convert.ToInt32(reader["SHIP_X"]);
                            int y = Convert.ToInt32(reader["SHIP_Y"]);
                            int hp = Convert.ToInt32(reader["SHIP_HP"]);
                            int nano = Convert.ToInt32(reader["SHIP_NANO"]);
                            bool active = Convert.ToBoolean(reader["ACTIVE"]);
                            int mapId = Convert.ToInt32(reader["SHIP_MAP_ID"]);

                            hangars.Add(id, new Hangar(World.StorageManager.Ships[shipId], player.Drones, new Vector(x, y),
                                World.StorageManager.Spacemaps[mapId], hp, nano, new Dictionary<string, Item>(),
                                active));
                        }
                    }
                }
                return hangars;
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Error loading hangars for player [ID: " + player.Id + "]", e);
            }
            return hangars;
        }

        public Statistics LoadStatistics(Player player)
        {
            return null;
        }

        public BaseInfo LoadInfo(Player player, BaseInfo baseInfo)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var getName = baseInfo.GetType().Name?.ToUpper();
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT " + getName + " FROM player_data WHERE PLAYER_ID=" + player.Id);
                    baseInfo.SyncedValue = doubleConv(queryRow[getName].ToString());
                    baseInfo.LastTimeSynced = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("error " + e);
            }
            return baseInfo;
        }

        public int LoadInfo(Player player, string row)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT " + row + " FROM player_data WHERE PLAYER_ID=" + player.Id);
                    return intConv(queryRow[row]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return 0;
        }

        public void UpdateInfo(Player player, BaseInfo baseInfo, double amount_change)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var getName = baseInfo.GetType().Name?.ToUpper();
                    mySqlClient.ExecuteNonQuery($"UPDATE player_data SET {getName}={getName}+{amount_change} WHERE PLAYER_ID={player.Id}");
                    baseInfo.SyncedValue = baseInfo.SyncedValue + amount_change;
                    baseInfo.LastTimeSynced = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
        }

        public void UpdateInfo(Player player, string row, double amount_change)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_data SET {row}={row}+{amount_change} WHERE PLAYER_ID={player.Id}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
        }

        public int UpdateAmmo(Ammunition ammunition, int ammo_change)
        {
            try
            {
                var row = Converter.AmmoToDbString(ammunition.LootId);
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_ammo SET {row}={row}-{ammo_change} WHERE PLAYER_ID={ammunition.Player.Id}");
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT " + row + " FROM player_ammo WHERE PLAYER_ID=" + ammunition.Player.Id);
                    return intConv(queryRow[row]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return -1;
        }

        public void BasicRefresh(Player player)
        {

        }

        public bool CheckWhitelist(int id)
        {
            return false;
        }

        public object GetPlayerGameplaySettings(Player player)
        {
            object userSettings = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var settingsVersion = "SETTINGS_GAMEPLAY_OLD";
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT " + settingsVersion + " FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    if (player.UsingNewClient)
                    {
                        userSettings = JsonConvert.DeserializeObject<netty.commands.new_client.UserSettingsCommand>(queryRow[settingsVersion].ToString());
                    }
                    else
                    {
                        userSettings = JsonConvert.DeserializeObject<netty.commands.old_client.UserSettingsCommand>(queryRow[settingsVersion].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return userSettings;
        }

        public void SavePlayerGameplaySettings(Settings settings)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    string query = "";
                    if (settings.Player.UsingNewClient)
                    {
                        Console.WriteLine("TODO: Save new client gameplay settings");
                        //throw new NotImplementedException();
                    }
                    else
                    {
                        query = $"UPDATE player_extra_data SET SETTINGS_GAMEPLAY_OLD='{JsonConvert.SerializeObject(settings.OldClientUserSettingsCommand)}' WHERE PLAYER_ID={settings.Player.Id}";
                    }
                    mySqlClient.ExecuteNonQuery(query);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        public object GetPlayerShipSettings(Player player)
        {
            object userSettings = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var settingsVersion = "SETTINGS_SLOTBAR_OLD";
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT " + settingsVersion + " FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    if (player.UsingNewClient)
                    {
                        //userSettings = JsonConvert.DeserializeObject<netty.commands.new_client.ShipSettingsCommand>(queryRow[settingsVersion].ToString());
                    }
                    else
                    {
                        userSettings = JsonConvert.DeserializeObject<netty.commands.old_client.ShipSettingsCommand>(queryRow[settingsVersion].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return userSettings;
        }

        public void SavePlayerShipSettings(Settings settings)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    string query = "";
                    if (settings.Player.UsingNewClient)
                    {
                        //TODO Save player ship settings for new client
                        Console.WriteLine("TODO Save player ship settings for new client");
                        //throw new NotImplementedException();
                    }
                    else
                    {
                        query = $"UPDATE player_extra_data SET SETTINGS_SLOTBAR_OLD='{JsonConvert.SerializeObject(settings.OldClientShipSettingsCommand)}' WHERE PLAYER_ID={settings.Player.Id}";
                    }
                    mySqlClient.ExecuteNonQuery(query);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }

        public void SavePlayerHangar(Player player)
        {
            try
            {
                if (player.Spacemap != null && player.Position != null)
                {
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        mySqlClient.ExecuteNonQuery($"UPDATE player_hangar SET SHIP_MAP_ID={player.Spacemap.Id}, SHIP_HP={player.CurrentHealth}, SHIP_NANO={player.CurrentNanoHull}, SHIP_X={player.Position.X}, SHIP_Y={player.Position.Y} WHERE PLAYER_ID={player.Id} AND ACTIVE=1");
                        player.Storage.DistancePassed = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed saving pos");
            }
        }

        public Skilltree LoadSkilltree(Player player)
        {
            Skilltree skilltree = new Skilltree(player);
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow($"SELECT * FROM player_skill_tree WHERE PLAYER_ID={player.Id}");

                    skilltree.AlienHunter = intConv(queryRow["ALIEN_HUNTER"]);
                    skilltree.BountyHunter = intConv(queryRow["BOUNTY_HUNTER"]);
                    skilltree.Cruelty = intConv(queryRow["CRUELTY"]);
                    skilltree.Detonation = intConv(queryRow["DETONATION"]);
                    skilltree.ElectroOptics = intConv(queryRow["ELECTRO_OPTICS"]);
                    skilltree.Engineering = intConv(queryRow["ENGINEERING"]);
                    skilltree.EvasiveManeuvers = intConv(queryRow["EVASIVE_MANEUVERS"]);
                    skilltree.Explosives = intConv(queryRow["EXPLOSIVES"]);
                    skilltree.Greed = intConv(queryRow["GREED"]);
                    skilltree.HeatSeekingMissles = intConv(queryRow["HEAT_SEEKING_MISSLES"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return null;
        }

        public bool LoadPremium(Player player)
        {
            bool premium = false;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow($"SELECT PREMIUM FROM player_data WHERE PLAYER_ID={player.Id}");
                    premium = Convert.ToBoolean(intConv(queryRow["PREMIUM"]));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return premium;
        }

        public void SetPlayerAssetsVersion(Settings settings)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_extra_data SET ASSETS_VERSION={settings.ASSET_VERSION} WHERE PLAYER_ID={settings.Player.Id}");
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void UpdateDrone(Drone drone)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_drones SET EXPERIENCE={drone.Experience}, LEVEL={drone.Level.Id} WHERE ID={drone.Id}");
                }
            }
            catch (Exception)
            {

            }
        }

        public void SaveConfig(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    //CONFIG_1_EXTRAS={JsonConvert.SerializeObject(Extra.ToItems(player.Hangar.Configurations[0].Extras))}, CONFIG_2_EXTRAS={JsonConvert.SerializeObject(Extra.ToItems(player.Hangar.Configurations[1].Extras))}
                    mySqlClient.ExecuteNonQuery($"UPDATE player_ship_config SET CONFIG_1_SHIELD_LEFT={player.Hangar.Configurations[0].CurrentShield}, CONFIG_2_SHIELD_LEFT={player.Hangar.Configurations[1].CurrentShield} " +
                                                $"WHERE PLAYER_ID={player.Id}");
                }
            }
            catch (Exception)
            {

            }
        }

        public int GetPlayerAssetsVersion(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow($"SELECT ASSETS_VERSION FROM player_extra_data WHERE PLAYER_ID={player.Id}");
                    return intConv(queryRow["ASSETS_VERSION"]);
                }
            }
            catch (Exception)
            {

            }
            return 0;
        }

        public void UpdateEventForPlayer(PlayerEvent playerEvent)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"INSERT INTO player_event_info (PLAYER_ID, EVENT_ID, SCORE, DATA) VALUES('{playerEvent.Player.Id}', '{playerEvent.Id}', '{playerEvent.Score}', '{JsonConvert.SerializeObject(playerEvent)}') ON DUPLICATE KEY UPDATE SCORE='{playerEvent.Score}', DATA='{JsonConvert.SerializeObject(playerEvent)}'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoadEventForPlayer(int id, Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var query = mySqlClient.ExecuteQueryRow(
                        $"SELECT * FROM player_event_info WHERE EVENT_ID={id} AND PLAYER_ID={player.Id}");
                    var eventData = JsonConvert.DeserializeObject<PlayerEvent>(query["DATA"].ToString());

                    if (eventData != null)
                    {
                        if (!player.EventsPraticipating.ContainsKey(id))
                            player.EventsPraticipating.Add(id, eventData);
                        else player.EventsPraticipating[id] = eventData;
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager_load_event", $"PlayerId: {player.Id}, EventId: {id}", e);
            }
        }

        public void UpdateServerEvent(int id, bool active)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE server_events SET ACTIVE={intConv(active)} WHERE ID={id}");
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager_server_event", "event", e);
            }
        }

        public Killscreen GetLastKillscreen(int playerId)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow(
                        $"SELECT * FROM player_deaths WHERE PLAYER_ID={playerId} ORDER BY id DESC LIMIT 1");
                    var id = Convert.ToInt32(queryRow["ID"]);
                    var killerName = queryRow["KILLER_NAME"].ToString();
                    var killerLink = queryRow["KILLER_LINK"].ToString();
                    var deathType = (DeathType) (Convert.ToInt32(queryRow["DEATH_TYPE"]));
                    var alias = queryRow["ALIAS"].ToString();
                    var tod = queryRow["TIME_OF_DEATH"].ToString();
                    Console.WriteLine($"{id} -> {killerName} -> {killerLink} -> {deathType} -> {alias} -> {tod}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;// TODO
        }
    }
}

