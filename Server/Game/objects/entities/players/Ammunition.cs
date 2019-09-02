using System;
using System.Collections.Generic;
using System.Data;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Utils;

namespace Server.Game.objects.entities.players
{
    class Ammunition
    {
        public Dictionary<string, Item> Ammo = new Dictionary<string, Item>();

        public void AddAmmunition(string lootId, int amount)
        {
            if (amount < 0)
            {
                Out.QuickLog("Trying to add negative amount of ammo");
                throw new ArgumentOutOfRangeException("Invalid ammunition amount to add; Amount tried adding: " + amount);
            }
            if (!Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Not found key '{lootId}' at ammunition dictionary");
                throw new KeyNotFoundException("Selected ammunition not found on Player [" + lootId + "]");
            }

            var item = Ammo[lootId];
            item.SetAmount(item.Amount + amount);
        }

        public void RemoveAmmunition(string lootId, int amount)
        {
            if (amount > 0)
            {
                Out.QuickLog("Trying to add positive amount of ammo");
                throw new ArgumentOutOfRangeException("Invalid ammunition amount to remove; Amount tried removing: " + amount);
            }
            
            if (!Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Not found key '{lootId}' at ammunition dictionary");
                throw new KeyNotFoundException("Selected ammunition not found on Player [" + lootId + "]");
            }

            var item = Ammo[lootId];
            item.SetAmount(item.Amount - amount);
        }

        /// <summary>
        /// Creates a new ammunition record
        /// </summary>
        /// <param name="lootId"></param>
        /// <param name="amount"></param>
        public void Create(string lootId, int amount)
        {
            if (Ammo.ContainsKey(lootId))
            {
                Out.QuickLog($"Duplicate key '{lootId}' found in ammo dictionary");
                throw new DuplicateNameException($"Loot-ID '{lootId}' already exists in dictionary");
            }
            
            var item = new AmmunitionItem(lootId, amount);
            Ammo.Add(lootId, item);
        }
    }
}