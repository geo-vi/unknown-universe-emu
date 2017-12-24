using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                    int absorb1 = intConv(queryRow["CONFIG_1_SHIELDABSORB"]);
                    int lcount1 = intConv(queryRow["CONFIG_1_LASERCOUNT"]);
                    int ltypes1 = intConv(queryRow["CONFIG_1_LASER_TYPES"]);
                    string rlTypes1 = queryRow["CONFIG_1_HEAVY"].ToString();

                    if (rlTypes1 == "") rlTypes1 = "[]";
                    if (extras1 == "") extras1 = "[]";
                    if (velocity1 == 0) velocity1 = player.Hangar.Ship.Speed;
                    var config1 = new Configuration(player, 1, dmg1, velocity1, shield1, shield1, absorb1, lcount1,
                        ltypes1, JsonConvert.DeserializeObject<int[]>(rlTypes1),
                        JsonConvert.DeserializeObject<List<Item>>(extras1).ToDictionary(x => x.LootId));

                    int dmg2 = intConv(queryRow["CONFIG_2_DMG"]);
                    int velocity2 = intConv(queryRow["CONFIG_2_SPEED"]);
                    string extras2 = queryRow["CONFIG_2_EXTRAS"].ToString();
                    int shield2 = intConv(queryRow["CONFIG_2_SHIELD"]);
                    int absorb2 = intConv(queryRow["CONFIG_2_SHIELDABSORB"]);
                    int lcount2 = intConv(queryRow["CONFIG_2_LASERCOUNT"]);
                    int ltypes2 = intConv(queryRow["CONFIG_2_LASER_TYPES"]);
                    string rlTypes2 = queryRow["CONFIG_2_HEAVY"].ToString();
                    if (velocity2 == 0) velocity2 = player.Hangar.Ship.Speed;
                    if (rlTypes2 == "") rlTypes2 = "[]";
                    if (extras2 == "") extras2 = "[]";
                    var config2 = new Configuration(player, 2, dmg2, velocity2, shield2, shield2, absorb2, lcount2,
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
                    baseInfo.SyncedValue = longConv(queryRow[getName].ToString());
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

        public void UpdateInfo(Player player, BaseInfo baseInfo, long amount_change)
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

        public void UpdateInfo(Player player, string row, long amount_change)
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
                        throw new NotImplementedException();
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
                        throw new NotImplementedException();
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

        public void SavePlayerPos(Player player)
        {
            try
            {
                if (player.Spacemap != null && player.Position != null)
                {
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        mySqlClient.ExecuteNonQuery($"UPDATE player_hangar SET SHIP_MAP_ID={player.Spacemap.Id}, SHIP_X={player.Position.X}, SHIP_Y={player.Position.Y} WHERE PLAYER_ID={player.Id} AND ACTIVE=1");
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
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow($"SELECT * FROM player_skill_tree WHERE PLAYER_ID={player.Id}");
                    return new Skilltree
                    {
                        Character = player,
                        AlienHunter = intConv(queryRow["ALIEN_HUNTER"]),
                        BountyHunter = intConv(queryRow["BOUNTY_HUNTER"]),
                        Cruelty = intConv(queryRow["CRUELTY"]),
                        Detonation = intConv(queryRow["DETONATION"]),
                        ElectroOptics = intConv(queryRow["ELECTRO_OPTICS"]),
                        Engineering = intConv(queryRow["ENGINEERING"]),
                        EvasiveManeuvers = intConv(queryRow["EVASIVE_MANEUVERS"]),
                        Explosives = intConv(queryRow["EXPLOSIVES"]),
                        Greed = intConv(queryRow["GREED"]),
                        HeatSeekingMissles = intConv(queryRow["HEAT_SEEKING_MISSLES"])
                    };
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
    }
}

