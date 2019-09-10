using System;
using System.Collections.Generic;
using System.Data;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.items;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.managers
{
    class AmmunitionManager
    {
        public static AmmunitionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AmmunitionManager();
                }

                return _instance;
            }
        }

        private static AmmunitionManager _instance;

        public void IncreaseAmmunition(Player player, string lootId, int amount)
        {
            if (amount < 0)
            {
                Out.QuickLog("Trying to add negative amount of ammo");
                throw new ArgumentOutOfRangeException("Invalid ammunition amount to add; Amount tried adding: " + amount);
            }
            if (!player.Ammunition.Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Not found key '{lootId}' at ammunition dictionary");
                throw new KeyNotFoundException("Selected ammunition not found on Player [" + lootId + "]");
            }

            if (!(player.Ammunition.Ammo[lootId] is AmmunitionItem item))
            {
                Out.QuickLog("Failed adding, Ammunition item value is null", LogKeys.ERROR_LOG);
                throw new Exception("Ammunition item value is null");
            }

            item.SetAmount(item.Amount + amount);
            
            player.Ammunition.UpdateAmmunition(item);
        }
        
        public bool TryReduceAmmunition(Player player, string lootId, int amount)
        {
            if (amount > 0)
            {
                Out.QuickLog("Trying to add positive amount of ammo");
                throw new ArgumentOutOfRangeException("Invalid ammunition amount to remove; Amount tried removing: " + amount);
            }
            
            if (!player.Ammunition.Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Not found key '{lootId}' at ammunition dictionary");
                throw new KeyNotFoundException("Selected ammunition not found on Player [" + lootId + "]");
            }

            if (!(player.Ammunition.Ammo[lootId] is AmmunitionItem item))
            {
                Out.QuickLog("Failed removing, Ammunition item value is null", LogKeys.ERROR_LOG);
                throw new Exception("Ammunition item value is null");
            }

            if (item.Amount - amount < 0)
            {
                return false;
            }
            
            item.SetAmount(item.Amount - amount);
            
            player.Ammunition.UpdateAmmunition(item);
            
            return true;
        }

        /// <summary>
        /// Creates a new ammunition record
        /// </summary>
        /// <param name="player"></param>
        /// <param name="lootId"></param>
        /// <param name="amount"></param>
        public void CreateAmmunition(Player player, string lootId, int amount)
        {
            if (player.Ammunition.Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Duplicate key '{lootId}' found in ammo dictionary");
                throw new DuplicateNameException($"Loot-ID '{lootId}' already exists in dictionary");
            }
            
            var item = new AmmunitionItem(lootId, amount);
            player.Ammunition.Ammo.Add(lootId, item);
            //player.Ammunition.Create(item);
        }
    }
}