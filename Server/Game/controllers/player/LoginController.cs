using Server.Game.objects;

namespace Server.Game.controllers.player
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
            CreateSelfShip();
            CreateMapAssets();
        }
        
        /// <summary>
        /// Creates the ship with all it's abilities
        /// </summary>
        private void CreateSelfShip()
        {
            
        }

        /// <summary>
        /// Initiates the map
        /// </summary>
        private void CreateMapAssets()
        {
            
        }

        /// <summary>
        /// Changes the player's state, making him now being able to broadcast his existance
        /// </summary>
        public void ChangePlayerState()
        {
            
        }

        /// <summary>
        /// Changing the state of the player,
        /// putting him on the map where everyone will now see him
        /// </summary>
        private void BroadcastEntryToMap()
        {
            SendLoginGreetings();
        }
        
        private void SendLoginGreetings()
        {
            
        }
    }
}
