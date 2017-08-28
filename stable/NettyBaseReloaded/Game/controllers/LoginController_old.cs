using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.newcommands;
using NettyBaseReloaded.Game.netty.newcommands.slotbarModules;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.assets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.slotbarnew;
using NettyBaseReloaded.Game.objects.world.storages.playerStorages;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.controllers
{
    class LoginController
    {
        public static void Respawn(GameSession gameSession)
        {
            gameSession.Player.CurrentHealth = 1000;
            NormalSpawn(gameSession);
        }

        public static void Login(GameSession gameSession)
        {
            var player = gameSession.Player;
           
            if ((player.Id == 544 || player.Id == 495 || player.Id == 498 || player.Id == 497) || Properties.PUBLIC_BETA_END > DateTime.Now)
            {

                if (!player.Spacemap.Entities.ContainsKey(player.Id))
                {
                    player.Spacemap.Entities.Add(player.Id, player);
                    //Start controller
                    if (player.Controller == null) 
                        player.Controller = new PlayerController(player);
                    player.Controller.StopController = false;
                    player.Controller.Jumping = false;
                    player.AttachedNpcs.Clear();
                }
                else
                {
                    Logger.Logger.WritingManager.Write("Player #" + player.Id +
                                                       " already exists on the Spacemap (LoginManager)");
                }


                if (Properties.SERVER_LOCKED)
                {
                    LockedServerSpawn(gameSession);
                    return;
                }

                if (player.CurrentHealth <= 0)
                {
                    ShowKillscreen(gameSession);
                    return;
                }

                NormalSpawn(gameSession);
            }
        }

        private static bool IsInWhitelist(int id)
        {
            return false;
        }

        private static void UserSettings(GameSession gameSession)
        {
            var client = gameSession.Client;
            var player = gameSession.Player;

            if (!player.UsingNewClient)
            {
                client.Send(ShipSettingsCommand.write(player.Settings.Slotbar.QuickbarSlots,
                    player.Settings.Slotbar.QuickbarSlotsPremium,
                    player.Settings.Slotbar.SelectedLaser, player.Settings.Slotbar.SelectedRocket,
                    player.Settings.Slotbar.SelectedHellstormRocket));

                client.Send(PacketBuilder.UserSettingsCommand(player, player.UsingNewClient));
            }
            else
            {
                client.Send(PacketBuilder.NewClientWindowsCommand());

                //client.Send(PacketBuilder.UserSettingsCommand(player, player.UsingNewClient));

                //SendSlotbars(gameSession);
            }
            
            client.Send(PacketBuilder.ShipInitializationCommand(player));

            client.Send(PacketBuilder.DronesCommand(player, player.UsingNewClient).Bytes);
        }

        private static void LockedServerSpawn(GameSession gameSession)
        {
            var client = gameSession.Client;

            UserSettings(gameSession);

            client.Send(CameraLockToCoordinatesCommand.write(0, 0, 1));

            client.Send(PacketBuilder.LegacyModule("0|A|STD|SERVER IS LOCKED").Bytes);

        }

        private static void LoadConfigurations(GameSession gameSession)
        {
            var player = gameSession.Player;

            World.DatabaseManager.BasicRefresh(player);
            player.Hangar = World.DatabaseManager.LoadHangar(player);
            player.Hangar.Configurations = World.DatabaseManager.LoadConfig(player);
            player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
            //player.CurrentAmmo = World.DatabaseManager.LoadAmmo(player);

        }

        private static void NormalSpawn(GameSession gameSession)
        {
            MapChecker(gameSession);
            LoadConfigurations(gameSession);
            UserSettings(gameSession);

            InitiateLegacy(gameSession);
            InitiateHotkeys(gameSession);
            InitiateDroneFormations(gameSession);
            InitiatePet(gameSession);

            ShipFx(gameSession);
            LoadPortals(gameSession);
            LoadAssets(gameSession);
            LoadStations(gameSession);
            
            LoadPlayerAmmo(gameSession);

            LoadTicks(gameSession);

            Logger.Logger.WritingManager.Write("User logged in [ID: " + gameSession.Player.Id + ", Name: " + gameSession.Player.Name + "]");
        }

        private static void LoadTicks(GameSession gameSession)
        {
            var player = gameSession.Player;

            if (!Global.TickManager.Exists(player))
                Global.TickManager.Tickables.Add(player);

            if (!Global.TickManager.Exists(player.Controller))
                Global.TickManager.Tickables.Add(player.Controller);

            player.Controller.Start();
        }

        private static void ShipFx(GameSession gameSession)
        {
            //var client = gameSession.Client;
            //var player = gameSession.Player;

            //switch (player.Hangar.Ship.Id)
            //{
            //    case 9:
            //        client.Send(PacketBuilder.LegacyModule("0|n|fx|||"));
            //        break;
            //}
        }

        private static void MapChecker(GameSession gameSession)
        {
            if (gameSession.Player.Spacemap.Id == 0)
            {
                switch (gameSession.Player.FactionId)
                {
                    case Faction.MMO:
                        gameSession.Player.Spacemap = World.StorageManager.Spacemaps[1];
                        gameSession.Player.Position = new Vector(1000, 1000);
                        break;
                    case Faction.EIC:
                        gameSession.Player.Spacemap = World.StorageManager.Spacemaps[5];
                        gameSession.Player.Position = new Vector(19800, 1000);
                        break;
                    case Faction.VRU:
                        gameSession.Player.Spacemap = World.StorageManager.Spacemaps[9];
                        gameSession.Player.Position = new Vector(19800, 11800);
                        break;

                }
            }
        }

        private static void InitiateLegacy(GameSession gameSession)
        {
            var player = gameSession.Player;
            var client = gameSession.Client;

            if (player.UsingNewClient)
            {
                client.Send(PacketBuilder.BigMessage("beta_splash_text", true).Bytes);
            }
            else
            {
                client.Send(CameraLockToHeroCommand.write());
                client.Send(
                    PacketBuilder.LegacyModule("0|A|" + ServerCommands.SERVER_VERSION + "|3.0.1 / Bug-less").Bytes);
                client.Send(PacketBuilder.BigMessage("beta_splash_text").Bytes);

                client.Send(PacketBuilder.LegacyModule("0|A|ITM|" + player.GetConsumablesPacket()).Bytes);
                client.Send(PacketBuilder.LegacyModule("0|A|BK|" + player.BootyKeys[0]).Bytes); //green booty
                client.Send(PacketBuilder.LegacyModule("0|A|BKR|" + player.BootyKeys[1]).Bytes); //red booty
                client.Send(PacketBuilder.LegacyModule("0|A|BKB|" + player.BootyKeys[2]).Bytes); //blue booty
                client.Send(PacketBuilder.LegacyModule("0|TR").Bytes);
                client.Send(PacketBuilder.LegacyModule("0|A|CC|" + player.CurrentConfig).Bytes);
                client.Send(PacketBuilder.LegacyModule("0|ps|nüscht|").Bytes);
                client.Send(PacketBuilder.LegacyModule("0|ps|blk|0").Bytes);
                client.Send(PacketBuilder.LegacyModule("0|n|w|-1").Bytes); //enemy warning
                client.Send(
                    PacketBuilder.LegacyModule(
                        "0|g|a|b,1000,1,10000.0,C,2,500.0,U,3,1000.0,U,5,1000.0,U|r,100,1,10000,C,2,50000,C,3,500.0,U,4,700.0,").Bytes);
                //client.Send(PacketBuilder.LegacyModule("0|UI|MM|SM|0|6000|2000|1"));
            }
        }

        private static void InitiateDroneFormations(GameSession gameSession)
        {
            // TEMP
            List<int> formations = new List<int>();
            foreach (DroneFormation droneFormation in Enum.GetValues(typeof(DroneFormation)))
            {
                gameSession.Player.OwnedFormations.Add(droneFormation);
                formations.Add((int)droneFormation);
            }

            gameSession.Client.Send(DroneFormationAvailableFormationsCommand.write(formations));
        }


        public static void InitiateHotkeys(GameSession gameSession)
        {
            var keysModule = new List<UserKeyBindingsModule>();
            var keys = new List<Hotkey>();
            keys.Add(new Hotkey(Hotkey.ACTIVATE_LASER, (int)Keys.ControlKey));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D1, 0));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D2, 1));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D3, 2));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D4, 3));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D5, 4));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D6, 5));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D7, 6));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D8, 7));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D9, 8));
            keys.Add(new Hotkey(Hotkey.QUICKSLOT, (int)Keys.D0, 9));
            keys.Add(new Hotkey(Hotkey.LAUNCH_ROCKET, (int)Keys.Space));
            keys.Add(new Hotkey(Hotkey.JUMP, (int)Keys.J));
            keys.Add(new Hotkey(Hotkey.LOGOUT, (int)Keys.L));
            keys.Add(new Hotkey(Hotkey.TOGGLE_WINDOWS, (int)Keys.H));
            keys.Add(new Hotkey(Hotkey.CHANGE_CONFIG, (int)Keys.C));

            foreach (var key in keys)
            {
                keysModule.Add(key.Object);
            }
            gameSession.Client.Send(UserKeyBindingsUpdate.write(keysModule, false));
        }

        public static void InitiatePet(GameSession gameSession)
        {
            gameSession.Player.Pet = new Pet(gameSession.Player.Id, gameSession.Player.Id, "TretiqKrak", new Hangar(), );
            gameSession.Client.Send(PetInitializationCommand.write(true, true, true));
        }

        /// <summary>
        /// Load all the portals of the map
        /// </summary>
        /// <param name="gameSession"></param>
        private static void LoadPortals(GameSession gameSession)
        {
            foreach (var portal in gameSession.Player.Spacemap.Portals.Values)
            {
                if (gameSession.Player.UsingNewClient)
                {
                    gameSession.Client.Send(new CreatePortalCommand(
                        portal.Id,
                        portal.FactionScrap,
                        portal.Gfx,
                        portal.Position.X,
                        portal.Position.Y,
                        portal.Visible,
                        portal.Working,
                        new List<int>() //idk
                    ).getBytes());
                }
                else
                {
                    gameSession.Client.Send(PacketBuilder.LegacyModule(portal.ToString()).Bytes);
                }
            }
        }

        private static void LoadAssets(GameSession gameSession)
        {
            List<QuestGiverModule> questGivers = new List<QuestGiverModule>();
            foreach (var asset in gameSession.Player.Spacemap.Assets.Values)
            {
                //if (asset is QuestGiver)
                //{
                //    var qg = (QuestGiver)asset;
                //    questGivers.Add(new QuestGiverModule(qg.QuestGiverId, qg.Id));
                //}
                if (!gameSession.Player.UsingNewClient)
                    gameSession.Client.Send(PacketBuilder.CreateAsset(asset));
            }
            if (!gameSession.Player.UsingNewClient)
                gameSession.Client.Send(QuestGiversAvailableCommand.write(questGivers));
        }

        private static void LoadStations(GameSession gameSession)
        {
            foreach (var station in gameSession.Player.Spacemap.Stations)
            {
                //http://pastebin.com/evZkU9Vq
                //    gameSession.Client.Send(netty.newcommands.AssetCreateCommand);
                if (!gameSession.Player.UsingNewClient)
                    gameSession.Client.Send(PacketBuilder.LegacyModule(station.GetString()).Bytes);
            }
        }

        private static void ShowKillscreen(GameSession gameSession)
        {
            gameSession.Player.Spacemap = World.StorageManager.Spacemaps[0];
            gameSession.Player.CurrentHealth = 1000;

            if (gameSession.Player.Controller != null)
                gameSession.Player.Controller.Dead = false;

            NormalSpawn(gameSession);
        }

        private static void LoadPlayerAmmo(GameSession gameSession)
        {
            gameSession.Player.CurrentAmmo = new Ammo(999,999,999,999,999, 0,999,0,999,999,999,999,999,0);
            if (!gameSession.Player.UsingNewClient)
                gameSession.Client.Send(gameSession.Player.CurrentAmmo.GetPacket());
        }

        public static void SendSlotbars(GameSession gameSession)
        {
            var player = gameSession.Player;
            var gameHandler = gameSession.Client;

            var counterValue = 0;

            var categories = new List<SlotbarCategoryModule>();
            var slotbarItemsTest = new List<SlotbarQuickslotItem>
            {
                new SlotbarQuickslotItem(1, "ammunition_laser_ucb-100"),
                new SlotbarQuickslotItem(2, "ammunition_laser_rsb-75"),
                new SlotbarQuickslotItem(3, "ammunition_laser_mcb-50"),
                new SlotbarQuickslotItem(4, "ammunition_laser_ucb-100"),
                new SlotbarQuickslotItem(5, "ammunition_laser_sab-50"),
                new SlotbarQuickslotItem(6, "ammunition_mine_smb-01"),
                new SlotbarQuickslotItem(7, "equipment_extra_cpu_ish-01"),
                new SlotbarQuickslotItem(8, "ammunition_specialammo_emp-01"),
            };

            var premium = new List<SlotbarQuickslotItem>
            {
                new SlotbarQuickslotItem(1, "drone_formation_f-3d-rg"),
                new SlotbarQuickslotItem(2, "drone_formation_f-10-cr"),
                new SlotbarQuickslotItem(3, "drone_formation_f-09-mo")
            };


            var slotbars = new List<SlotbarQuickslotModule>
            {
                new SlotbarQuickslotModule("standardSlotBar", slotbarItemsTest, "50,85|0,40", "0", true),
                new SlotbarQuickslotModule("premiumSlotBar", premium, "50,85|0,80", "0", true)
            };
            var items = new List<SlotbarCategoryItemModule>();

            //LASERS
            var maxCounter = 1000;
            foreach (var itemId in ItemStorage.LaserIds)
            {
                var item = new LaserItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                try
                {
                    item.CounterValue = player.Hangar.Configurations[player.CurrentConfig].Consumables[itemId].Amount;
                }
                catch (Exception)
                {
                    item.CounterValue = 0;
                }

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("lasers", items));

            //ROCKETS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 200;
            foreach (var itemId in ItemStorage.RocketIds)
            {
                var item = new RocketItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("rockets", items));

            //ROCKET LAUNCHER
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 200;
            foreach (var itemId in ItemStorage.RocketLauncherIds)
            {
                var item = new RocketLauncherItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("rocket_launchers", items));

            //SPECIAL ITEMS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 100;
            foreach (var itemId in ItemStorage.SpecialItemsIds)
            {
                var item = new SpecialItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                try
                {
                    item.CounterValue = player.Hangar.Configurations[player.CurrentConfig].Consumables[itemId].Amount;
                }
                catch (Exception)
                {
                    item.CounterValue = 0;
                }

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("special_items", items));

            //MINES
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 100;
            foreach (var itemId in ItemStorage.MinesIds)
            {
                var item = new MineItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("mines", items));

            //CPUS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in ItemStorage.CpusIds)
            {
                //TODO
            }
            categories.Add(new SlotbarCategoryModule("cpus", items));

            //BUY NOW
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in ItemStorage.BuyNowIds)
            {
                var item = new BuyItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("buy_now", items));

            //TECH ITEMS
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in ItemStorage.TechIds)
            {
                var item = new TechItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("tech_items", items));

            //SHIP ABILITIES
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in ItemStorage.ShipAbilitiesIds)
            {
                var item = new ShipAbilityItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("ship_abilities", items));

            //DRONE FORMATION
            items = new List<SlotbarCategoryItemModule>();
            maxCounter = 0;
            foreach (var itemId in ItemStorage.FormationIds)
            {
                var item = new FormationItem(
                    itemId,
                    counterValue,
                    maxCounter
                    );

                item.Create();
                items.Add(item.Object);
                player.SlotbarItems[item.ClickedId] = item;
            }
            categories.Add(new SlotbarCategoryModule("drone_formations", items));

            gameHandler.Send(new SlotbarsCommand(categories, "50,85", slotbars).getBytes());
        }
    }
}
