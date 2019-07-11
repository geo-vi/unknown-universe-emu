using System;
using System.Collections.Concurrent;
using Server.Game.objects;

namespace Server.Game.managers
{
    class GameStorageManager
    {
        private static GameStorageManager _instance;
        
        public static GameStorageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStorageManager();
                }
                return _instance;
            }
        }

        
        public ConcurrentDictionary<int, GameSession> GameSessions = new ConcurrentDictionary<int, GameSession>();

        /// <summary>
        /// Boots the storage manager of the Game
        /// </summary>
        public void Initiate()
        {
            
        }
        
        /// <summary>
        /// Gets and returns a gameSession
        /// </summary>
        /// <param name="userId">UserId required</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GameSession GetGameSession(int userId)
        {
            throw new NotImplementedException();
        }
    }
}