using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
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
        /// 1 => 20-50% of ship inventory is lf3, 2 => 50%-99%, 3 => 100%
        /// if 3 => Elite lasers on attack
        /// </summary>
        public int ExpansionStage => CalculateExpansionStage();

        /// <summary>
        /// Extras equipped
        /// </summary>
        public Dictionary<int, Extra> Extras = new Dictionary<int, Extra>();

        public Dictionary<int, EquipmentItem> EquippedItemsOnShip = new Dictionary<int, EquipmentItem>();

        public Dictionary<int, Tuple<Drone, EquipmentItem>> EquippedItemsOnDrones = new Dictionary<int, Tuple<Drone, EquipmentItem>>();

        public Dictionary<int, EquipmentItem> ShieldGens = new Dictionary<int, EquipmentItem>();

        public RocketLauncher RocketLauncher;

        public double ShieldAbsorb => CalculateShieldAbs();

        public Configuration(int id)
        {
            Id = id;
        }

        private double CalculateShieldAbs()
        {
            var countA01 = ShieldGens.Count(x => x.Value.Item.Id == 5);
            var countA02 = ShieldGens.Count(x => x.Value.Item.Id == 7);
            var countA03 = ShieldGens.Count(x => x.Value.Item.Id == 8);
            var countBO1 = ShieldGens.Count(x => x.Value.Item.Id == 9);
            var countBO2 = ShieldGens.Count(x => x.Value.Item.Id == 6);
            return (countA01 * 0.4 + countA02 * 0.5 + countA03 * 0.6 + countBO1 * 0.7 + countBO2 * 0.8) / ShieldGens.Count;
        }

        private int CalculateExpansionStage()
        {
            var lasers = EquippedItemsOnShip.Where(x => x.Value.Item.Category == EquippedItemCategories.LASER).ToArray();
            if (lasers
                .All(x => x.Value.Item.Id == 1 || x.Value.Item.Id == 2))
            {
                return 3;
            }

            if (lasers.Count() > 3)
                return 2;

            return 1;
        }

        public Dictionary<int, Extra> ParseExtras()
        {
            var extras = new Dictionary<int, Extra>();
            
            foreach (var equipmentItem in EquippedItemsOnShip)
            {
                if (equipmentItem.Value.Item.Id == 55 || equipmentItem.Value.Item.Id == 56 ||
                    equipmentItem.Value.Item.Id == 57
                    || equipmentItem.Value.Item.Id == 58)
                {
                    extras.Add(equipmentItem.Key, new Robot(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 59)
                {
                    extras.Add(equipmentItem.Key, new RocketTurbo(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 65)
                {
                    extras.Add(equipmentItem.Key, new ISHCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 66)
                {
                    extras.Add(equipmentItem.Key, new SmartbombCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 67)
                {
                    extras.Add(equipmentItem.Key,
                        new AutoRocketLauncher(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 68 || equipmentItem.Value.Item.Id == 69)
                {
                    extras.Add(equipmentItem.Key, new AimCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 70 || equipmentItem.Value.Item.Id == 71)
                {
                    extras.Add(equipmentItem.Key, new JumpCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 72)
                {
                    extras.Add(equipmentItem.Key, new CargoXtender(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 73 || equipmentItem.Value.Item.Id == 74 ||
                         equipmentItem.Value.Item.Id == 75)
                {
                    extras.Add(equipmentItem.Key, new Cloak(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 76)
                {
                    extras.Add(equipmentItem.Key, new AutoRocket(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 77 || equipmentItem.Value.Item.Id == 78)
                {
                    extras.Add(equipmentItem.Key, new MineTurbo(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 79)
                {
                    extras.Add(equipmentItem.Key, new AutoRepair(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 81)
                {
                    extras.Add(equipmentItem.Key, new AdvancedJumpCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 84 || equipmentItem.Value.Item.Id == 85)
                {
                    extras.Add(equipmentItem.Key, new DROCpu(equipmentItem.Value.Player, equipmentItem.Value));
                }
                else if (equipmentItem.Value.Item.Id == 114)
                {
                    extras.Add(equipmentItem.Key, new TradeDrone(equipmentItem.Value.Player, equipmentItem.Value));
                }
            }

            Extras = extras;
            return extras;
        }

        public RocketLauncher GetRocketLauncher(Player player)
        {
            var launchers = new List<RocketLaunchers>();
            foreach (var equippedItem in EquippedItemsOnShip.Where(x => x.Value.Item.Category == EquippedItemCategories.ROCKET_LAUNCHER))
            {
                if (equippedItem.Value.Item.Id == 25)
                {
                    launchers.Add(RocketLaunchers.HST_02);
                }
                else if (equippedItem.Value.Item.Id == 26)
                {
                    launchers.Add(RocketLaunchers.HST_01);
                }
            }
            var rocketLauncher = new RocketLauncher(player, launchers.ToArray());
            RocketLauncher = rocketLauncher;
            return rocketLauncher;
        }
    }
}
