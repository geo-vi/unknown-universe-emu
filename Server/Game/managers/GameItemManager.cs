using System;
using System.Linq;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.errors;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.managers
{
    class GameItemManager
    {
        public static GameItemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameItemManager();
                }

                return _instance;
            }
        }

        private static GameItemManager _instance;

        /// <summary>
        /// Using an item
        /// </summary>
        /// <param name="player"></param>
        /// <param name="lootId"></param>
        public void Use(Player player, string lootId)
        {
            if (!Exist(lootId, out var item))
            {
                Out.WriteLog("Item does not exist in item database", LogKeys.ERROR_LOG, player.Id);
                throw new ItemException(player, lootId);
            }
            
            switch (item.GeneralCategory)
            {
                case GeneralItemCategories.AMMO:
                    UseAmmo(player, lootId);
                    break;
            }

            Out.WriteLog("Using item " + lootId, LogKeys.PLAYER_LOG, player.Id);
        }

        /// <summary>
        /// Item activator
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="lootId">Item key</param>
        /// <exception cref="ItemException">If item does not exist it throws error</exception>
        public void Activate(Player player, string lootId)
        {
            if (!Exist(lootId, out var item))
            {
                Out.WriteLog("Item does not exist in item database", LogKeys.ERROR_LOG, player.Id);
                throw new ItemException(player, lootId);
            }
            
            Out.WriteLog("Activating item " + lootId, LogKeys.PLAYER_LOG, player.Id);
        }

        /// <summary>
        /// Checking if an item exists
        /// </summary>
        /// <param name="lootId">Item's lootid</param>
        /// <param name="item">Output item from game storage manager</param>
        /// <returns>Result if item is null</returns>
        public bool Exist(string lootId, out Item item)
        {
            item = GameStorageManager.Instance.FindItem(lootId);
            return item != null;
        }
        
        public bool ExistOnConfiguration(Player player, string lootId)
        {
            return player.GetCurrentConfiguration().EquippedItemsOnShip.Any(x => x.Value.LootId == lootId);
        }

        public void ActivateRobot(Player player)
        {
            
        }

        public void ActivateCloak(Player player)
        {
            
        }

        public void ActivateFireworks(Player player)
        {
            
        }

        public bool IsActive(Player player, string lootId)
        {
            throw new NotImplementedException();
        }

        private void UseAmmo(Player player, string lootId)
        {
            if (!player.Ammunition.Ammo.ContainsKey(lootId))
            {
                Out.WriteLog("Ammunition doesnt exist for player", LogKeys.ERROR_LOG, player.Id);
                throw new ItemException(player, lootId);
            }
            
            if (ItemMap.LaserIds.Contains(lootId))
            {
                CombatManager.Instance.CreateCombat(player, player.Selected, AttackTypes.LASER, lootId);
            }
            else if (ItemMap.RocketIds.Contains(lootId))
            {
                CombatManager.Instance.CreateCombat(player, player.Selected, AttackTypes.ROCKET, lootId);
            }
            else if (ItemMap.MinesIds.Contains(lootId))
            {
                //drop bomb   
            }
            else if (ItemMap.RocketLauncherIds.Contains(lootId))
            {
                CombatManager.Instance.CreateCombat(player, player.Selected, AttackTypes.ROCKET_LAUNCHER, lootId);
            }
            else if (ItemMap.SpecialItemsIds.Contains(lootId))
            {
                //create effect   
            }
        }

        /// <summary>
        /// Counting all player's equipped lasers
        /// </summary>
        /// <param name="player">player</param>
        /// <returns>Laser count on config</returns>
        public int CountLasers(Player player)
        {
            var lasers = player.GetCurrentConfiguration().EquippedItemsOnShip
                .Count(x => x.Value.GeneralCategory == GeneralItemCategories.LASER) + 
                         player.GetCurrentConfiguration().EquippedItemsOnDrones.Count(
                             x => x.Value.Item2.GeneralCategory == GeneralItemCategories.LASER);

            return lasers;
        }
    }
}