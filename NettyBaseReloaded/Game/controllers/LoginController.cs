﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.login;
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
            LoadControllers();
            GetLoginType();
            LoadTicks();
        }

        private void LoadConfigs()
        {
            var player = _gameSession.Player;

            World.DatabaseManager.BasicRefresh(player);
            player.Hangar = World.DatabaseManager.LoadHangar(player);
            player.Hangar.Configurations = World.DatabaseManager.LoadConfig(player);
            player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
            //player.CurrentAmmo = World.DatabaseManager.LoadAmmo(player);
            //player.Pet = new Pet(player.Id, player.Id, "Test", new Hangar(World.StorageManager.Ships[15], new List<Drone>(), player.Position, player.Spacemap, 1000, 0, new Dictionary<string, Item>()), player.FactionId, new Level(1, 1000), 0, 1000, new List<Gear>());
            player.UserStorage.State = State.LOADED;
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
            if (player.Spacemap.Id == 0)
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
                player.Controller = new PlayerController(player);
            player.Controller.Start();
            //if (player.Pet != null)
            //{
            //    player.Pet.Controller = new PetController(player.Pet);
            //}
        }

        private void GetLoginType()
        {
            ILogin type;
            if (Properties.Server.LOCKED)
                type = new Locked(_gameSession);
            else if (_gameSession.Player.CurrentHealth <= 0 || _gameSession.Player.Controller.Dead)
                type = new Killscreen(_gameSession);
            else type = new Regular(_gameSession);
            type.Execute();
        }

        private void LoadTicks()
        {
            Global.TickManager.Add(_gameSession.Player);
            Global.TickManager.Add(_gameSession.Player.Controller);
        }
    }
}