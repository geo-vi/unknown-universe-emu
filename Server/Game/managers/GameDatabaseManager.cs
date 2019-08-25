using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Game.objects.server;
using Server.Main;
using Server.Main.managers;
using Server.Main.objects;
using Server.Utils;
using Hangar = Server.Game.objects.entities.Hangar;

namespace Server.Game.managers
{
    class GameDatabaseManager : AbstractDbUtils
    {
        public static GameDatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameDatabaseManager();
                }

                return _instance;
            }
        }

        private static GameDatabaseManager _instance;

        public Player CreatePlayer(int userId, bool newClient)
        {
            try
            {
                Player player = null;

                string cargoString = "";

                string settingsString = "";
                
                using (var mysqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mysqlClient.ExecuteQueryReader(
                        "SELECT * FROM player_data, player_extra_data WHERE player_data.PLAYER_ID = player_extra_data.PLAYER_ID AND player_data.PLAYER_ID = " +
                        userId);

                    while (queryRow.Read())
                    {
                        var globalId = Convert.ToInt32(queryRow["USER_ID"]);
                        var name = queryRow["PLAYER_NAME"].ToString();
                        var factionId = (Factions) (Convert.ToInt32(queryRow["FACTION_ID"]));
                        var rank = (Ranks) Convert.ToInt32(queryRow["RANK"]);
                        var clanId = Convert.ToInt32(queryRow["CLAN_ID"]);
                        var sessionId = queryRow["SESSION_ID"].ToString();

                        cargoString = queryRow["CARGO"].ToString();
                        
                        settingsString = queryRow["SETTINGS"].ToString();
                        
                        Global.StorageManager.GetClanById(clanId, true, out Clan outClan);

                        player = new Player(userId, globalId, name, outClan, factionId, sessionId, rank, newClient);
                    }
                }

                if (player == null)
                {
                    throw new Exception("Something went wrong while loading player");
                }

                var hangars = LoadPlayerHangars(userId);
                var equipment = LoadEquipment(userId);

                player.Equipment = new Equipment(player)
                {
                    Hangars = hangars,
                    Items = new ConcurrentDictionary<int, EquipmentItem>(equipment)
                };

                var ammo = CreatePlayerAmmunitions(userId);
                player.Ammunition = ammo;

                var information = LoadPlayerInformation(userId);
                player.Information = information;
                
                player.Cargo = LoadCargo(player, cargoString);

                player.Gates = LoadGates(userId);
                
                player.Settings = LoadPlayerSettings(player, settingsString);
                
                return player;
            }
            catch (Exception e)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
                Out.QuickLog(e.Message, LogKeys.ERROR_LOG);
            }

            return null;
        }
        
        public Ammunition CreatePlayerAmmunitions(int playerId)
        {
            try
            {
                var ammunition = new Ammunition();
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM player_ammo WHERE PLAYER_ID=" + playerId);
                    
                    ammunition.Create("ammunition_laser_lcb-10", intConv(queryRow["LCB_10"]));
                    ammunition.Create("ammunition_laser_mcb-25", intConv(queryRow["MCB_25"]));
                    ammunition.Create("ammunition_laser_mcb-50", intConv(queryRow["MCB_50"]));
                    ammunition.Create("ammunition_laser_ucb-100", intConv(queryRow["UCB_100"]));
                    ammunition.Create("ammunition_laser_sab-50", intConv(queryRow["SAB_50"]));
                    ammunition.Create("ammunition_laser_rsb-75", intConv(queryRow["RSB_75"]));
                    ammunition.Create("ammunition_laser_cbo-100", intConv(queryRow["CBO_100"]));
                    ammunition.Create("ammunition_laser_job-100", intConv(queryRow["JOB_100"]));
                    ammunition.Create("ammunition_rocket_r-310", intConv(queryRow["R_310"]));
                    ammunition.Create("ammunition_rocket_plt-2026", intConv(queryRow["PLT_2026"]));
                    ammunition.Create("ammunition_rocket_plt-2021", intConv(queryRow["PLT_2021"]));
                    ammunition.Create("ammunition_rocket_plt-3030", intConv(queryRow["PLT_3030"]));
                    ammunition.Create("ammunition_specialammo_pld-8", intConv(queryRow["PLD_8"]));
                    ammunition.Create("ammunition_specialammo_dcr-250", intConv(queryRow["DCR_250"]));
                    ammunition.Create("ammunition_specialammo_wiz-x", intConv(queryRow["WIZ_X"]));
                    ammunition.Create("ammunition_specialammo_emp-01", intConv(queryRow["EMP_01"]));
                    ammunition.Create("ammunition_rocket_bdr-1211", intConv(queryRow["BDR_1211"]));
                    ammunition.Create("ammunition_rocketlauncher_hstrm-01", intConv(queryRow["HSTRM_01"]));
                    ammunition.Create("ammunition_rocketlauncher_ubr-100", intConv(queryRow["UBR_100"]));
                    ammunition.Create("ammunition_rocketlauncher_eco-10", intConv(queryRow["ECO_10"]));
                    ammunition.Create("ammunition_rocketlauncher_sar-01", intConv(queryRow["SAR_01"]));
                    ammunition.Create("ammunition_rocketlauncher_sar-02", intConv(queryRow["SAR_02"]));
                    ammunition.Create("ammunition_mine_acm-01", intConv(queryRow["ACM_01"]));
                    ammunition.Create("ammunition_mine_empm-01", intConv(queryRow["EMP_M01"]));
                    ammunition.Create("ammunition_mine_slm-01", intConv(queryRow["SL_M01"]));
                    ammunition.Create("ammunition_mine_ddm-01", intConv(queryRow["DD_M01"]));
                    ammunition.Create("ammunition_mine_sabm-01", intConv(queryRow["SAB_M01"]));
                    ammunition.Create("ammunition_firework_fwx-s", intConv(queryRow["FWX_S"]));
                    ammunition.Create("ammunition_firework_fwx-m", intConv(queryRow["FWX_M"]));
                    ammunition.Create("ammunition_firework_fwx-l", intConv(queryRow["FWX_L"]));
                }
                return ammunition;
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }
            return null;
        }

        public ConcurrentDictionary<int, Hangar> LoadPlayerHangars(int playerId)
        {
            var hangars = new ConcurrentDictionary<int, Hangar>();
            var drones = LoadDrones(playerId);
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var queryTable =
                    mySqlClient.ExecuteQueryTable("SELECT * FROM player_hangar WHERE PLAYER_ID=" + playerId);

                foreach (DataRow row in queryTable.Rows)
                {
                    var id = Convert.ToInt32(row["ID"]);
                    var active = Convert.ToBoolean(Convert.ToInt32(row["ACTIVE"]));
                    var shipId = Convert.ToInt32(row["SHIP_ID"]);
                    var shipDesign = Convert.ToInt32(row["SHIP_DESIGN"]);
                    var hp = Convert.ToInt32(row["SHIP_HP"]);
                    var nano = Convert.ToInt32(row["SHIP_NANO"]);
                    var mapId = Convert.ToInt32(row["SHIP_MAP_ID"]);
                    var shipX = Convert.ToInt32(row["SHIP_X"]);
                    var shipY = Convert.ToInt32(row["SHIP_Y"]);

                    var hangar = new Hangar(id, GameStorageManager.Instance.FindShip(shipId), drones, new Vector(shipX, shipY), GameStorageManager.Instance.FindMap(mapId), hp, nano, active);
                    if (shipDesign != shipId)
                    {
                        hangar.ShipDesign = GameStorageManager.Instance.FindShip(shipDesign);
                    }

                    hangars.TryAdd(id, hangar);
                }
            }
            
            return hangars;
        }

        public Dictionary<int, EquipmentItem> LoadEquipment(int userId)
        {
            Dictionary<int, EquipmentItem> equipmentItems = new Dictionary<int, EquipmentItem>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable("SELECT * FROM player_equipment WHERE PLAYER_ID=" + userId);
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = Convert.ToInt32(row["ID"]);
                        var itemId = Convert.ToInt32(row["ITEM_ID"]);
                        var itemType = Convert.ToInt32(row["ITEM_TYPE"]);
                        var itemLvl = Convert.ToInt32(row["ITEM_LVL"]);
                        
                        var itemAmount = 1;
                        
                        var dbItemAmount = row["ITEM_AMOUNT"];
                        if (dbItemAmount != DBNull.Value)
                        {
                            itemAmount = Convert.ToInt32(dbItemAmount);
                        }
                        
                        var onConfig1 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_CONFIG_1"].ToString());
                        var onConfig2 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_CONFIG_2"].ToString());
                        var onDrone1 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_DRONE_ID_1"].ToString());
                        var onDrone2 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_DRONE_ID_2"].ToString());
                        var onPet1 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_PET_1"].ToString());
                        var onPet2 = JsonConvert.DeserializeObject<EquippedItem>(row["ON_PET_2"].ToString());
                        var activated = Convert.ToBoolean(Convert.ToInt32(row["ACTIVATED"]));

                        var equipmentItem = new EquipmentItem(id, itemId, itemType,
                            GameStorageManager.Instance.FindLootId(itemId), activated, itemLvl, itemAmount, onConfig1,
                            onConfig2,
                            onDrone1, onDrone2, onPet1, onPet2);
                        
                        equipmentItems.Add(id, equipmentItem);
                    }
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }

            return equipmentItems;
        }

        public void CreateSpacemaps()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_maps");
                foreach (DataRow row in queryTable.Rows)
                {
                    var id = Convert.ToInt32(row["ID"]);
                    var name = Convert.ToString(row["NAME"]);
                    var factionId = Convert.ToInt32(row["FACTION_ID"]);
                    var mapType = Convert.ToInt32(row["MAP_TYPE"]);
                    bool pvp = mapType == 1;
                    var starter = Convert.ToBoolean(Convert.ToInt32(row["IS_STARTER_MAP"]));
                    var level = Convert.ToInt32(row["LEVEL"]);

                    var npcs = JsonConvert.DeserializeObject<List<BaseNpc>>(row["NPCS"].ToString());

                    var portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["PORTALS"].ToString());

                    GameStorageManager.Instance.Spacemaps.TryAdd(id,
                        new Spacemap(id, name, (Factions) factionId, pvp, starter, level, npcs, portals));
                }
            }

            GameStorageManager.Instance.Spacemaps.TryAdd(51, new Spacemap(51, "GG α", Factions.NONE, false, false,
                0, new List<BaseNpc>(),
                new List<PortalBase>()) {Disabled = true, RangeDisabled = true});

            GameStorageManager.Instance.Spacemaps.TryAdd(52, new Spacemap(52, "GG β", Factions.NONE, false, false,
                    0, new List<BaseNpc>(),
                    new List<PortalBase>())
                {Disabled = true, RangeDisabled = true});

            GameStorageManager.Instance.Spacemaps.TryAdd(53, new Spacemap(53, "GG γ", Factions.NONE, false, false,
                    0, new List<BaseNpc>(),
                    new List<PortalBase>())
                {Disabled = true, RangeDisabled = true});
            
            GameStorageManager.Instance.Spacemaps.TryAdd(255,
                new Spacemap(255, "0-1", Factions.NONE, false, true, 0, null, null));

            Out.QuickLog($"Loaded {GameStorageManager.Instance.Spacemaps.Count} spacemaps to Game Storage", "dblog");
        }

        public void CreateItems()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_items");
                foreach (DataRow row in queryTable.Rows)
                {
                    var id = Convert.ToInt32(row["ID"]);
                    var name = row["NAME"].ToString();
                    var type = Convert.ToInt32(row["TYPE"]);
                    var lootId = row["LOOT_ID"].ToString();
                    var category = row["CATEGORY"].ToString();
                    var priceU = Convert.ToInt32(row["PRICE_U"]);
                    var priceC = Convert.ToInt32(row["PRICE_C"]);
                    var sellingCredits = Convert.ToInt32(row["SELLING_CREDITS"]);

                    /* POSSIBLE NULL */
                    int damage = 0;
                    if (row["DAMAGE"] != DBNull.Value)
                    {
                        damage = Convert.ToInt32(row["DAMAGE"]);
                    }

                    int shield = 0;
                    if (row["SHIELD"] != DBNull.Value)
                    {
                        shield = Convert.ToInt32(row["SHIELD"]);
                    }

                    int speed = 0;
                    if (row["SPEED"] != DBNull.Value)
                    {
                        speed = Convert.ToInt32(row["SPEED"]);
                    }

                    int uses = 0;
                    if (row["USES"] != DBNull.Value)
                    {
                        uses = Convert.ToInt32(row["USES"]);
                    }


                    var item = new Item(id, type, lootId, 1, 1)
                    {
                        Category = category,
                        PriceCredits = priceC,
                        PriceUridium = priceU,
                        Damage = damage,
                        Shield = shield,
                        Speed = speed,
                        Name = name,
                        Uses = uses
                    };

                    GameStorageManager.Instance.Items.TryAdd(id, item);
                }
            }

            Out.QuickLog($"Loaded {GameStorageManager.Instance.Items.Count} items to Game Storage", LogKeys.DATABASE_LOG);
        }

        public void CreateShips()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_ships");
                if (queryTable != null)
                {
                    foreach (DataRow reader in queryTable.Rows)
                    {
                        //Informations
                        var shipId = intConv(reader["ship_id"]);
                        var shipName = stringConv(reader["name"]);
                        var shipLootId = stringConv(reader["ship_lootid"]);
                        var shipHp = intConv(reader["ship_hp"]);
                        var shipNano = intConv(reader["nanohull"]);
                        var shipSpeed = intConv(reader["base_speed"]);
                        var shipShield = intConv(reader["shield"]);
                        var shipShieldAbsorb = intConv(reader["shieldAbsorb"]);
                        var shipMinDamage = intConv(reader["minDamage"]);
                        var shipMaxDamage = intConv(reader["maxDamage"]);
                        var shipNeutral = Convert.ToBoolean(intConv(reader["isNeutral"]));
                        var shipLaserColor = intConv(reader["laserID"]);
                        var shipBatteries = intConv(reader["batteries"]);
                        var shipRockets = intConv(reader["rockets"]);
                        var shipCargo = intConv(reader["cargo"]);
                        var shipAi = intConv(reader["isNeutral"]);

                        //Rewards
                        var shipExp = intConv(reader["experience"]);
                        var shipHonor = intConv(reader["honor"]);
                        var shipCredits = intConv(reader["credits"]);
                        var shipUridium = intConv(reader["uridium"]);

                        var rewards = new Dictionary<RewardTypes, int>();
                        rewards.Add(RewardTypes.EXPERIENCE, shipExp);
                        rewards.Add(RewardTypes.HONOR, shipHonor);
                        rewards.Add(RewardTypes.CREDITS, shipCredits);
                        rewards.Add(RewardTypes.URIDIUM, shipUridium);
                        var shipRewards = new Reward(rewards);

                        var shipOreDrop =
                            JsonConvert.DeserializeObject<OreCollection>(reader["dropJSON"].ToString());

                        var shipDrops = new DropRewards { OreCollection = shipOreDrop };

                        ////add to Storage
                        GameStorageManager.Instance.Ships.TryAdd(shipId, new Ship(
                            shipId,
                            shipName,
                            shipLootId,
                            shipHp,
                            shipNano,
                            shipSpeed,
                            shipShield,
                            shipShieldAbsorb,
                            shipMinDamage,
                            shipMaxDamage,
                            shipNeutral,
                            shipLaserColor,
                            shipBatteries,
                            shipRockets,
                            shipCargo,
                            shipRewards,
                            shipDrops,
                            shipAi
                        ));
                    }
                }
            }

            //World.StorageManager.LoadCatalog();
            Out.QuickLog("Loaded " + GameStorageManager.Instance.Ships.Count + " ships to Game Storage.", LogKeys.DATABASE_LOG);
        }

        /// <summary>
        /// Loading player's drones
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public ConcurrentDictionary<int, Drone> LoadDrones(int playerId)
        {
            var drones = new ConcurrentDictionary<int, Drone>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_drones WHERE PLAYER_ID = " + playerId);
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = intConv(row["ID"]);
                        var itemId = intConv(row["ITEM_ID"]);
                        var droneType = intConv(row["DRONE_TYPE"]);
                        var level = intConv(row["LEVEL"]);
                        var experience = intConv(row["EXPERIENCE"]);
                        var upgradeLevel = intConv(row["UPGRADE_LVL"]);
                        
                        var drone = new Drone(id, (DroneTypes)droneType, GameStorageManager.Instance.FindDroneLevel(level), experience, 0, upgradeLevel);
                        drones.TryAdd(id, drone);
                    }
                }

            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }

            return drones;
        }

        /// <summary>
        /// Loading all of the game's levels
        /// </summary>
        public void CreateGameLevels()
        {
            using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
            {
                CreatePlayerLevels(mySqlClient);

                CreateDroneLevels(mySqlClient);

                CreatePetLevels(mySqlClient);
            }
        }

        /// <summary>
        /// Loading Player levels
        /// </summary>
        /// <param name="client"></param>
        private void CreatePlayerLevels(SqlDatabaseClient client)
        {
            var queryTable = client.ExecuteQueryTable("SELECT * FROM server_levels_player");

            if (queryTable != null)
            {
                foreach (DataRow row in queryTable.Rows)
                {
                    var levelId = intConv(row["ID"]);
                    var experience = doubleConv(row["EXP"]);
                    GameStorageManager.Instance.PlayerLevels.TryAdd(levelId, new Level(levelId, experience));
                }
            }
            Out.QuickLog(
                "Loaded " + GameStorageManager.Instance.PlayerLevels.Count +
                " player levels to Game Storage.", LogKeys.DATABASE_LOG);
        }

        /// <summary>
        /// Loading Drone levels
        /// </summary>
        /// <param name="client"></param>
        private void CreateDroneLevels(SqlDatabaseClient client)
        {
            var queryTable = client.ExecuteQueryTable("SELECT * FROM server_levels_drone");

            if (queryTable != null)
            {
                foreach (DataRow row in queryTable.Rows)
                {
                    var levelId = intConv(row["ID"]);
                    var experience = doubleConv(row["EXP"]);
                    GameStorageManager.Instance.DroneLevels.TryAdd(levelId, new Level(levelId, experience));
                }
            }
            Out.QuickLog(
                "Loaded " + GameStorageManager.Instance.DroneLevels.Count +
                " drone levels to Game Storage.", LogKeys.DATABASE_LOG);
        }

        /// <summary>
        /// Loading PET levels
        /// </summary>
        /// <param name="client"></param>
        private void CreatePetLevels(SqlDatabaseClient client)
        {
            var queryTable = client.ExecuteQueryTable("SELECT * FROM server_levels_pet");

            if (queryTable != null)
            {
                foreach (DataRow row in queryTable.Rows)
                {
                    var levelId = intConv(row["ID"]);
                    var experience = doubleConv(row["EXP"]);
                    GameStorageManager.Instance.PetLevels.TryAdd(levelId, new Level(levelId, experience));
                }
            }
                
            Out.QuickLog(
                "Loaded " + GameStorageManager.Instance.PetLevels.Count +
                " pet levels to Game Storage.", LogKeys.DATABASE_LOG);
        }

        /// <summary>
        /// Loading the player's informations
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        private Information LoadPlayerInformation(int playerId)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow(
                        "SELECT EXP, HONOR, CREDITS, URIDIUM, JACKPOT, PREMIUM_UNTIL, LVL, BOOTY_KEYS FROM player_data, player_extra_data WHERE player_data.PLAYER_ID = player_extra_data.PLAYER_ID AND player_data.PLAYER_ID = " + playerId);
                    var exp = doubleConv(queryRow["EXP"]);
                    var honor = doubleConv(queryRow["HONOR"]);
                    var credits = doubleConv(queryRow["CREDITS"]);
                    var uridium = doubleConv(queryRow["URIDIUM"]);
                    var jackpot = Convert.ToSingle(queryRow["JACKPOT"]);
                    var premiumUntil = Convert.ToDateTime(queryRow["PREMIUM_UNTIL"]);
                    var levelId = Convert.ToInt32(queryRow["LVL"]);
                    var bootyKeys = JsonConvert.DeserializeObject<int[]>(queryRow["BOOTY_KEYS"].ToString());
                    if (bootyKeys == null || bootyKeys.Length != 3)
                    {
                        bootyKeys = new[] { 0, 0, 0};
                    }
                    var information = new Information(exp, honor, credits, uridium, jackpot, premiumUntil, GameStorageManager.Instance.FindPlayerLevel(levelId), bootyKeys);
                    return information;
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG); 
            }

            return null;
        }

        private PlayerGates LoadGates(int userId)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow("SELECT * FROM player_galaxy_gates WHERE PLAYER_ID=" + userId);
                    
                    var gates = new PlayerGates();
                    var completedGates = JsonConvert.DeserializeObject<int[]>(queryRow["COMPLETED_GATES"].ToString());
                    if (completedGates != null && completedGates.Length == 10)
                    {
                        gates.AlphaComplete = completedGates[0];
                        gates.BetaComplete = completedGates[1];
                        gates.GammaComplete = completedGates[2];
                        gates.DeltaComplete = completedGates[3];
                        gates.EpsilonComplete = completedGates[4];
                        gates.ZetaComplete = completedGates[5];
                        gates.KappaComplete = completedGates[6];
                        gates.KronosComplete = completedGates[7];
                        gates.LambdaComplete = completedGates[8];
                        gates.HadesComplete = completedGates[9];
                    }

                    // ALPHA Gate
                    var alphaPrepared = Convert.ToBoolean(intConv(queryRow["ALPHA_PREPARED"]));
                    gates.AlphaReady = alphaPrepared;
                    var alphaWave = intConv(queryRow["ALPHA_WAVE"]);
                    gates.AlphaWave = alphaWave;
                    var alphaLives = intConv(queryRow["ALPHA_LIVES"]);
                    gates.AlphaLives = alphaLives;

                    // BETA Gate
                    var betaPrepared = Convert.ToBoolean(intConv(queryRow["BETA_PREPARED"]));
                    gates.BetaReady = betaPrepared;
                    var betaWave = intConv(queryRow["BETA_WAVE"]);
                    gates.BetaWave = betaWave;
                    var betaLives = intConv(queryRow["BETA_LIVES"]);
                    gates.BetaLives = betaLives;

                    // GAMMA Gate
                    var gammaPrepared = Convert.ToBoolean(intConv(queryRow["GAMMA_PREPARED"]));
                    gates.GammaReady = gammaPrepared;
                    var gammaWave = intConv(queryRow["GAMMA_WAVE"]);
                    gates.GammaWave = gammaWave;
                    var gammaLives = intConv(queryRow["GAMMA_LIVES"]);
                    gates.GammaLives = gammaLives;

                    // DELTA Gate
                    var deltaPrepared = Convert.ToBoolean(intConv(queryRow["DELTA_PREPARED"]));
                    gates.DeltaReady = deltaPrepared;
                    var deltaWave = intConv(queryRow["DELTA_WAVE"]);
                    gates.DeltaWave = deltaWave;
                    var deltaLives = intConv(queryRow["DELTA_LIVES"]);
                    gates.DeltaLives = deltaLives;

                    // EPSILON Gate
                    var epsilonPrepared = Convert.ToBoolean(intConv(queryRow["EPSILON_PREPARED"]));
                    gates.EpsilonReady = epsilonPrepared;
                    var epsilonWave = intConv(queryRow["EPSILON_WAVE"]);
                    gates.EpsilonWave = epsilonWave;
                    var epsilonLives = intConv(queryRow["EPSILON_LIVES"]);
                    gates.EpsilonLives = epsilonLives;

                    // ZETA Gate
                    var zetaPrepared = Convert.ToBoolean(intConv(queryRow["ZETA_PREPARED"]));
                    gates.ZetaReady = zetaPrepared;
                    var zetaWave = intConv(queryRow["ZETA_WAVE"]);
                    gates.ZetaWave = zetaWave;
                    var zetaLives = intConv(queryRow["ZETA_LIVES"]);
                    gates.ZetaLives = zetaLives;

                    // KAPPA Gate
                    var kappaPrepared = Convert.ToBoolean(intConv(queryRow["KAPPA_PREPARED"]));
                    gates.KappaReady = kappaPrepared;
                    var kappaWave = intConv(queryRow["KAPPA_WAVE"]);
                    gates.KappaWave = kappaWave;
                    var kappaLives = intConv(queryRow["KAPPA_LIVES"]);
                    gates.KappaLives = kappaLives;

                    // LAMBDA Gate
                    var lambdaPrepared = Convert.ToBoolean(intConv(queryRow["LAMBDA_PREPARED"]));
                    gates.LambdaReady = lambdaPrepared;
                    var lambdaWave = intConv(queryRow["LAMBDA_WAVE"]);
                    gates.LambdaWave = lambdaWave;
                    var lambdaLives = intConv(queryRow["LAMBDA_LIVES"]);
                    gates.LambdaLives = lambdaLives;
                    
                    // LAMBDA Gate
                    var hadesPrepared = Convert.ToBoolean(intConv(queryRow["HADES_PREPARED"]));
                    gates.HadesReady = hadesPrepared;
                    var hadesWave = intConv(queryRow["HADES_WAVE"]);
                    gates.HadesWave = hadesWave;
                    var hadesLives = intConv(queryRow["HADES_LIVES"]);
                    gates.HadesLives = hadesLives;

                    return gates;
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG); 
            }

            return null;
        }

        private Cargo LoadCargo(Player player, string cargoString)
        {
            try
            {
                OreCollection ores = null;
                if (cargoString == "")
                {
                    ores = new OreCollection();
                }
                else
                {
                    var myCargo = JsonConvert.DeserializeObject<int[]>(cargoString);
                    ores = new OreCollection(myCargo[0], myCargo[1], myCargo[2],
                        myCargo[3], myCargo[4],myCargo[5],myCargo[6],
                        myCargo[7], myCargo[8]);
                }
                
                var cargo = new Cargo(player)
                {
                    Ores = ores
                };
                
                return cargo;
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }

            return null;
        }
        
        private PlayerSettings LoadPlayerSettings(Player player, string settingsString)
        {
            var playerSettings = new PlayerSettings(player);
            try
            {
                if (settingsString == "")
                {
                    playerSettings.CreateSettings();
                }
                else
                {
                    var settings = JsonConvert.DeserializeObject<AbstractSettings[]>(settingsString, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });

                    playerSettings.CreateSettings(settings);
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
                Out.WriteLog("Creating default settings for player", LogKeys.PLAYER_LOG, player.Id);
                playerSettings.CreateSettings();
            }
            return playerSettings;
        }

        public void SavePlayerSettings(Player player)
        {
            try
            {
                var settings = JsonConvert.SerializeObject(player.Settings.PackSettings(), Formatting.None, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_extra_data SET SETTINGS='{settings}' WHERE PLAYER_ID=" + player.Id);
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }
        }

        public void SaveCurrentHangar(Player player)
        {
            try
            {
                var position = player.Position;

                using (var mySqlCleint = SqlDatabaseManager.GetClient())
                {
                    mySqlCleint.ExecuteNonQuery(
                        $"UPDATE player_hangar SET SHIP_X={position.X}, SHIP_Y={position.Y}, SHIP_HP={player.CurrentHealth}, SHIP_NANO={player.CurrentNanoHull}, SHIP_MAP_ID={player.Spacemap.Id}, IN_EQUIPMENT_ZONE={Convert.ToInt32(CharacterStateManager.Instance.IsInState(player, CharacterStates.IN_EQUIPMENT_AREA))} WHERE PLAYER_ID={player.Id} AND ID={player.Hangar.Id}");
                }
            }
            catch (Exception)
            {
                Out.QuickLog("Critical error occured", LogKeys.ERROR_LOG);
            }
        }
    }
}