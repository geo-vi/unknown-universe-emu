using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.pets;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.controllers
{
    class LoginController
    {
        private GameSession GameSession { get; }
        public LoginController(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        public bool AllowedToEnter()
        {
            var id = GameSession.Player.Id;
            return (id == 544 || id == 495 || id == 498 || id == 497 || id == 697 || id == 5002) ||
                   Properties.Server.PUBLIC_BETA_END > DateTime.Now;
        }

        public void MapChecker()
        {
            var player = GameSession.Player;
            if (Properties.Game.PVP_MODE && player.Spacemap.Id != 16)
            {
                var closestStation = player.GetClosestStation();
                player.Spacemap = closestStation.Item2;
                player.Position = closestStation.Item1;
            }
            if (player.Spacemap.Id == 0)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        player.Spacemap = World.StorageManager.Spacemaps[1];
                        GameSession.Player.Position = new Vector(1000, 1000);
                        break;
                    case Faction.EIC:
                        player.Spacemap = World.StorageManager.Spacemaps[5];
                        player.Position = new Vector(19800, 1000);
                        break;
                    case Faction.VRU:
                        player.Spacemap = World.StorageManager.Spacemaps[9];
                        player.Position = new Vector(19800, 11800);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void LoadConfigurations()
        {
            var player = GameSession.Player;

            World.DatabaseManager.BasicRefresh(player);
            player.Hangar = World.DatabaseManager.LoadHangar(player);
            player.Hangar.Configurations = World.DatabaseManager.LoadConfig(player);
            player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
            //player.CurrentAmmo = World.DatabaseManager.LoadAmmo(player);
            player.UserStorage.State = State.LOADED;
        }

        public void LoadTicks()
        {
            var player = GameSession.Player;

            if (!Global.TickManager.Exists(player))
                Global.TickManager.Tickables.Add(player);

            if (!Global.TickManager.Exists(player.Controller))
                Global.TickManager.Tickables.Add(player.Controller);
        }

        public void SpawnOnMap()
        {
            var player = GameSession.Player;
            if (!player.Spacemap.Entities.ContainsKey(player.Id))
            {
                player.Spacemap.Entities.Add(player.Id, player);

                if (player.Controller == null)
                {
                    player.Controller = new PlayerController(player);
                    player.Controller.Start();
                }

                if (player.Pet != null)
                {
                    if (player.Pet.Controller == null)
                        player.Pet.Controller = new PetController(player.Pet);
                }

                player.Controller.StopController = false;
                player.AttachedNpcs.Clear();
                player.CleanRange();
                player.CleanStorage();
            }
        }

        public void Initiate()
        {
            if (!AllowedToEnter()) return;

            LoadConfigurations();
            MapChecker();
            SpawnOnMap();

            if (GameSession.Player.UsingNewClient)
            {
                if (Properties.Server.LOCKED)
                {
                    NewClient.LockedServerSpawn(GameSession);
                    return;
                }

                if (GameSession.Player.CurrentHealth <= 0)
                {
                    NewClient.SendKillscren(GameSession);
                    return;
                }

                new NewClient(GameSession);
            }
            else
            {
                if (Properties.Server.LOCKED)
                {
                    OldClient.LockedServerSpawn(GameSession);
                    return;
                }

                if (GameSession.Player.CurrentHealth <= 0)
                {
                    OldClient.SendKillscren(GameSession);
                    return;
                }

                new OldClient(GameSession);
            }
            LoadTicks();
            GameSession.Player.UserStorage.State = State.READY;
        }

        class OldClient
        {
            private GameSession _gameSession { get; }

            public OldClient(GameSession gameSession)
            {
                _gameSession = gameSession;
                Settings();
                Spawn();
                Legacy();
            }

            #region Settings
            void Settings()
            {
                Packet.Builder.UserSettingsCommand(_gameSession);
                Packet.Builder.SendUserSettings(_gameSession);
            }
            #endregion
            #region Spawn
            void Spawn()
            {
                Packet.Builder.ShipInitializationCommand(_gameSession);
                Packet.Builder.DronesCommand(_gameSession, _gameSession.Player);

                //LoadPortals();
                //LoadStations();

                Console.WriteLine("Spawning\nShip:{0}, {1}", _gameSession.Player.Id + " " + _gameSession.Player.Name, _gameSession.Player.Spacemap.Id + "|" + _gameSession.Player.Position.ToPacket());
            }

            void LoadPortals()
            {
                foreach (var portal in _gameSession.Player.Spacemap.Objects.Where(x => x.Value is Jumpgate))
                {
                    var value = (Jumpgate)portal.Value;
                    Packet.Builder.JumpgateCreateCommand(_gameSession, value);
                }
            }

            void LoadStations()
            {
                Packet.Builder.LegacyModule(_gameSession, "0|s|69|1|pirateStation|6|1500|20800|12800");

                foreach (var station in _gameSession.Player.Spacemap.Objects.Where(x => x.Value is Station))
                {
                    var value = (Station) station.Value;
                    Packet.Builder.StationCreateCommand(_gameSession, value);
                }
            }
            #endregion
            #region Legacy
            void Legacy()
            {
                //Packet.Builder.PetInitializationCommand(_gameSession, _gameSession.Player.Pet);
                // TODO => fix Packet.Builder.LegacyModule(_gameSession, "0|A|ITM|" + _gameSession.Player.GetConsumablesPacket());
                Packet.Builder.HotkeysCommand(_gameSession);
                Packet.Builder.LegacyModule(_gameSession, "0|A|CC|" + _gameSession.Player.CurrentConfig);
                Packet.Builder.LegacyModule(_gameSession, "0|n|t|" + _gameSession.Player.Id + "|367|most_wanted");
                Packet.Builder.VideoWindowCreateCommand(_gameSession, 1, "c", true, new List<string>{ "login_dialog_1", "login_dialog_2" }, 0, 1);
                Packet.Builder.BeaconCommand(_gameSession);
            }
            #endregion
            #region Static methods
            public static void SendKillscren(GameSession gameSession)
            {
                
            }

            public static void LockedServerSpawn(GameSession gameSession)
            {
                
            }
            #endregion
        }

        class NewClient
        {
            private GameSession _gameSession { get; }

            public NewClient(GameSession gameSession)
            {
                _gameSession = gameSession;
                Settings();
                Spawn();
                Legacy();
            }
            #region Settings
            void Settings()
            {
                Packet.Builder.UserSettingsCommand(_gameSession);
                Packet.Builder.SendUserSettings(_gameSession);
            }
            #endregion
            #region Spawn
            void Spawn()
            {
                Packet.Builder.ShipInitializationCommand(_gameSession);
                Packet.Builder.commandX35(_gameSession);
                Packet.Builder.DronesCommand(_gameSession, _gameSession.Player);
            //    LoadPortals();
            //    LoadStations();
            }

            void LoadPortals()
            {
                foreach (var portal in _gameSession.Player.Spacemap.Objects.Where(x => x.Value is Jumpgate))
                {
                    var value = (Jumpgate) portal.Value;
                    Packet.Builder.JumpgateCreateCommand(_gameSession, value);
                }
            }

            void LoadStations()
            {
                foreach (var station in _gameSession.Player.Spacemap.Objects.Where(x => x.Value is Station))
                {
                    var value = (Station) station.Value;
                    Packet.Builder.StationCreateCommand(_gameSession, value);
                }
            }

            void LoadPOI()
            {

            }
            #endregion
            #region Legacy
            void Legacy()
            {
                //Packet.Builder.PetInitializationCommand(_gameSession, _gameSession.Player.Pet);
                _gameSession.Player.GetConsumablesPacket();
                Packet.Builder.HotkeysCommand(_gameSession);
                Packet.Builder.LegacyModule(_gameSession, "0|n|t|" + _gameSession.Player.Id + "|222|most_wanted");
                Packet.Builder.VideoWindowCreateCommand(_gameSession, 1, "c", true, new List<string>{ "login_dialog_1", "login_dialog_2" }, 0, 1);
                Packet.Builder.BeaconCommand(_gameSession);
                //_gameSession.Client.Send(netty.commands.new_client.commandY2c.write(0, 6, 1500, 20800, 12800));
                //_gameSession.Client.Send(netty.commands.new_client.MapAddPOICommand.write("Middle35",new POITypeModule(5), "", new POIDesignModule(3), 2,new List<int>{18400,11100,19100,11100,19100,12300,18400,12300}, false, true));
            }

            void AssetViewer()
            {
                int id = 505050;
                int posXMax = 38000;
                int posXMin = 24000;
                int posX = posXMin;
                int posY = 22000;
                for (int i = 0; i <= 59; i++)
                {
                    id++;
                    if (posX + 800 > posXMax)
                    {
                        posX = posXMin;
                        posY += 400;
                    }
                    else
                    {
                        posX += 800;
                    }
                    Packet.Builder.AssetCreateCommand(_gameSession, new Asset(id, "ASSET VIEWER", (short)i, 0, "", id, 0, 0,new Vector(posX, posY), 0, false, false, false));
                }
            }

            #endregion
            #region Static methods
            public static void SendKillscren(GameSession gameSession)
            {

            }

            public static void LockedServerSpawn(GameSession gameSession)
            {

            }
            #endregion
        }
    }
}
