using System;
using System.Collections.Concurrent;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.objects.entities.players
{
    class Equipment : PlayerImplementedClass
    {
        public ConcurrentDictionary<int, Hangar> Hangars { get; set; }
        
        public ConcurrentDictionary<int, EquipmentItem> Items { get; set; }

        public Equipment(Player player) : base(player) 
        {
            Hangars = new ConcurrentDictionary<int, Hangar>();
        }

        public Hangar GetActiveHangar()
        {
            foreach (var hangar in Hangars)
            {
                if (hangar.Value.Active)
                    return hangar.Value;
            }

            Out.QuickLog("No hangar found.", LogKeys.ERROR_LOG);
            throw new PlayerException(Player);
        }
    }
}