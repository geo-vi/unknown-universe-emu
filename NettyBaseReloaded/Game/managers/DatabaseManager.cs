using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Logger;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;

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
                            CargoDrop ship_drops = null;

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

                // Add 0-1 (prevents invalid mapId)
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
                    double absorb1 = Convert.ToDouble(queryRow["CONFIG_1_SHIELDABSORB"]) / 10000;
                    int lcount1 = intConv(queryRow["CONFIG_1_LASERCOUNT"]);

                    if (extras1 == "") extras1 = "[]";
                    var config1 = new Configuration(1, dmg1, velocity1, shield1, shield1, absorb1, lcount1, JsonConvert.DeserializeObject<List<Item>>(extras1).ToDictionary(x => x.LootId));

                    int dmg2 = intConv(queryRow["CONFIG_2_DMG"]);
                    int velocity2 = intConv(queryRow["CONFIG_2_SPEED"]);
                    string extras2 = queryRow["CONFIG_2_EXTRAS"].ToString();
                    int shield2 = intConv(queryRow["CONFIG_2_SHIELD"]);
                    double absorb2 = Convert.ToDouble(queryRow["CONFIG_2_SHIELDABSORB"]) / 10000;
                    int lcount2 = intConv(queryRow["CONFIG_2_LASERCOUNT"]);

                    if (extras2 == "") extras2 = "[]";
                    var config2 = new Configuration(2, dmg2, velocity2, shield2, shield2, absorb2, lcount2, JsonConvert.DeserializeObject<List<Item>>(extras2).ToDictionary(x => x.LootId));

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

                            drones.Add(new Drone(droneId, player.Id, (DroneType) (type + 1),
                                World.StorageManager.Levels.DroneLevels[level], exp, 0));
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
                        var spacemap = World.StorageManager.Spacemaps[mapId];
                        var currentHealth = intConv(querySet["SHIP_HP"]);
                        var currentNanohull = intConv(querySet["SHIP_NANO"]);
                        var hangar = new Hangar(ship, new List<Drone>(), position, spacemap, currentHealth, currentNanohull, new Dictionary<string, Item>());
                        var factionId = (Faction) intConv(querySet["FACTION_ID"]);
                        var levelId = intConv(querySet["LVL"]);
                        var rank = (Rank)(intConv(querySet["RANK"]));
                        var sessionId = stringConv(querySet["SESSION_ID"]);

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

                        querySet.Dispose();
                        player = new Player(playerId, name, hangar,currentHealth,currentNanohull, factionId, position, spacemap, null, null, sessionId, rank, false);
                    }
                }

            }
            catch (Exception e)
            {
                new ExceptionLog("dbmanager", "Failed getting player account [ID: " + playerId + "]", e);
            }
            return player;
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
                            int shipId = Convert.ToInt32(reader["SHIP_ID"]);
                            int x = Convert.ToInt32(reader["SHIP_X"]);
                            int y = Convert.ToInt32(reader["SHIP_Y"]);
                            int hp = Convert.ToInt32(reader["SHIP_HP"]);
                            int nano = Convert.ToInt32(reader["SHIP_NANO"]);
                            bool active = Convert.ToBoolean(reader["ACTIVE"]);
                            int mapId = Convert.ToInt32(reader["SHIP_MAP_ID"]);

                            Console.WriteLine("Added hangar");

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
                    baseInfo.SyncedValue = longConv(queryRow[getName].ToString());
                    baseInfo.LastTimeSynced = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                
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

            }
            return 0;
        }

        public void BasicSave(Player player)
        {

        }

        public void BasicRefresh(Player player)
        {

        }

        public bool CheckWhitelist(int id)
        {
            return false;
        }

    }
}

