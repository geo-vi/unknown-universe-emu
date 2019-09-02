namespace Server.Game.controllers.players
{
    class PlayerLevelController : PlayerSubController
    {
        /// <summary>
        /// if State is in-game it will only work then
        /// </summary>
        public void Refresh()
        {
            CheckLevel();
        }
        
        /// <summary>
        /// Performs a check on current level
        /// </summary>
        public void CheckLevel()
        {
            
        }
    }
}
