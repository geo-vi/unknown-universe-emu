using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.login;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.controllers
{
    class LoginController
    {
        public static void Initiate(GameSession gameSession)
        {
            new LoginController(gameSession);
        }

        public GameSession _gameSession;

        private LoginController(GameSession gameSession)
        {
            _gameSession = gameSession;
            LoadConfigs();
            CheckPos();
            Console.WriteLine("Been through");
            LoadControllers();
            GetLoginType();
            LoadTicks();
        }

        private void LoadConfigs()
        {
            var player = _gameSession.Player;
            
            var config = World.DatabaseManager.LoadConfig(player);;
            player.Hangar.Configurations = config;
            player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
            //player.Pet = new Pet(player.Id, player.Id, "JustAnotherPet", new Hangar(World.StorageManager.Ships[15], new List<Drone>(), player.Position, player.Spacemap, 1000, 0, new Dictionary<string, Item>()), 1000, player.FactionId, new Level(1, 1000), 500, 1000, new List<Gear>());
        }

        private void CheckPos()
        {
            var player = _gameSession.Player;
            if (Properties.Game.PVP_MODE && player.Spacemap.Id != 16)
            {
                var closestStation = player.GetClosestStation();
                player.Spacemap = closestStation.Item2;
                player.Position = closestStation.Item1;
            }
            if (player.Spacemap.Id == 0 || player.Spacemap.Id == 255)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        player.Spacemap = World.StorageManager.Spacemaps[1];
                        player.Position = new Vector(1000, 1000);
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

        private void LoadControllers()
        {
            var player = _gameSession.Player;
            if (player.Controller == null)
            {
                player.Controller = new PlayerController(player);
            }
            if (player.Pet != null && player.Pet.Controller == null)
                player.Pet.Controller = new PetController(player.Pet);

            player.Controller.Setup();
            player.Controller.Initiate();
        }

        private void GetLoginType()
        {
            ILogin type;
            if (Properties.Server.LOCKED)
                type = new Locked(_gameSession);
            else if (_gameSession.Player.CurrentHealth <= 0 || _gameSession.Player.EntityState == EntityStates.DEAD)
                type = new Killscreen(_gameSession);
            else type = new Regular(_gameSession);
            type.Execute();
        }

        private void LoadTicks()
        {
            Global.TickManager.Add(_gameSession.Player);
        }
    }
}
