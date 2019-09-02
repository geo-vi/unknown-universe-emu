using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.events;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players.events;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using Types = NettyBaseReloaded.Game.objects.world.map.pois.Types;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.pets.gears;
using NettyBaseReloaded.Game.objects.world.players.quests;
using NettyBaseReloaded.Game.objects.world.players.quests.serializables;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using NettyBaseReloaded.Game.objects.world.map.collectables.rewards;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.players.equipment.item;
using NettyBaseReloaded.Game.objects.world.players.extra;
using NettyBaseReloaded.Game.objects.world.players.extra.boosters;
using DroneFormation = NettyBaseReloaded.Game.objects.world.DroneFormation;

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
            LoadItems();
            LoadTitles();
            LoadQuests();
            //LoadClanBattleStations();
            //LoadEquippedModules();
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

                            DropableRewards ship_drops =
                                JsonConvert.DeserializeObject<DropableRewards>(reader["dropJSON"].ToString());

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
                World.StorageManager.LoadCatalog();
                Out.WriteDbLog("Loaded successfully " + World.StorageManager.Ships.Count + " ships from DB.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading ships, " + e.Message);
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
                            var map_faction = (Faction) intConv(row["FACTION_ID"]);

                            bool map_pvp = Convert.ToBoolean(0);
                            bool map_starter = Convert.ToBoolean(intConv(row["IS_STARTER_MAP"]));

                            var map_level = intConv(row["LEVEL"]);

                            var npcs = JsonConvert.DeserializeObject<List<BaseNpc>>(row["NPCS"].ToString());

                            var portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["PORTALS"].ToString());

                            World.StorageManager.Spacemaps.Add(map_id,
                                new Spacemap(map_id, map_name, map_faction, map_pvp, map_starter, map_level, npcs,
                                    portals));
                        }
                    }

                }

                World.StorageManager.Spacemaps.Add(51, new Spacemap(51, "GG α", Faction.NONE, false, false, 0, new List<BaseNpc>(), 
                    new List<PortalBase>()) { Disabled = true, RangeDisabled = true });

                World.StorageManager.Spacemaps.Add(52, new Spacemap(52, "GG β", Faction.NONE, false, false, 0, new List<BaseNpc>(),
                        new List<PortalBase>())
                    { Disabled = true, RangeDisabled = true });

                World.StorageManager.Spacemaps.Add(53, new Spacemap(53, "GG γ", Faction.NONE, false, false, 0, new List<BaseNpc>(),
                        new List<PortalBase>())
                    { Disabled = true, RangeDisabled = true });


                World.StorageManager.Spacemaps.Add(200,
                    new Spacemap(200, "Lord of War", Faction.NONE, false, false, 0, new List<BaseNpc>(),
                        new List<PortalBase>())
                    {
                        POIs = new Dictionary<string, POI>
                        {
                            {
                                "bot_left",
                                new POI("bot_left", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(3000, 15000),
                                        new Vector(3000, 4000),
                                        new Vector(4500, 4000),
                                        new Vector(4500, 15000)
                                    })
                            },
                            {
                                "bot_mid",
                                new POI("bot_mid", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(11000, 10000),
                                        new Vector(11000, 8300),
                                        new Vector(4500, 8300),
                                        new Vector(4500, 10000)
                                    })
                            },
                            {
                                "top_mid",
                                new POI("top_mid", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(5000, 2500),
                                        new Vector(5000, -2000),
                                        new Vector(11000, -2000),
                                        new Vector(11000, 2500)
                                    })
                            },
                            {
                                "top_right1",
                                new POI("top_right1", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(14500, 2500),
                                        new Vector(16000, 2500),
                                        new Vector(16000, 5500),
                                        new Vector(14500, 5500)
                                    })
                            },
                            {
                                "top_right2",
                                new POI("top_right2", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(16000, 5300),
                                        new Vector(22500, 5300),
                                        new Vector(22500, 7000),
                                        new Vector(16000, 7000)
                                    })
                            },
                            {
                                "bot_right",
                                new POI("bot_right", Types.NO_ACCESS, Designs.SIMPLE, Shapes.RECTANGLE,
                                    new List<Vector>
                                    {
                                        new Vector(14500, 17000),
                                        new Vector(14500, 9000),
                                        new Vector(13000, 9000),
                                        new Vector(13000, 17000)
                                    })
                            },
                        },
                        RangeDisabled = true,
                        Disabled = true
                    });

                World.StorageManager.Spacemaps.Add(255,
                    new Spacemap(255, "0-1", Faction.NONE, false, true, 0, null, null));

                Out.WriteDbLog($"Loaded successfully {World.StorageManager.Spacemaps.Count} ships from DB.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading spacemaps, " + e.Message);
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
                            var Exp = doubleConv(row["EXP"]);
                            World.StorageManager.Levels.PlayerLevels.Add(Id, new Level(Id, Exp));
                        }
                    }


                    queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_levels_drone");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var Id = intConv(row["ID"]);
                            var Exp = doubleConv(row["EXP"]);
                            World.StorageManager.Levels.DroneLevels.Add(Id, new Level(Id, Exp));
                        }
                    }

                    queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_levels_pet");

                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var Id = intConv(row["ID"]);
                            var Exp = doubleConv(row["EXP"]);
                            World.StorageManager.Levels.PetLevels.Add(Id, new Level(Id, Exp));
                        }
                    }

                    Out.WriteDbLog(
                        "Loaded successfully " + World.StorageManager.Levels.PlayerLevels.Count +
                        " player levels from DB.");
                    Out.WriteDbLog(
                        "Loaded successfully " + World.StorageManager.Levels.DroneLevels.Count +
                        " drone levels from DB.");
                    Out.WriteDbLog(
                        "Loaded successfully " + World.StorageManager.Levels.PetLevels.Count +
                        " pet levels from DB.");

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading levels, " + e.Message);
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
                            var rewards =
                                JsonConvert.DeserializeObject<List<PotentialReward>>(row["REWARDS"].ToString());
                            var spawn_count = intConv(row["SPAWN_COUNT"]);
                            var pvp_spawn_count = intConv(row["PVP_SPAWN_COUNT"]);
                            switch (id)
                            {
                                case 2:
                                    //bonusbox
                                    BonusBox.REWARDS = rewards;
                                    BonusBox.SPAWN_COUNT = spawn_count;
                                    BonusBox.PVP_SPAWN_COUNT = pvp_spawn_count;
                                    break;
                                case 20:
                                    //pirate booty
                                    break;
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading collectable rewards, " + e.Message);
            }
        }

        public void LoadTitles()
        {
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_titles");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int title_id = intConv(reader["ID"]);
                            string title_key = stringConv(reader["KEY"]);
                            string title_name = stringConv(reader["TITLE_NAME"]);
                            int title_color = intConv(reader["TITLE_COLOR"]);
                            string title_color_hex = stringConv(reader["TITLE_COLOR_HEX"]);

                            World.StorageManager.Titles.Add(title_id, new Title(title_id, title_key, title_name, title_color, title_color_hex));
                        }
                    }

                }

                Out.WriteDbLog("Loaded successfully " + World.StorageManager.Titles.Count + " titles from DB.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading titles, " + e.Message);
            }

        }
        
        public void LoadQuests()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_quests");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            var id = intConv(reader["ID"]);
                            var root = reader["ROOT"].ToString();
                            var reward = reader["REWARDS"].ToString();
                            var type = reader["TYPE"].ToString();
                            var icon = reader["ICON"].ToString();
                            var expiryDate = DateTime.Parse(reader["EXPIRY_DATE"].ToString());
                            int dayOfWeek = 0;
                            if (reader.IsNull("DAY_OF_WEEK"))
                            {
                                dayOfWeek = -1;
                            }
                            else intConv(reader["DAY_OF_WEEK"]);

                            QuestIcons questIcon;
                            if (!Enum.TryParse(icon, out questIcon))
                            {
                                questIcon = QuestIcons.DISCOVER;
                            }

                            QuestTypes questType;
                            if (!Enum.TryParse(type, out questType))
                            {
                                questType = QuestTypes.UNDEFINED;
                            }
                            
                            QuestLoader loader = new QuestLoader()
                            {
                                Id = id, Root = JsonConvert.DeserializeObject<QuestRoot>(root),
                                DayOfWeek = dayOfWeek, ExpireDate = expiryDate, Icon = questIcon, QuestType = questType,
                                Rewards = JsonConvert.DeserializeObject<QuestSerializableReward>(reward)
                            };
                            World.StorageManager.Quests.Add(id, QuestLoader.Load(loader));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading Quests, " + e.Message);
            }
        }

        public void LoadClanBattleStations()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_clanbattlestations");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            var id = Convert.ToInt32(reader["ID"]);
                            var name = reader["NAME"].ToString();
                            var faction = (Faction) (intConv(reader["FACTION"]));
                            var type = intConv(reader["TYPE"]);
                            var mapId = intConv(reader["MAP_ID"]);
                            var clanId = intConv(reader["CLAN_ID"]);
                            var pos = reader["POSITION"].ToString();
                            var hp = intConv(reader["HEALTH"]);
                            var shield = intConv(reader["SHIELD"]);
                            var modules = reader["MODULES"].ToString();
                            var buildStart = reader["BUILD_START"];
                            var buildEnd = reader["BUILD_END"];
                            var deflectorEnd = reader["DEFLECTOR_END"];

                            var posSplit = pos.Split('|');
                            var posVector = new Vector(intConv(posSplit[0]), intConv(posSplit[1]));
                            var map = World.StorageManager.Spacemaps[mapId];
                            if (type == 1)
                            {
                                var cbs = new ClanBattleStation(map.GetNextObjectId(), id, name, faction, posVector,
                                    map, null, new Dictionary<int, BattleStationModule>());
                                World.StorageManager.ClanBattleStations.Add(id, cbs);
                                map.AddObject(cbs);
                            }
                            else
                            {
                                var asteroid = new Asteroid(map.GetNextObjectId(), id, name, posVector, map);
                                World.StorageManager.Asteroids.Add(id, asteroid);
                                map.AddObject(asteroid);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void LoadItems()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM server_items");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            var id = intConv(reader["ID"]);
                            var typeId = intConv(reader["TYPE"]);
                            var lootId = reader["LOOT_ID"].ToString();
                            var amount = 0;
                            if (!reader.IsNull("USES"))
                            {
                                amount = intConv(reader["USES"]);
                            }
                            World.StorageManager.Items.Add(id, new Item(id, typeId, lootId, amount));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public Dictionary<int, Hangar> LoadHangar(Player player)
        {
            var drones = LoadDrones(player);
            var hangars = new Dictionary<int, Hangar>();
            try
            {
                using (SqlDatabaseClient mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable("SELECT * FROM player_hangar WHERE PLAYER_ID=" + player.Id);

                    foreach (DataRow queryRow in queryTable.Rows)
                    {
                        var id = intConv(queryRow["ID"]);
                        var ship = World.StorageManager.Ships[intConv(queryRow["SHIP_DESIGN"])];
                        var pos = new Vector(intConv(queryRow["SHIP_X"]), intConv(queryRow["SHIP_Y"]));
                        var mapId = intConv(queryRow["SHIP_MAP_ID"]);
                        var hp = intConv(queryRow["SHIP_HP"]);
                        var nano = intConv(queryRow["SHIP_NANO"]);
                        Spacemap map;
                        if (World.StorageManager.Spacemaps.ContainsKey(mapId))
                            map = World.StorageManager.Spacemaps[mapId];
                        else map = World.StorageManager.Spacemaps[255];

                        var active = Convert.ToBoolean(intConv(queryRow["ACTIVE"]));

                        var hangar = new Hangar(id, ship, drones, pos, map, hp, nano, active);
                        hangars.Add(id, hangar);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading hangar, " + e.Message);
            }
            return hangars;
        }

        public Dictionary<int, EquipmentItem> LoadEquipment(Player player)
        {
            var items = new Dictionary<int, EquipmentItem>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_equipment WHERE PLAYER_ID=" + player.Id);
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = intConv(row["ID"]);
                        var itemId = intConv(row["ITEM_ID"]);
                        var itemType = intConv(row["ITEM_TYPE"]);
                        var itemLvl = intConv(row["ITEM_LVL"]);
                        var onConfig1JSON = row["ON_CONFIG_1"].ToString();
                        var onConfig1 = JsonConvert.DeserializeObject<EquippedItem>(onConfig1JSON);
                        var onConfig2JSON = row["ON_CONFIG_2"].ToString();
                        var onConfig2 = JsonConvert.DeserializeObject<EquippedItem>(onConfig2JSON);
                        var onDrone1JSON = row["ON_DRONE_ID_1"].ToString();
                        var onDrone1 = JsonConvert.DeserializeObject<EquippedItem>(onDrone1JSON);
                        var onDrone2JSON = row["ON_DRONE_ID_2"].ToString();
                        var onDrone2 = JsonConvert.DeserializeObject<EquippedItem>(onDrone2JSON);
                        var onPet1JSON = row["ON_PET_1"].ToString();
                        var onPet1 = JsonConvert.DeserializeObject<EquippedItem>(onPet1JSON);
                        var onPet2JSON = row["ON_PET_2"].ToString();
                        var onPet2 = JsonConvert.DeserializeObject<EquippedItem>(onPet2JSON);
                        var itemAmount = intConv(row["ITEM_AMOUNT"]);
                        var item = new EquipmentItem(id, player, Item.Find(itemId), itemLvl, onConfig1, onConfig2,
                            onDrone1, onDrone2, onPet1, onPet2, itemAmount);
                        items.Add(id, item);
                    }
                }
            }
            catch (Exception)
            {
            }

            return items;
        }

        public Dictionary<int, Drone> LoadDrones(Player player)
        {
            var drones = new Dictionary<int, Drone>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable("SELECT * FROM player_drones WHERE PLAYER_ID=" + player.Id);

                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int droneId = intConv(reader["ID"]);
                            int type = intConv(reader["DRONE_TYPE"]);
                            int level = intConv(reader["LEVEL"]);
                            int exp = intConv(reader["EXPERIENCE"]);
                            var upgLevel = intConv(reader["UPGRADE_LVL"]);

                            drones.Add(droneId, new Drone(droneId, player, (DroneType) (type + 1),
                                World.StorageManager.Levels.DroneLevels[level], exp, 0, upgLevel));
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading Drones, " + e.Message);
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
                            EventTypes type = (EventTypes) intConv(reader["TYPE"]);
                            bool active = Convert.ToBoolean(intConv(reader["ACTIVE"]));
                            GameEvent gameEvent;
                            switch (type)
                            {
                                case EventTypes.SPACEBALL:
                                    gameEvent = new SpaceballGameEvent(eventId, name, type, active);
                                    break;
                                default:
                                    gameEvent = new GameEvent(eventId, name, type, active);
                                    break;
                            }
                            if (active)
                                gameEvent.Start();
                            World.StorageManager.Events.Add(eventId, gameEvent);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading Events, " + e.Message);
            }
        }

        public void LoadEquippedModules()
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_modules WHERE EQUIPPED=1");
                    if (queryTable != null)
                    {
                        foreach (DataRow reader in queryTable.Rows)
                        {
                            int moduleId = intConv(reader["ID"]);
                            var ownerId = intConv(reader["PLAYER_ID"]);
                            var type = (Module.Types) (intConv(reader["TYPE"]));
                            var cbsId = intConv(reader["CBS_ID"]);
                            var position = intConv(reader["POSITION"]); // slot id (0-9)
                            var hp = intConv(reader["HP"]);
                            var shield = intConv(reader["SHIELD"]);
                            var upgradeLvl = intConv(reader["UPGRADE_LVL"]);

                            if (World.StorageManager.Asteroids.ContainsKey(cbsId))
                            {
                                //var asteroid = World.StorageManager.Asteroids[cbsId];
                                //var module = new Module(type, new Item(moduleId, "", 1), true);
                                //var bModule = BattleStationModule.Equip(null, module, asteroid, position, 0);
                                //asteroid.EquippedModules.Add(moduleId, bModule);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("WHATS GOING ON HERE?!??!?!?!");
                Console.WriteLine(e.Message);
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
                        "SELECT * FROM player_data WHERE PLAYER_ID = " +
                        playerId;
                    var querySet = mySqlClient.ExecuteQueryRow(sql);

                    var globalId = intConv(querySet["USER_ID"]);
                    var name = stringConv(querySet["PLAYER_NAME"]);
                    var factionId = (Faction) intConv(querySet["FACTION_ID"]);
                    var rank = (Rank) (intConv(querySet["RANK"]));
                    var sessionId = stringConv(querySet["SESSION_ID"]);
                    var clan = Global.StorageManager.GetClan(intConv(querySet["CLAN_ID"]));
                    player = new Player(playerId, globalId, name, clan, factionId,
                         sessionId, rank, false);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading account, " + e.Message + " " + e.StackTrace);
            }
            return player;
        }

        public Dictionary<string, Ammunition> LoadAmmunition(Player player)
        {
            var ammoDictionary = new Dictionary<string, Ammunition>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT * FROM player_ammo WHERE PLAYER_ID=" + player.Id);

                    ammoDictionary.Add("ammunition_laser_lcb-10",
                        new Ammunition(player, "ammunition_laser_lcb-10", intConv(queryRow["LCB_10"])));
                    ammoDictionary.Add("ammunition_laser_mcb-25",
                        new Ammunition(player, "ammunition_laser_mcb-25", intConv(queryRow["MCB_25"])));
                    ammoDictionary.Add("ammunition_laser_mcb-50",
                        new Ammunition(player, "ammunition_laser_mcb-50", intConv(queryRow["MCB_50"])));
                    ammoDictionary.Add("ammunition_laser_ucb-100",
                        new Ammunition(player, "ammunition_laser_ucb-100", intConv(queryRow["UCB_100"])));
                    ammoDictionary.Add("ammunition_laser_sab-50",
                        new Ammunition(player, "ammunition_laser_sab-50", intConv(queryRow["SAB_50"])));
                    ammoDictionary.Add("ammunition_laser_rsb-75",
                        new Ammunition(player, "ammunition_laser_rsb-75", intConv(queryRow["RSB_75"])));
                    ammoDictionary.Add("ammunition_laser_cbo-100",
                        new Ammunition(player, "ammunition_laser_cbo-100", intConv(queryRow["CBO_100"])));
                    ammoDictionary.Add("ammunition_laser_job-100",
                        new Ammunition(player, "ammunition_laser_job-100", intConv(queryRow["JOB_100"])));
                    ammoDictionary.Add("ammunition_rocket_r-310",
                        new Ammunition(player, "ammunition_rocket_r-310", intConv(queryRow["R_310"])));
                    ammoDictionary.Add("ammunition_rocket_plt-2026",
                        new Ammunition(player, "ammunition_rocket_plt-2026", intConv(queryRow["PLT_2026"])));
                    ammoDictionary.Add("ammunition_rocket_plt-2021",
                        new Ammunition(player, "ammunition_rocket_plt-2021", intConv(queryRow["PLT_2021"])));
                    ammoDictionary.Add("ammunition_rocket_plt-3030",
                        new Ammunition(player, "ammunition_rocket_plt-3030", intConv(queryRow["PLT_3030"])));
                    ammoDictionary.Add("ammunition_specialammo_pld-8",
                        new Ammunition(player, "ammunition_specialammo_pld-8", intConv(queryRow["PLD_8"])));
                    ammoDictionary.Add("ammunition_specialammo_dcr-250",
                        new Ammunition(player, "ammunition_specialammo_dcr-250", intConv(queryRow["DCR_250"])));
                    ammoDictionary.Add("ammunition_specialammo_wiz-x",
                        new Ammunition(player, "ammunition_specialammo_wiz-x", intConv(queryRow["WIZ_X"])));
                    ammoDictionary.Add("ammunition_rocket_bdr-1211",
                        new Ammunition(player, "ammunition_rocket_bdr-1211", intConv(queryRow["BDR_1211"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_hstrm-01",
                        new Ammunition(player, "ammunition_rocketlauncher_hstrm-01", intConv(queryRow["HSTRM_01"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_ubr-100",
                        new Ammunition(player, "ammunition_rocketlauncher_ubr-100", intConv(queryRow["UBR_100"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_eco-10",
                        new Ammunition(player, "ammunition_rocketlauncher_eco-10", intConv(queryRow["ECO_10"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_sar-01",
                        new Ammunition(player, "ammunition_rocketlauncher_sar-01", intConv(queryRow["SAR_01"])));
                    ammoDictionary.Add("ammunition_rocketlauncher_sar-02",
                        new Ammunition(player, "ammunition_rocketlauncher_sar-02", intConv(queryRow["SAR_02"])));
                    ammoDictionary.Add("ammunition_mine_acm-01",
                        new Ammunition(player, "ammunition_mine_acm-01", intConv(queryRow["ACM_01"])));
                    ammoDictionary.Add("equipment_extra_cpu_ish-01",
                        new Ammunition(player, "equipment_extra_cpu_ish-01", 100));
                    ammoDictionary.Add("ammunition_mine_smb-01", new Ammunition(player, "ammunition_mine_smb-01", 100));
                    ammoDictionary.Add("ammunition_specialammo_emp-01",
                        new Ammunition(player, "ammunition_specialammo_emp-01", 100));

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading ammo, " + e.Message);
            }
            return ammoDictionary;
        }

        public Statistics LoadStatistics(Player player)
        {
            var statistics = new Statistics(player);
            return statistics;
        }

        public BaseInfo LoadInfo(Player player, BaseInfo baseInfo)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var getName = baseInfo.SqlName;
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + getName + " FROM player_data WHERE PLAYER_ID=" +
                                                    player.Id);
                    baseInfo.SyncedValue = doubleConv(queryRow[getName].ToString());
                    baseInfo.LastTimeSynced = DateTime.Now;

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading info, " + e.Message);
            }
            return baseInfo;
        }

        public int LoadInfo(Player player, string row)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + row + " FROM player_data WHERE PLAYER_ID=" + player.Id);

                    return intConv(queryRow[row]);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading info, " + e.Message);
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
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_data SET {getName}={getName}+{amount_change} WHERE PLAYER_ID={player.Id}");
                    baseInfo.SyncedValue = baseInfo.SyncedValue + amount_change;
                    baseInfo.LastTimeSynced = DateTime.Now;

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error updating info, " + e.Message);
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
                Debug.WriteLine("Error updating info, " + e.Message);
            }
        }

        public void UpdateInfoBulk(Player player, double creChange, double uriChange, double expChange, double honChange)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteNonQuery($"UPDATE player_data SET CREDITS=CREDITS+{creChange}, URIDIUM=URIDIUM+{uriChange},LVL={player.Information.Level.Id}, EXP=EXP+{expChange}, HONOR=HONOR+{honChange} WHERE" +
                                                               $" PLAYER_ID={player.Id}");

                    var row = mySqlClient.ExecuteQueryRow("SELECT CREDITS,URIDIUM,EXP,HONOR FROM player_data WHERE PLAYER_ID=" + player.Id);

                    player.Information.Credits.SyncedValue = doubleConv(row["CREDITS"]);
                    player.Information.Credits.Value = player.Information.Credits.SyncedValue;
                    player.Information.Credits.LastTimeSynced = DateTime.Now;

                    player.Information.Uridium.SyncedValue = doubleConv(row["URIDIUM"]);
                    player.Information.Uridium.Value = player.Information.Uridium.SyncedValue;
                    player.Information.Uridium.LastTimeSynced = DateTime.Now;

                    player.Information.Experience.SyncedValue = doubleConv(row["EXP"]);
                    player.Information.Experience.Value = player.Information.Experience.SyncedValue;
                    player.Information.Experience.LastTimeSynced = DateTime.Now;

                    player.Information.Honor.SyncedValue = doubleConv(row["HONOR"]);
                    player.Information.Honor.Value = player.Information.Honor.SyncedValue;
                    player.Information.Honor.LastTimeSynced = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error updating info, " + e.Message);
            }
        }

        public void SetInfo(Player player, BaseInfo baseInfo, double new_amount)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var getName = baseInfo.GetType().Name?.ToUpper();
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_data SET {getName}={new_amount} WHERE PLAYER_ID={player.Id}");
                    baseInfo.SyncedValue = new_amount;
                    baseInfo.LastTimeSynced = DateTime.Now;

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error setting info, " + e.Message);
            }
        }

        public void SetInfo(Player player, string row, double new_amount)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_data SET {row}={new_amount} WHERE PLAYER_ID={player.Id}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error setting info, " + e.Message);
            }
        }

        public int UpdateAmmo(Ammunition ammunition, int ammo_change)
        {
            try
            {
                var row = AmmoConverter.AmmoToDbString(ammunition.LootId);
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_ammo SET {row}={row}-{ammo_change} WHERE PLAYER_ID={ammunition.Player.Id}");
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + row + " FROM player_ammo WHERE PLAYER_ID=" +
                                                    ammunition.Player.Id);

                    return intConv(queryRow[row]);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error updating ammo, " + e.Message);
            }
            return -1;
        }

        public void PerformFullRefresh(Information info)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryReader("SELECT CREDITS,URIDIUM,EXP,HONOR,LVL,PREMIUM_UNTIL FROM player_data WHERE PLAYER_ID=" + info.Player.Id);

                    while (queryRow.Read())
                    {
                        var cre = doubleConv(queryRow["CREDITS"]);
                        var uri = doubleConv(queryRow["URIDIUM"]);
                        var exp = doubleConv(queryRow["EXP"]);
                        var hon = doubleConv(queryRow["HONOR"]);
                        var premEnd = Convert.ToDateTime(queryRow["PREMIUM_UNTIL"]);
                        info.Credits.Sync(cre);
                        info.Uridium.Sync(uri);
                        info.Experience.Sync(exp);
                        info.Honor.Sync(hon);
                        info.Premium.Sync(premEnd);
                    }
                    queryRow.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error refreshing user infos, " + e.Message);
            }
            LoadExtraData(info.Player, info);
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
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + settingsVersion +
                                                    " FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    if (player.UsingNewClient)
                    {
                        userSettings =
                            JsonConvert.DeserializeObject<netty.commands.new_client.UserSettingsCommand>(
                                queryRow[settingsVersion].ToString());
                    }
                    else
                    {
                        userSettings =
                            JsonConvert.DeserializeObject<netty.commands.old_client.UserSettingsCommand>(
                                queryRow[settingsVersion].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error setting gameplay settings, " + e.Message);
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
                        query =
                            $"UPDATE player_extra_data SET SETTINGS_GAMEPLAY_OLD='{JsonConvert.SerializeObject(settings.OldClientUserSettingsCommand)}' WHERE PLAYER_ID={settings.Player.Id}";
                    }
                    mySqlClient.ExecuteNonQuery(query);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error saving gameplay settings, " + e.Message);
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
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + settingsVersion +
                                                    " FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    if (player.UsingNewClient)
                    {
                        //userSettings = JsonConvert.DeserializeObject<netty.commands.new_client.ShipSettingsCommand>(queryRow[settingsVersion].ToString());
                    }
                    else
                    {
                        userSettings =
                            JsonConvert.DeserializeObject<netty.commands.old_client.ShipSettingsCommand>(
                                queryRow[settingsVersion].ToString());
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
                        query =
                            $"UPDATE player_extra_data SET SETTINGS_SLOTBAR_OLD='{JsonConvert.SerializeObject(settings.OldClientShipSettingsCommand)}' WHERE PLAYER_ID={settings.Player.Id}";
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

        public void SavePlayerHangar(Player player,Hangar hangar)
        {
            try
            {
                var activeHangarId = player.Hangar.Id;
                if (player.Spacemap != null && player.Position != null)
                {
                    using (var mySqlClient = SqlDatabaseManager.GetClient())
                    {
                        mySqlClient.ExecuteNonQuery(
                            $"UPDATE player_hangar SET ACTIVE=0 WHERE PLAYER_ID={player.Id}");
                        mySqlClient.ExecuteNonQuery(
                            $"UPDATE player_hangar SET ACTIVE=1 WHERE ID={activeHangarId} AND PLAYER_ID={player.Id}");

                        mySqlClient.ExecuteNonQuery(
                            $"UPDATE player_hangar SET SHIP_MAP_ID={player.Spacemap.Id}, SHIP_HP={player.CurrentHealth}, SHIP_NANO={player.CurrentNanoHull}, SHIP_X={player.Position.X}, SHIP_Y={player.Position.Y} WHERE PLAYER_ID={player.Id} AND ID={hangar.Id}");
                        player.Storage.DistancePassed = 0;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed saving pos");
            }
        }

        public void LoadSkilltree(Player player, Skilltree skilltree)
        {
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
        }

        public Premium LoadPremium(Player player)
        {
            Premium premium = new Premium();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow($"SELECT PREMIUM_UNTIL FROM player_data WHERE PLAYER_ID={player.Id}");
                    var premiumExpiry = Convert.ToDateTime(queryRow["PREMIUM_UNTIL"]);
                    premium.ExpiryDate = premiumExpiry;
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
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_extra_data SET ASSETS_VERSION={settings.ASSET_VERSION} WHERE PLAYER_ID={settings.Player.Id}");
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
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_drones SET EXPERIENCE={drone.Experience}, LEVEL={drone.Level.Id} WHERE ID={drone.Id}");
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
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_ship_config SET CONFIG_1_SHIELD_LEFT={player.Hangar.Configurations[0].CurrentShieldLeft}, CONFIG_2_SHIELD_LEFT={player.Hangar.Configurations[1].CurrentShieldLeft} " +
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
                    var queryRow =
                        mySqlClient.ExecuteQueryRow(
                            $"SELECT ASSETS_VERSION FROM player_extra_data WHERE PLAYER_ID={player.Id}");
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
                    mySqlClient.ExecuteNonQuery(
                        $"INSERT INTO player_event_info (PLAYER_ID, EVENT_ID, SCORE, DATA) VALUES('{playerEvent.Player.Id}', '{playerEvent.Id}', '{playerEvent.Score}', '{JsonConvert.SerializeObject(playerEvent)}') ON DUPLICATE KEY UPDATE SCORE='{playerEvent.Score}', DATA='{JsonConvert.SerializeObject(playerEvent)}'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoadEventForPlayer(int id, Player player)
        {
            var eventInfo = World.StorageManager.Events[id];
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var query = mySqlClient.ExecuteQueryRow(
                        $"SELECT * FROM player_event_info WHERE EVENT_ID={id} AND PLAYER_ID={player.Id}");

                    if (query != null)
                    {
                        var eventData = query["DATA"].ToString();

                        PlayerEvent playerEventData = null;
                        var eventType = eventInfo.EventType;
                        switch (eventType)
                        {
                            case EventTypes.SCOREMAGEDDON:
                                if (eventData == "")
                                {
                                    playerEventData = new ScoreMageddon(player, id);
                                    break;
                                }
                                var data = JsonConvert.DeserializeObject<ScoreMageddon>(eventData);
                                playerEventData = new ScoreMageddon(player, data);
                                break;
                            default: return;
                        }

                        if (!player.EventsPraticipating.ContainsKey(id))
                            player.EventsPraticipating.TryAdd(id, playerEventData);
                        else player.EventsPraticipating[id] = playerEventData;
                    }
                    else if (eventInfo.Open) eventInfo.CreatePlayerEvent(player);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading event for player, " + e.Message + " [" + player.Id + "]");
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
                Debug.WriteLine("Error updating server event, " + e.Message);
            }
        }

        public Killscreen GetLastKillscreen(Player player)
        {
            Killscreen killscreen = new Killscreen();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow(
                        $"SELECT * FROM player_deaths WHERE PLAYER_ID={player.Id} ORDER BY id DESC LIMIT 1");
                    var id = Convert.ToInt32(queryRow["ID"]);
                    var killerName = queryRow["KILLER_NAME"].ToString();
                    var killerLink = queryRow["KILLER_LINK"].ToString();
                    var deathType = (DeathType) (Convert.ToInt32(queryRow["DEATH_TYPE"]));
                    var alias = queryRow["ALIAS"].ToString();
                    var tod = Convert.ToDateTime(queryRow["TIME_OF_DEATH"]);

                    killscreen =  new Killscreen()
                    {
                        Id = id,
                        KilledPlayer = player,
                        KillerName = killerName,
                        KillerLink = killerLink,
                        DeathType = deathType,
                        TimeOfDeath = tod
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return killscreen;
        }

        public void AddKillScreen(Killscreen killscreen)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"INSERT INTO player_deaths (PLAYER_ID, KILLER_NAME, KILLER_LINK, DEATH_TYPE, ALIAS, TIME_OF_DEATH) VALUES('{killscreen.KilledPlayer.Id}', '{killscreen.KillerName}', '{killscreen.KillerLink}', '{(int) killscreen.DeathType}', '{killscreen.Alias}', '{killscreen.TimeOfDeath.ToString("yyyy-MM-dd H:mm:ss")}')");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Title LoadTitle(Player player)
        {
            Title title = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT TITLE_ID FROM player_data WHERE PLAYER_ID=" + player.Id);
                    var titleId = intConv(queryRow["TITLE_ID"]);
                    if (World.StorageManager.Titles.ContainsKey(titleId))
                        title = World.StorageManager.Titles[titleId];
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading title, " + e.Message);
            }
            return title;
        }

        public void Refresh(Player player)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public ConcurrentDictionary<int, QuestSerializableState> LoadPlayerQuestData(Player player)
        {
            ConcurrentDictionary<int, QuestSerializableState> data = new ConcurrentDictionary<int, QuestSerializableState>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_quests WHERE PLAYER_ID=" + player.Id);
                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var conditionId = intConv(row["CONDITION_ID"]);
                            var stateString = row["STATE"].ToString();
                            var state = JsonConvert.DeserializeObject<QuestSerializableState>(stateString);
                            if (state == null)
                            {
                                var questId = intConv(row["QUEST_ID"]);
                                state = new QuestSerializableState { QuestId = questId, ConditionId = conditionId };
                            }
                            else data.TryAdd(conditionId, state);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading player quests, " + e.Message + " [" + player.Id + "]");
            }

            return data;
        }

        public void Reset(GameSession session, string query)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow = mySqlClient.ExecuteQueryRow(query);
                    Packet.Builder.LegacyModule(session, "0|A|STD|" + JsonConvert.SerializeObject(queryRow.ItemArray));
                }
            }
            catch (Exception e)
            {
                Packet.Builder.LegacyModule(session, "0|A|STD|" + e.Message);
            }
        }

        public void SaveCargo(Player player, Cargo cargo)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_extra_data SET CARGO='{JsonConvert.SerializeObject(cargo.GetOreArray())}' WHERE PLAYER_ID=" +
                        player.Id);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Cargo LoadCargo(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT CARGO FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    var cargoJson = queryRow["CARGO"].ToString();
                    var cargo = JsonConvert.DeserializeObject<int[]>(cargoJson);
                    if (cargo != null)
                    {
                        return new Cargo(player, cargo[0], cargo[1], cargo[2], cargo[3], cargo[4], cargo[5], cargo[6], cargo[7], cargo[8]);
                    }
                    return new Cargo(player, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading player cargo, " + e.Message);
            }

            return new Cargo(player, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        public Skylab LoadSkylab(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT SKYLAB FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    var skylabJson = queryRow["SKYLAB"].ToString();
                    var skylab = JsonConvert.DeserializeObject<Skylab>(skylabJson);
                    if (skylab != null)
                    {
                        skylab.Player = player;
                    }
                    else return new Skylab(player);

                    return skylab;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        public void SaveSkylab(Player player, Skylab skylab)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_extra_data SET SKYLAB='{JsonConvert.SerializeObject(skylab)}' WHERE PLAYER_ID=" +
                        player.Id);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Pet LoadPet(Player player)
        {
            Pet pet = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var reader = mySqlClient.ExecuteQueryReader("SELECT * FROM player_pet, player_pet_config WHERE player_pet.PLAYER_ID=player_pet_config.PLAYER_ID AND player_pet.PLAYER_ID=" + player.Id);
                    while (reader.Read())
                    {
                        var id = intConv(reader["ID"]);
                        var name = reader["NAME"].ToString();
                        var level = intConv(reader["LEVEL"]);
                        var exp = doubleConv(reader["EXPERIENCE"]);
                        var hp = intConv(reader["HP"]);
                        var fuel = intConv(reader["FUEL"]);

                        var petShip = Pet.GetShipByLevel(level);

                        pet = new Pet(id , player, name, new Hangar(0, petShip, new Dictionary<int, Drone>(), player.Position, player.Spacemap, hp, 0, true) { Configurations = player.Equipment.PetConfigParser()}, player.FactionId, World.StorageManager.Levels.PetLevels[level], exp, fuel);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return pet;
        }

        public Dictionary<int, int> LoadStats(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var reader = mySqlClient.ExecuteQueryReader("SELECT STATS FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    while (reader.Read())
                    {
                        var statsString = reader["STATS"].ToString();
                        Dictionary<int, int> stats = JsonConvert.DeserializeObject<Dictionary<int, int>>(statsString);
                        if (stats == null || statsString == "null")
                        {
                            stats = new Dictionary<int, int>();
                        }

                        return stats;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error loading stats, " + e.Message);
            }

            return null;
        }

        public void SaveStats(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"UPDATE player_extra_data SET STATS='{JsonConvert.SerializeObject(player.Information.KilledShips)}' WHERE PLAYER_ID=" +
                        player.Id);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void LoadExtraData(Player player, Information information)
        {
            information.Vouchers = 0;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var row =
                        mySqlClient.ExecuteQueryRow("SELECT * FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    var bkJson = row["BOOTY_KEYS"].ToString();
                    if (bkJson == "") bkJson = "[0,0,0]";
                    int[] bks = JsonConvert.DeserializeObject<int[]>(bkJson);
                    information.BootyKeys = bks;
                    var ggSpins = intConv(row["GG_ENERGY"]);
                    information.GGSpins = ggSpins;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void RepairPet(Pet pet)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_pet SET HP='{pet.CurrentHealth}' WHERE ID='{pet.DbId}'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void SavePet(Pet pet)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_pet SET HP='{pet.CurrentHealth}', PET_TYPE='{Pet.GetShipByLevel(pet.Level.Id).Id}', LEVEL='{pet.Level.Id}', EXPERIENCE='{pet.Experience}', FUEL='{pet.Fuel}' WHERE ID='{pet.DbId}'");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void RemovePlayerQuest(Player player, int questId)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery("DELETE FROM player_quests WHERE PLAYER_ID=" + player.Id + " AND QUEST_ID=" + questId);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error removing quest, " + e.Message);
            }
        }

        public void AddQuestCondition(Player player, QuestSerializableState state)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"INSERT INTO player_quests (PLAYER_ID, QUEST_ID, CONDITION_ID, STATE) VALUES ('{player.Id}', '{state.QuestId}', '{state.ConditionId}' ,'{JsonConvert.SerializeObject(state)}')");
                }
            }
            catch (Exception)
            {

            }
        }

        public void UpdateQuestCondition(Player player, QuestSerializableState state)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_quests SET STATE='{JsonConvert.SerializeObject(state)}' WHERE PLAYER_ID='{player.Id}' AND CONDITION_ID='{state.ConditionId}'");
                }
            }
            catch (Exception)
            {

            }
        }

        public Dictionary<int, Module> LoadPlayerModules(Player player)
        {
            var modules = new Dictionary<int, Module>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable =
                        mySqlClient.ExecuteQueryTable("SELECT * FROM player_modules WHERE PLAYER_ID=" + player.Id);
                    if (queryTable != null)
                    {
                        foreach (DataRow row in queryTable.Rows)
                        {
                            var id = intConv(row["ID"]);
                            var typeId = intConv(row["TYPE"]);
                            var equipped = Convert.ToBoolean(intConv(row["EQUIPPED"]));
                            var cbsId = intConv(row["CBS_ID"]);
                            var pos = intConv(row["POSITION"]);
                            var hp = intConv(row["HP"]);
                            var shd = intConv(row["SHIELD"]);
                            var upLevel = intConv(row["UPGRADE_LVL"]);

                            if (!equipped)
                            {
                                //var module = new Module((Module.Types)typeId, new Item(id, "", 1), false);
                                //modules.Add(id, module);
                            }
                            else
                            {
                                if (World.StorageManager.Asteroids.ContainsKey(cbsId))
                                {
                                    foreach (var equippedModule in World.StorageManager.Asteroids[cbsId]
                                        .EquippedModules)
                                    {
                                        Console.WriteLine("gay module !! "+ equippedModule.Key);
                                    }
                                    //var module = World.StorageManager.Asteroids[cbsId].EquippedModules[id];
                                    //if (module.Owner == player)
                                    //{
                                    //    Console.WriteLine("we got a match");
                                    //}else Console.WriteLine("fok");

                                    //var gaymodule = new Module((Module.Types)typeId, new Item(id, "", 1), true);
                                    //modules.Add(id, gaymodule);
                                }
                                else
                                {
                                    //todo
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("bruhhh");
                Console.WriteLine(e.Message);
            }

            return modules;
        }

        public List<DroneFormation> LoadDroneFormations(Player player)
        {
            var droneFormationsList = new List<DroneFormation> {DroneFormation.STANDARD};
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT DRONE_FORMATIONS FROM player_extra_data WHERE PLAYER_ID=" +
                                                    player.Id);
                    var droneFormationsJson = queryRow["DRONE_FORMATIONS"].ToString();
                    if (droneFormationsJson == "") return droneFormationsList;
                    droneFormationsJson = droneFormationsJson.Replace("{\"ID\":", "").Replace("}", "");
                    var formations = JsonConvert.DeserializeObject<int[]>(droneFormationsJson);
                    foreach (var formation in formations)
                    {
                        switch (formation)
                        {
                            case 40:
                                droneFormationsList.Add(DroneFormation.TURTLE);
                                break;
                            case 41:
                                droneFormationsList.Add(DroneFormation.ARROW);
                                break;
                            case 42:
                                droneFormationsList.Add(DroneFormation.LANCE);
                                break;
                            case 43:
                                droneFormationsList.Add(DroneFormation.STAR);
                                break;
                            case 44:
                                droneFormationsList.Add(DroneFormation.PINCER);
                                break;
                            case 45:
                                droneFormationsList.Add(DroneFormation.DOUBLE_ARROW);
                                break;
                            case 46:
                                droneFormationsList.Add(DroneFormation.DIAMOND);
                                break;
                            case 47:
                                droneFormationsList.Add(DroneFormation.CHEVRON);
                                break;
                            case 48:
                                droneFormationsList.Add(DroneFormation.MOTH);
                                break;
                            case 49:
                                droneFormationsList.Add(DroneFormation.CRAB);
                                break;
                            case 50:
                                droneFormationsList.Add(DroneFormation.HEART);
                                break;
                            case 51:
                                droneFormationsList.Add(DroneFormation.BARRAGE);
                                break;
                            case 52:
                                droneFormationsList.Add(DroneFormation.BAT);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return droneFormationsList;
        }

        public ConcurrentDictionary<int, Booster> LoadBoosters(Player player)
        {
            var boosters = new ConcurrentDictionary<int, Booster>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryRow =
                        mySqlClient.ExecuteQueryTable("SELECT * FROM player_boosters WHERE PLAYER_ID=" +
                                                    player.Id);
                    foreach (DataRow reader in queryRow.Rows)
                    {
                        var id = intConv(reader["ID"]);
                        var boosterId = intConv(reader["BOOSTER_ID"]);
                        var endTime = DateTime.Parse(reader["END_TIME"].ToString());
                        
                        switch (boosterId)
                        {
                            case 33:
                                boosters.TryAdd(id, new DMGB01(id, player, endTime));
                                break;
                            case 34:
                                boosters.TryAdd(id, new EPB01(id, player, endTime));
                                break;
                            case 35:
                                boosters.TryAdd(id, new HONB01(id, player, endTime));
                                break;
                            case 36:
                                boosters.TryAdd(id, new HPB01(id, player, endTime));
                                break;
                            case 37:
                                boosters.TryAdd(id, new REPB01(id, player, endTime));
                                break;
                            case 38:
                                boosters.TryAdd(id, new RESB01(id, player, endTime));
                                break;
                            case 39:
                                boosters.TryAdd(id, new SHDB01(id, player, endTime));
                                break;
                            case 102:
                                boosters.TryAdd(id, new QR01(id, player, endTime));
                                break;
                            case 103:
                                boosters.TryAdd(id, new BB01(id, player, endTime));
                                break;
                            case 104:
                                boosters.TryAdd(id, new HON50(id, player, endTime));
                                break;
                            case 105:
                                boosters.TryAdd(id, new EP50(id, player, endTime));
                                break;
                            case 115:
                                boosters.TryAdd(id, new DMGB02(id, player, endTime));
                                break;
                            case 116:
                                boosters.TryAdd(id, new EPB02(id, player, endTime));
                                break;
                            case 117:
                                boosters.TryAdd(id, new HONB02(id, player, endTime));
                                break;
                            case 118:
                                boosters.TryAdd(id, new HPB02(id, player, endTime));
                                break;
                            case 119:
                                boosters.TryAdd(id, new REPB02(id, player, endTime));
                                break;
                            case 120:
                                boosters.TryAdd(id, new RESB02(id, player, endTime));
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return boosters;
        }

        public EquipmentItem AddEquipmentItem(Player player, Item item)
        {
            EquipmentItem eqItem = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var reader = mySqlClient.ExecuteQueryReader("INSERT INTO player_equipment (USER_ID, PLAYER_ID, ITEM_ID, ITEM_TYPE, ITEM_LVL, ON_CONFIG_1, ON_CONFIG_2, ON_DRONE_ID_1, ON_DRONE_ID_2, ON_PET_1, ON_PET_2, ITEM_AMOUNT) VALUES ('" + player.GlobalId + "', '" + player.Id + "', '" + item.Id + "', '" + item.TypeId + "', '1', '{ \"hangars\" : [] }', '{ \"hangars\" : [] }', '{ \"hangars\" : [],\"droneID\":[] }', '{ \"hangars\" : [],\"droneID\":[] }', '{ \"hangars\" : [] }', '{ \"hangars\" : [] }', '0')");
                    while (reader.Read())
                    {
                        var id = intConv(reader["ID"]);
                        eqItem = new EquipmentItem(id, player, item, 1, new EquippedItem(), new EquippedItem(),
                            new EquippedItem(), new EquippedItem(), new EquippedItem(), new EquippedItem(), 1);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
            if (eqItem != null) player.Equipment.EquipmentItems.Add(eqItem.Id, eqItem);
            return eqItem;
        }

        public void UpdateEquipmentItem(EquipmentItem equipmentItem)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_equipment SET ITEM_LVL='{equipmentItem.Level}', ITEM_AMOUNT='{equipmentItem.ItemAmount}' WHERE ID=" + equipmentItem.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void DeleteEquipmentItem(EquipmentItem equipmentItem)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery("DELETE FROM player_equipment WHERE ID=" + equipmentItem.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void AddGGEnergy(Player player, int amount)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery("UPDATE player_extra_data SET GG_ENERGY=GG_ENERGY+" + amount +
                                                " WHERE PLAYER_ID=" + player.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void UpdateBootyKeys(Player player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery("UPDATE player_extra_data SET BOOTY_KEYS='" + JsonConvert.SerializeObject(player.Information.BootyKeys) + "' WHERE PLAYER_ID=" + player.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }

        public object GetPlayerKeySettings(Player player)
        {
            object keySettings = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var settingsVersion = "SETTINGS_KEYS_OLD";
                    var queryRow =
                        mySqlClient.ExecuteQueryRow("SELECT " + settingsVersion +
                                                    " FROM player_extra_data WHERE PLAYER_ID=" + player.Id);
                    if (player.UsingNewClient)
                    {
                    }
                    else
                    {
                        keySettings =
                            JsonConvert.DeserializeObject<netty.commands.old_client.UserKeyBindingsUpdate>(
                                queryRow[settingsVersion].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error " + e);
            }
            return keySettings;
        }

        public void SavePlayerKeySettings(Settings settings)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    string query = "";
                    if (settings.Player.UsingNewClient)
                    {
                        //TODO Save player ship settings for new client
                        Console.WriteLine("TODO Save player key settings for new client");
                        //throw new NotImplementedException();
                    }
                    else
                    {
                        query =
                            $"UPDATE player_extra_data SET SETTINGS_KEYS_OLD='{JsonConvert.SerializeObject(settings.OldClientKeyBindingsCommand)}' WHERE PLAYER_ID={settings.Player.Id}";
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

        public void LoadPlayerGates(PlayerGates gates)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var row = mySqlClient.ExecuteQueryRow("SELECT * FROM player_galaxy_gates WHERE PLAYER_ID=" + gates.Player.Id);
                    var completedGates = JsonConvert.DeserializeObject<int[]>(row["COMPLETED_GATES"].ToString());
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
                    var alphaPrepared = Convert.ToBoolean(intConv(row["ALPHA_PREPARED"]));
                    gates.AlphaReady = alphaPrepared;
                    var alphaWave = intConv(row["ALPHA_WAVE"]);
                    gates.AlphaWave = alphaWave;
                    var alphaLives = intConv(row["ALPHA_LIVES"]);
                    gates.AlphaLives = alphaLives;

                    // BETA Gate
                    var betaPrepared = Convert.ToBoolean(intConv(row["BETA_PREPARED"]));
                    gates.BetaReady = betaPrepared;
                    var betaWave = intConv(row["BETA_WAVE"]);
                    gates.BetaWave = betaWave;
                    var betaLives = intConv(row["BETA_LIVES"]);
                    gates.BetaLives = betaLives;

                    // GAMMA Gate
                    var gammaPrepared = Convert.ToBoolean(intConv(row["GAMMA_PREPARED"]));
                    gates.GammaReady = gammaPrepared;
                    var gammaWave = intConv(row["GAMMA_WAVE"]);
                    gates.GammaWave = gammaWave;
                    var gammaLives = intConv(row["GAMMA_LIVES"]);
                    gates.GammaLives = gammaLives;

                    var deltaPrepared = Convert.ToBoolean(intConv(row["DELTA_PREPARED"]));
                    gates.DeltaReady = deltaPrepared;

                    var epsilonPrepared = Convert.ToBoolean(intConv(row["EPSILON_PREPARED"]));
                    gates.EpsilonReady = epsilonPrepared;

                    var zetaPrepared = Convert.ToBoolean(intConv(row["ZETA_PREPARED"]));
                    gates.ZetaReady = zetaPrepared;

                    var kappaPrepared = Convert.ToBoolean(intConv(row["KAPPA_PREPARED"]));
                    gates.KappaReady = kappaPrepared;

                    var lambdaPrepared = Convert.ToBoolean(intConv(row["LAMBDA_PREPARED"]));
                    gates.LambdaReady = lambdaPrepared;

                }
            }
            catch (Exception e)
            {
                
            }
        }

        public void SaveGalaxyGates(PlayerGates gates)
        {
            try
            {
                using (var mysqlClient = SqlDatabaseManager.GetClient())
                {
                    var completedGates = new int[]
                    {
                        gates.AlphaComplete, gates.BetaComplete, gates.GammaComplete,
                        gates.DeltaComplete, gates.EpsilonComplete, gates.ZetaComplete,
                        gates.KappaComplete, gates.KronosComplete, gates.LambdaComplete,
                        gates.HadesComplete
                    };
                    
                    mysqlClient.ExecuteNonQuery(
                        $"UPDATE player_galaxy_gates SET COMPLETED_GATES='" + JsonConvert.SerializeObject(completedGates) + "'," +
                        $" ALPHA_PREPARED = {gates.AlphaReady}, ALPHA_WAVE = {gates.AlphaWave}, ALPHA_LIVES = {gates.AlphaLives}," +
                        $" BETA_PREPARED = {gates.BetaReady}, BETA_WAVE = {gates.BetaWave}, BETA_LIVES = {gates.BetaLives}," +
                        $" GAMMA_PREPARED = {gates.GammaReady}, GAMMA_WAVE = {gates.GammaWave}, GAMMA_LIVES = {gates.GammaLives}" +
                        $" WHERE PLAYER_ID = {gates.Player.Id}");
                }
            }
            catch (Exception e)
            {
                
            }
        }
        
        public Dictionary<int, GameBan> LoadGameBans(Player player)
        {
            var gameBans = new Dictionary<int, GameBan>();
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var queryTable = mySqlClient.ExecuteQueryTable("SELECT * FROM player_game_bans WHERE PLAYER_ID=" + player.Id);
                    foreach (DataRow row in queryTable.Rows)
                    {
                        var id = intConv(row["ID"]);
                        var issuedTime = DateTime.Parse(row["ISSUED_TIME"].ToString());
                        var reason = row["REASON"].ToString();
                        var expireTime = DateTime.Parse(row["EXPIRE_TIME"].ToString());
                        var issuedBy = intConv(row["ISSUED_BY"]);
                        var gameBan = new GameBan(id, issuedTime, reason, expireTime, issuedBy);
                        gameBans.Add(id, gameBan);
                    }
                }
            }
            catch (Exception)
            {

            }

            return gameBans;
        }

        public void AddToCheatList(Player player)
        {
            
        }

        public void AddPlayerLog(Player player, PlayerLogTypes logType, string log)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery(
                        $"INSERT INTO player_logs (`USER_ID`, `PLAYER_ID`, `LOG_TYPE`, `LOG_DESCRIPTION`, `LOG_DATE`) VALUES('{player.GlobalId}', '{player.Id}', '{(int)logType}', '{log}', '{DateTime.Now:yyyy-MM-dd H:mm:ss}')");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

