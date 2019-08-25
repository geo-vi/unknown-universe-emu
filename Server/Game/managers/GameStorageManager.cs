using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.server;

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

        
        public readonly ConcurrentDictionary<int, GameSession> GameSessions = new ConcurrentDictionary<int, GameSession>();

        public readonly ConcurrentDictionary<int, Spacemap> Spacemaps = new ConcurrentDictionary<int, Spacemap>();
        
        public readonly ConcurrentDictionary<int, Item> Items = new ConcurrentDictionary<int, Item>();
        
        public readonly ConcurrentDictionary<int, Ship> Ships = new ConcurrentDictionary<int, Ship>();
        
        public readonly ConcurrentDictionary<int, Level> PlayerLevels = new ConcurrentDictionary<int, Level>();
        
        public readonly ConcurrentDictionary<int, Level> DroneLevels = new ConcurrentDictionary<int, Level>();

        public readonly ConcurrentDictionary<int, Level> PetLevels = new ConcurrentDictionary<int, Level>();

        /// <summary>
        /// Boots the storage manager of the Game
        /// </summary>
        public void Initiate()
        {
            GameDatabaseManager.Instance.CreateSpacemaps();
            GameDatabaseManager.Instance.CreateItems();
            GameDatabaseManager.Instance.CreateShips();
            GameDatabaseManager.Instance.CreateGameLevels();
        }
        
        /// <summary>
        /// Gets and returns a gameSession
        /// </summary>
        /// <param name="userId">UserId required</param>
        /// <returns></returns>
        public GameSession GetGameSession(int userId)
        {
            if (GameSessions.ContainsKey(userId))
            {
                return GameSessions[userId];
                
            }

            return null;
        }

        public Item FindItem(int id)
        {
            return Items.ContainsKey(id) ? Items[id] : null;
        }

        public Item FindItem(string lootId)
        {
            var item = Items.FirstOrDefault(x => x.Value.LootId == lootId);
            return item.Value;
        }
        
        public string FindLootId(int itemId)
        {
            var foundItem = FindItem(itemId);
            if (foundItem == null)
            {
                return "";
            }
            return foundItem.LootId;
        }

        public Ship FindShip(int shipId)
        {
            if (Ships.ContainsKey(shipId))
            {
                return Ships[shipId];
            }

            return null;
        }

        public Spacemap FindMap(int mapId)
        {
            if (Spacemaps.ContainsKey(mapId))
            {
                return Spacemaps[mapId];
            }

            return Spacemaps[255];
        }

        public Level FindPlayerLevel(int level)
        {
            if (PlayerLevels.ContainsKey(level))
            {
                return PlayerLevels[level];
            }

            return PlayerLevels[0];
        }

        public Level FindDroneLevel(int level)
        {
            if (DroneLevels.ContainsKey(level))
            {
                return DroneLevels[level];
            }

            return DroneLevels[0];
        }

        public GameSession FindSession(Player player)
        {
            return FindSession(player.Id);
        }

        public GameSession FindSession(int playerId)
        {
            if (GameSessions.ContainsKey(playerId))
            {
                return GameSessions[playerId];
            }

            return null;
        }
    }
}