using System;
using System.Collections.Generic;
using System.Linq;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities.ships
{
    class Configuration
    {
        public int Id { get; }
        
        /// <summary>
        /// Shield left - after battle or random save
        /// </summary>
        public int CurrentShieldLeft { get; set; }

        /// <summary>
        /// Total shield we calculated when having an item equipped
        /// </summary>
        public int TotalShieldCalculated { get; set; }

        /// <summary>
        /// Total damage calculations
        /// </summary>
        public int TotalDamageCalculated { get; set; }

        public int TotalSpeedCalculated { get; set; }

        /// <summary>
        /// 0 => 0%
        /// 1 => 20-50% of ship inventory is lf3, 2 => 50%-99%, 3 => 100%
        /// if 3 => Elite lasers on attack
        /// </summary>
        public int ExpansionStage => GetExpansionStage();

        /// <summary>
        /// Extras equipped
        /// </summary>
        public Dictionary<int, EquipmentItem> EquippedItemsOnShip = new Dictionary<int, EquipmentItem>();

        public Dictionary<int, Tuple<Drone, EquipmentItem>> EquippedItemsOnDrones = new Dictionary<int, Tuple<Drone, EquipmentItem>>();

        public RocketLauncher RocketLauncher { get; set; }

        public double ShieldAbsorb => GetShieldAbs();

        public Configuration(int id)
        {
            Id = id;
        }

        private double GetShieldAbs()
        {
            return 0.5;
//            var countA01 = ShieldGens.Count(x => x.Value.Item.Id == 5);
//            var countA02 = ShieldGens.Count(x => x.Value.Item.Id == 7);
//            var countA03 = ShieldGens.Count(x => x.Value.Item.Id == 8);
//            var countBO1 = ShieldGens.Count(x => x.Value.Item.Id == 9);
//            var countBO2 = ShieldGens.Count(x => x.Value.Item.Id == 6);
//            return (countA01 * 0.4 + countA02 * 0.5 + countA03 * 0.6 + countBO1 * 0.7 + countBO2 * 0.8) / ShieldGens.Count;
        }

        private int GetExpansionStage()
        {
            var lasers = EquippedItemsOnShip.Where(x => x.Value.GeneralCategory == GeneralItemCategories.LASER).ToArray();
            if (lasers
                .All(x => x.Value.Id == 1 || x.Value.Id == 2) && lasers.Length > 0)
            {
                return 3;
            }

            if (lasers.Length > 3)
                return 2;

            return lasers.Length > 1 ? 1 : 0;
        }

        public RocketLauncher GetRocketLauncher(Player player)
        {
//            var launchers = new List<RocketLaunchers>();
//            foreach (var equippedItem in EquippedItemsOnShip.Where(x => x.Value.Item.Category == EquippedItemCategories.ROCKET_LAUNCHER))
//            {
//                if (equippedItem.Value.Item.Id == 25)
//                {
//                    launchers.Add(RocketLaunchers.HST_02);
//                }
//                else if (equippedItem.Value.Item.Id == 26)
//                {
//                    launchers.Add(RocketLaunchers.HST_01);
//                }
//            }
//            var rocketLauncher = new RocketLauncher(player, launchers.ToArray());
//            RocketLauncher = rocketLauncher;
//            return rocketLauncher;
            return null;
        }
    }
}