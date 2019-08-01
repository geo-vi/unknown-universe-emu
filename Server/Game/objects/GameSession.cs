using System;
using Server.Game.objects.entities;
using Server.Networking;
using Server.Networking.clients;

namespace Server.Game.objects
{
    class GameSession
    {
        /// <summary>
        /// The Client of the user
        /// </summary>
        public GameClient GameClient { get; set; }

        /// <summary>
        /// Loaded with ShipInitialization
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Time of last combat, used for controlling the session activity
        /// </summary>
        public DateTime LastCombatTime => Player.LastCombatTime;

        /// <summary>
        /// Time of login, used for controlling session activity
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// Calculating the last player's movement for session activity
        /// </summary>
        public DateTime LastMovementTime => Player.MovementStartTime.AddSeconds(Player.MovementTime);
        
        public GameSession(GameClient gameClient)
        {
            GameClient = gameClient;
        }
    }
}
