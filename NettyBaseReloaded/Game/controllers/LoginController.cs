using System;
using System.Collections.Generic;
using System.Diagnostics;
using NettyBaseReloaded.Game.controllers.login;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main;
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
            ReloadPlayer();
            LoadConfigs();
            CheckPos();
            LoadControllers();
            GetLoginType();
            LoadTicks();
        }

        private void ReloadPlayer()
        {
            if (_gameSession.InProcessOfDisconnection || _gameSession.InProcessOfReconection)
            {
                _gameSession.InProcessOfDisconnection = false;
                _gameSession.InProcessOfReconection = false;
                _gameSession.EstDisconnectionTime = DateTime.MaxValue;
                _gameSession.LastActiveTime = DateTime.Now;
            }

            if (_gameSession.Player.Controller != null)
            {
                if (_gameSession.Player.Controller.Miscs.LoggingOut)
                    _gameSession.Player.Controller.Miscs.AbortLogout();
                _gameSession.Player.Controller.Attack.Attacking = false;
                _gameSession.Player.Selected = null;
            }
        }

        private void LoadConfigs()
        {
            var player = _gameSession.Player;
            
            var config = World.DatabaseManager.LoadConfig(player);;
            player.Hangar.Configurations = config;
            player.Hangar.Drones = World.DatabaseManager.LoadDrones(player);
            if (player.Pet != null)
            {
                //todo load pet configs
            }
        }

        private void CheckPos()
        {
            //41600 * 25600
            var player = _gameSession.Player;
            if (player.Spacemap.Id == 255) //todo: tutorial on 255
            {
                var closestStation = player.GetClosestStation();
                player.Spacemap = closestStation.Item2;
                player.Position = closestStation.Item1;
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
        }

        private void GetLoginType()
        {
            ILogin type;
            if (Properties.Server.LOCKED)
                type = new Locked(_gameSession);
            else if (_gameSession.Player.CurrentHealth <= 0 || _gameSession.Player.EntityState == EntityStates.DEAD)
                type = new KilledLogin(_gameSession);
            else type = new Regular(_gameSession);
            type.Execute();
        }

        private void LoadTicks()
        {
            var tickId = 0;
            Global.TickManager.Add(_gameSession.Player, out tickId);
            _gameSession.Player.SetTickId(tickId);
            _gameSession.Player.Controller.Initiate();
        }
    }
}
