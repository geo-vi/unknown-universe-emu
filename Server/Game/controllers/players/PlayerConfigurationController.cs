using System;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.entities.ships;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.players
{
    class PlayerConfigurationController : ConfigurationController
    {
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        protected override Configuration[] Create()
        {
            var hangar = Player.Hangar;
            var hangarId = hangar.Id;
            
            Configuration[] configurations = {new Configuration(1), new Configuration(2)};

            if (Player.Equipment.Items.Any(x => x.Value.GeneralCategory == GeneralItemCategories.HM7))
            {
                var equippedItem = Player.Equipment.Items.FirstOrDefault(x => x.Value.GeneralCategory == GeneralItemCategories.HM7);
                configurations[0].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                configurations[1].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
            }

            foreach (var equippedItem in Player.Equipment.Items.Where(x => x.Value.HangarIds.Contains(hangarId)))
            {
                if (equippedItem.Value.OnConfig1.Hangars.Contains(hangarId))
                {
                    configurations[0].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    CalculateItemStats(configurations[0], equippedItem.Value);
                }

                if (equippedItem.Value.OnConfig2.Hangars.Contains(hangarId))
                {
                    configurations[1].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    CalculateItemStats(configurations[1], equippedItem.Value);
                }

                if (equippedItem.Value.OnDroneId1.Hangars.Contains(hangarId))
                {
                    var indexOf = equippedItem.Value.OnDroneId1.Hangars.IndexOf(hangarId);
                    var droneId = equippedItem.Value.OnDroneId1.DroneIds[indexOf];
                    var drone = hangar.Drones[droneId];
                    configurations[0].EquippedItemsOnDrones.Add(equippedItem.Key, new Tuple<Drone, EquipmentItem>(drone, equippedItem.Value));
                    CalculateItemStats(configurations[0], equippedItem.Value, drone);
                }

                if (equippedItem.Value.OnDroneId2.Hangars.Contains(hangarId))
                {
                    var indexOf = equippedItem.Value.OnDroneId2.Hangars.IndexOf(hangarId);
                    var droneId = equippedItem.Value.OnDroneId2.DroneIds[indexOf];
                    var drone = hangar.Drones[droneId];
                    configurations[1].EquippedItemsOnDrones.Add(equippedItem.Key, new Tuple<Drone, EquipmentItem>(drone, equippedItem.Value));
                    CalculateItemStats(configurations[1], equippedItem.Value, drone);
                }

                hangar.Items.Add(equippedItem.Key, equippedItem.Value);
            }

            Out.WriteLog($"Created 2 configurations, 1:: {configurations[0].TotalDamageCalculated} damage, {configurations[0].TotalShieldCalculated} shield, {configurations[0].TotalSpeedCalculated} speed," +
                         $" 2:: {configurations[1].TotalDamageCalculated} damage, {configurations[1].TotalShieldCalculated} shield, {configurations[1].TotalSpeedCalculated} speed", LogKeys.PLAYER_LOG, Player.Id);
            return configurations;
        }
        
        private void CalculateItemStats(Configuration config, EquipmentItem item, Drone drone = null)
        {
            switch (item.GeneralCategory) 
            {
                case GeneralItemCategories.LASER:
                    var damage = 0;
                    switch (item.LootId)
                    {
                        case "equipment_weapon_laser_lf-3": // lf3
                            damage = UpgradeDamage(150, item.Level);
                            break;
                        case "equipment_weapon_laser_lf-4": // lf4
                            damage = UpgradeDamage(200, item.Level);
                            break;
                        case "equipment_weapon_laser_lf-2": // lf2
                            damage = UpgradeDamage(100, item.Level);
                            break;
                        case "equipment_weapon_laser_mp-1": // mp1
                            damage = UpgradeDamage(60, item.Level, "c");
                            break;
                        case "equipment_weapon_laser_lf-1": // lf1
                            damage = UpgradeDamage(40, item.Level, "c");
                            break;

                    }

                    if (drone != null)
                    {
                        damage = (int) (damage * drone.GetDamageBoost());
                    }
                    config.TotalDamageCalculated += damage;
                    break;
                case GeneralItemCategories.SHIELD_GENERATOR:
                    var shield = 0;
                    switch (item.LootId)
                    {
                        case "equipment_generator_shield_sg3n-a01": // A01
                            shield = UpgradeShield(1000, item.Level, "c");
                            break;
                        case "equipment_generator_shield_sg3n-a02": // AO2
                            shield = UpgradeShield(2000, item.Level, "c");
                            break;
                        case "equipment_generator_shield_sg3n-a03": // AO3
                            shield = UpgradeShield(5000, item.Level, "c");
                            break;
                        case "equipment_generator_shield_sg3n-b01": // BO1
                            shield = UpgradeShield(8000, item.Level);
                            break;
                        case "equipment_generator_shield_sg3n-b02": // BO2
                            shield = UpgradeShield(10000, item.Level);
                            break;
                    }

                    if (drone != null)
                    {
                        shield = (int) (shield * drone.GetShieldBoost());
                    }

                    config.TotalShieldCalculated += shield;
                    break;
                case GeneralItemCategories.SPEED_GENERATOR:
                    var speed = 0;
                    switch (item.LootId)
                    {
                        case "equipment_generator_speed_g3n-1010":
                            speed = 2;
                            break;
                        case "equipment_generator_speed_g3n-2010":
                            speed = 3;
                            break;
                        case "equipment_generator_speed_g3n-3210":
                            speed = 4;
                            break;
                        case "equipment_generator_speed_g3n-3310":
                            speed = 5;
                            break;
                        case "equipment_generator_speed_g3n-6900":
                            speed = 7;
                            break;
                        case "equipment_generator_speed_g3n-7900":
                            speed = 10;
                            break;
                    }
                    config.TotalSpeedCalculated += speed;
                    break;
            }
        }
        
        /// <summary>
        /// Calculating damage with boost and by boost type
        /// </summary>
        /// <param name="actualDamage"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private int UpgradeDamage(int actualDamage, int upgradeLevel, string currency = "u")
        {
            if (upgradeLevel == 1) return actualDamage;
            switch (currency)
            {
                case "c":
                    return 0;
                case "u":
                    return 1;
            }

            return -1;
        }

        /// <summary>
        /// Calculating shield with boost and by boost type
        /// </summary>
        /// <param name="actualShield"></param>
        /// <param name="upgradeLevel"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        private int UpgradeShield(int actualShield, int upgradeLevel, string currency = "u")
        {
            if (upgradeLevel == 1) return actualShield;
            switch (currency)
            {
                case "c":
                    return 0;
                case "u":
                    return 1;
            }

            return -1;
        }

        public override void Switch()
        {
            base.Switch();
            PrebuiltLegacyCommands.Instance.UpdateConfigurations(Player);
            if (Player.Settings.GetSettings<GameplaySettings>().DisplayConfigurationChanges)
            {
                PrebuiltLegacyCommands.Instance.ServerMessage(Player, "Configuration changed!");
            }
        }
    }
}