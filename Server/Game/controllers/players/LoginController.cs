using System;
using Server.Game.controllers.server;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects;
using Server.Game.objects.enums;

namespace Server.Game.controllers.players
{
    class LoginController
    {
        private GameSession GameSession;

        public LoginController(GameSession gameSession)
        {
            GameSession = gameSession;
        }
        
        public void Execute()
        {
            CreateGlobalSession();
            CreateControllers();
            OnLoginSetupFinish();
            CreateSelfShip();
            CreateMapAssets();
            BroadcastEntryToMap();
        }

        /// <summary>
        /// Global session add
        /// </summary>
        private void CreateGlobalSession()
        {
            if (!GameStorageManager.Instance.GameSessions.ContainsKey(GameSession.Player.Id))
            {
                GameStorageManager.Instance.GameSessions.TryAdd(GameSession.Player.Id, GameSession);
            }
        }
        
        /// <summary>
        /// Creating all the controllers
        /// </summary>
        private void CreateControllers()
        {
            if (GameSession.Player.Controller == null)
            {
                var controller = new PlayerController(GameSession.Player);
                GameSession.Player.Controller = controller;
                controller.Initiate();
                
                CharacterStateManager.Instance.RequestStateChange(GameSession.Player, CharacterStates.LOGIN, out _);
            }
        }
        
        protected void OnLoginSetupFinish()
        {
            GameSession.GameClient.Initialized = true;
            GameSession.LoginTime = DateTime.Now;
        }

        /// <summary>
        /// Creates the ship with all it's abilities
        /// </summary>
        private void CreateSelfShip()
        {
            PrebuiltPlayerCommands.Instance.CreateSettingsCommand(GameSession.Player);
            PrebuiltPlayerCommands.Instance.CreateKeyBindingsCommand(GameSession.Player);
            PrebuiltPlayerCommands.Instance.CreateSlotbarSettings(GameSession.Player);
            PrebuiltPlayerCommands.Instance.ShipInitializationCommand(GameSession.Player);
            PrebuiltPlayerCommands.Instance.CreateAmmunition(GameSession.Player);
            PrebuiltLegacyCommands.Instance.SendBootyKeys(GameSession.Player);
            PrebuiltLegacyCommands.Instance.SendQuickbuyPriceMenu(GameSession.Player);
            PrebuiltLegacyCommands.Instance.ConfigurationCommand(GameSession.Player);
        }

        /// <summary>
        /// Initiates the map
        /// </summary>
        private void CreateMapAssets()
        {
            
        }

        /// <summary>
        /// Changing the state of the player,
        /// putting him on the map where everyone will now see him
        /// </summary>
        private void BroadcastEntryToMap()
        {
            ServerController.Get<SpawnController>().QueueCharacterForSpawning(GameSession.Player);
            SendLoginGreetings();
        }
        
        private void SendLoginGreetings()
        {
            
        }
    }
}
