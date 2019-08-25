using System;
using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.controllers.players
{
    class PlayerMovementController : CharacterMovementController
    {
        private Player _player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }
        
        private DateTime _lastMovementSaveTime = new DateTime();
        
        /// <summary>
        /// Called upon total movement finish.
        /// </summary>
        public override void OnMovementFinished()
        {
            base.OnMovementFinished();

            if (_lastMovementSaveTime.AddSeconds(3) > DateTime.Now)
            {
                //trying to call save too rapidly
                return;
            }
            
            GameDatabaseManager.Instance.SaveCurrentHangar(_player);
        }
    }
}