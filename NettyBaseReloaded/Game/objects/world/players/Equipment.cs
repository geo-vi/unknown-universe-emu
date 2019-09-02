using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Game.objects.world.players.equipment.item;
using NettyBaseReloaded.Main;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Equipment : PlayerBaseClass
    {
        public Dictionary<int, Hangar> Hangars = new Dictionary<int, Hangar>();

        public Dictionary<int, EquipmentItem> EquipmentItems = new Dictionary<int, EquipmentItem>();

        public Hangar ActiveHangar => Hangars.FirstOrDefault(x => x.Value.Active).Value;

        public Equipment(Player player) : base(player)
        {
            LoadEquipment();
        }

        public void LoadEquipment()
        {
            var hangars = CreateHangars();
            LoadItems();
            CreateConfigs(hangars);
            Hangars = hangars;
        }

        private Dictionary<int, Hangar> CreateHangars()
        {
            return World.DatabaseManager.LoadHangar(Player);
        }

        private void LoadItems()
        {
            EquipmentItems = World.DatabaseManager.LoadEquipment(Player);
        }

        private void CreateConfigs(Dictionary<int, Hangar> hangars)
        {
            foreach (var hangar in hangars)
            {
                hangar.Value.Configurations = ConfigParser(hangar.Value);
            }
        }

        private Configuration[] ConfigParser(Hangar hangar)
        {
            Configuration[] configurations = {new Configuration(1), new Configuration(2)};
            if (EquipmentItems.Any(x => x.Value.Item.Category == EquippedItemCategories.HM7))
            {
                var equippedItem = EquipmentItems.FirstOrDefault(x => x.Value.Item.Category == EquippedItemCategories.HM7);
                configurations[0].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                configurations[1].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
            }

            foreach (var equippedItem in EquipmentItems.Where(x => x.Value.HangarIds.Contains(hangar.Id)))
            {
                if (equippedItem.Value.OnConfig1.Hangars.Contains(hangar.Id))
                {
                    configurations[0].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    UpdateItemStats(configurations[0], equippedItem.Value);
                }

                if (equippedItem.Value.OnConfig2.Hangars.Contains(hangar.Id))
                {
                    configurations[1].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    UpdateItemStats(configurations[1], equippedItem.Value);
                }

                if (equippedItem.Value.OnDroneId1.Hangars.Contains(hangar.Id))
                {
                    var indexOf = equippedItem.Value.OnDroneId1.Hangars.IndexOf(hangar.Id);
                    var droneId = equippedItem.Value.OnDroneId1.DroneIds[indexOf];
                    var drone = hangar.Drones[droneId];
                    configurations[0].EquippedItemsOnDrones.Add(equippedItem.Key, new Tuple<Drone, EquipmentItem>(drone, equippedItem.Value));
                    UpdateItemStats(configurations[0], equippedItem.Value, drone);
                }

                if (equippedItem.Value.OnDroneId2.Hangars.Contains(hangar.Id))
                {
                    var indexOf = equippedItem.Value.OnDroneId2.Hangars.IndexOf(hangar.Id);
                    var droneId = equippedItem.Value.OnDroneId2.DroneIds[indexOf];
                    var drone = hangar.Drones[droneId];
                    configurations[1].EquippedItemsOnDrones.Add(equippedItem.Key, new Tuple<Drone, EquipmentItem>(drone, equippedItem.Value));
                    UpdateItemStats(configurations[1], equippedItem.Value, drone);
                }

                hangar.Items.Add(equippedItem.Key, equippedItem.Value);
            }

            configurations[0].ParseExtras();
            configurations[1].ParseExtras();
            return configurations;
        }

        public Configuration[] PetConfigParser()
        {
            Configuration[] configurations = { new Configuration(1), new Configuration(2) };
            foreach (var equippedItem in EquipmentItems.Where(x => x.Value.HangarIds.Contains(ActiveHangar.Id)))
            {
                if (equippedItem.Value.OnPet1.Hangars.Contains(ActiveHangar.Id))
                {
                    configurations[0].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    UpdateItemStats(configurations[0], equippedItem.Value);
                }

                if (equippedItem.Value.OnPet2.Hangars.Contains(ActiveHangar.Id))
                {
                    configurations[1].EquippedItemsOnShip.Add(equippedItem.Key, equippedItem.Value);
                    UpdateItemStats(configurations[1], equippedItem.Value);
                }
            }

            return configurations;
        }

        public void UpdateItemStats(Configuration config, EquipmentItem item, Drone drone = null)
        {
            switch (item.Item.Category) 
            {
                case EquippedItemCategories.LASER:
                    var damage = 0;
                    switch (item.Item.Id)
                    {
                        case 1: // lf3
                            damage = UpgradeDamage(150, item.Level);
                            break;
                        case 2: // lf4
                            damage = UpgradeDamage(200, item.Level);
                            break;
                        case 22: // lf2
                            damage = UpgradeDamage(100, item.Level);
                            break;
                        case 23: // mp1
                            damage = UpgradeDamage(60, item.Level, "c");
                            break;
                        case 24: // lf1
                            damage = UpgradeDamage(40, item.Level, "c");
                            break;

                    }

                    if (drone != null)
                    {
                        damage = (int) (damage * drone.GetDamageBoost());
                    }
                    config.TotalDamageCalculated += damage;
                    break;
                case EquippedItemCategories.SHIELD_GENERATOR:
                    var shield = 0;
                    switch (item.Item.Id)
                    {
                        case 5: // A01
                            shield = UpgradeShield(1000, item.Level, "c");
                            break;
                        case 7: // AO2
                            shield = UpgradeShield(2000, item.Level, "c");
                            break;
                        case 8: // AO3
                            shield = UpgradeShield(5000, item.Level, "c");
                            break;
                        case 9: // BO1
                            shield = UpgradeShield(8000, item.Level);
                            break;
                        case 6: // BO2
                            shield = UpgradeShield(10000, item.Level);
                            break;
                    }

                    if (drone != null)
                    {
                        shield = (int) (shield * drone.GetShieldBoost());
                    }

                    config.TotalShieldCalculated += shield;
                    config.ShieldGens.Add(item.Id, item); 
                    break;
                case EquippedItemCategories.SPEED_GENERATOR:
                    var speed = 0;
                    switch (item.Item.Id)
                    {
                        case 16:
                            speed = 2;
                            break;
                        case 17:
                            speed = 3;
                            break;
                        case 18:
                            speed = 4;
                            break;
                        case 19:
                            speed = 5;
                            break;
                        case 20:
                            speed = 7;
                            break;
                        case 21:
                            speed = 10;
                            break;
                    }
                    config.TotalSpeedCalculated += speed;
                    break;
            }
        }

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

        public DroneFormation[] GetDroneFormations()
        {
            var droneFormations = new List<DroneFormation>();
            droneFormations.Add(DroneFormation.STANDARD);
            foreach (var item in EquipmentItems.Values.Where(x =>
                x.Item.Category == EquippedItemCategories.DRONE_FORMATION))
            {
                switch (item.Item.Id)
                {
                    case 40:
                        droneFormations.Add(DroneFormation.TURTLE);
                        break;
                    case 41:
                        droneFormations.Add(DroneFormation.ARROW);
                        break;
                    case 42:
                        droneFormations.Add(DroneFormation.LANCE);
                        break;
                    case 43:
                        droneFormations.Add(DroneFormation.STAR);
                        break;
                    case 44:
                        droneFormations.Add(DroneFormation.PINCER);
                        break;
                    case 45:
                        droneFormations.Add(DroneFormation.DOUBLE_ARROW);
                        break;
                    case 46:
                        droneFormations.Add(DroneFormation.DIAMOND);
                        break;
                    case 47:
                        droneFormations.Add(DroneFormation.CHEVRON);
                        break;
                    case 48:
                        droneFormations.Add(DroneFormation.MOTH);
                        break;
                    case 49:
                        droneFormations.Add(DroneFormation.CRAB);
                        break;
                    case 50:
                        droneFormations.Add(DroneFormation.HEART);
                        break;
                    case 51:
                        droneFormations.Add(DroneFormation.BARRAGE);
                        break;
                    case 52:
                        droneFormations.Add(DroneFormation.BAT);
                        break;
                }
            }
            return droneFormations.ToArray();
        }

        public int LaserCount(bool pet = false)
        {
            if (!pet)
            {
                var hangar = ActiveHangar.Configurations[Player.CurrentConfig - 1];
                var shipItems = hangar.EquippedItemsOnShip
                    .Count(x => x.Value.Item.Category == EquippedItemCategories.LASER);
                var droneItems = hangar.EquippedItemsOnDrones
                    .Count(x => x.Value.Item2.Item.Category == EquippedItemCategories.LASER);
                return shipItems + droneItems;
            }
            else
            {
                var hangar = Player.Pet.Hangar.Configurations[Player.CurrentConfig - 1];
                var petItems = hangar.EquippedItemsOnShip
                    .Count(x => x.Value.Item.Category == EquippedItemCategories.LASER);
                return petItems;
            }
        }

        public int LaserTypes()
        {
            var config = ActiveHangar.Configurations[Player.CurrentConfig - 1];
            var lasers = config.EquippedItemsOnShip.Where(x => x.Value.Item.Category == EquippedItemCategories.LASER);
            if (lasers.All(x => x.Value.Item.Id == 1))
                return 3;
            return 0;
        }

        public string GetRobot()
        {
            var robot = Player.Extras.FirstOrDefault(x => x.Value is Robot);
            return robot.Value.EquipmentItem.Item.LootId;
        }

        public Dictionary<int, EquipmentItem> GetDroneEquipment(Drone drone)
        {
            var config = ActiveHangar.Configurations[Player.CurrentConfig - 1];
            var items = config.EquippedItemsOnDrones.Where(x => x.Value.Item1 == drone);
            return items.ToDictionary(x => x.Key, y => y.Value.Item2);
        }

        public string GetConsumablesPacket()
        {
            int? rep = Player.Extras.FirstOrDefault(x => x.Value is Robot).Value?.Level;
            int? droneRep = Player.Extras.FirstOrDefault(x => x.Value is DROCpu).Value?.Level;
            bool ammoBuy = false;
            int? cloak = Player.Extras.FirstOrDefault(x => x.Value is Cloak).Value?.Level;
            int? tradeDrone = Player.Extras.FirstOrDefault(x => x.Value is TradeDrone).Value?.Level;
            int? smb = Player.Extras.FirstOrDefault(x => x.Value is SmartbombCpu).Value?.Level;
            int? ish = Player.Extras.FirstOrDefault(x => x.Value is ISHCpu).Value?.Level;
            int? aim = Player.Extras.FirstOrDefault(x => x.Value is AimCpu).Value?.Level;
            int? autoRocket = Player.Extras.FirstOrDefault(x => x.Value is AutoRocket).Value?.Level;
            int? autoRocketLauncer = Player.Extras.FirstOrDefault(x => x.Value is AutoRocketLauncher).Value?.Level;
            bool rocketBuy = false;
            int? jump = Player.Extras.FirstOrDefault(x => x.Value is AdvancedJumpCpu).Value?.Level;
            bool petRefuel = false;
            int? jumpToBase = Player.Extras.FirstOrDefault(x => x.Value is JumpCpu).Value?.Level;
            int? rokTurbo = Player.Extras.FirstOrDefault(x => x.Value is RocketTurbo).Value?.Level;
            bool radar = false;
            int? mineTurbo = Player.Extras.FirstOrDefault(x => x.Value is MineTurbo).Value?.Level;

            return Convert.ToInt32(droneRep) + "|" + Convert.ToInt32(radar) + "|" + Convert.ToInt32(jumpToBase) + "|" +
                   Convert.ToInt32(ammoBuy) + "|" + Convert.ToInt32(rep) + "|" + Convert.ToInt32(tradeDrone) +
                   "|0|" + Convert.ToInt32(smb) + "|" + Convert.ToInt32(ish) + "|" + Convert.ToInt32(mineTurbo) + "|" + Convert.ToInt32(aim) + "|" +
                   Convert.ToInt32(autoRocket) + "|" + Convert.ToInt32(cloak) + "|" +
                   Convert.ToInt32(autoRocketLauncer) + "|" + Convert.ToInt32(rocketBuy) + "|" +
                   Convert.ToInt32(jump) + "|" + Convert.ToInt32(petRefuel);

        }

        public Configuration GetCurrentConfig()
        {
            var config = ActiveHangar.Configurations[Player.CurrentConfig - 1];
            return config;
        }

        public void ChangeHangar(Hangar targetHangar)
        {
            var activeHangar = ActiveHangar;
            targetHangar.Active = true;
            ActiveHangar.Active = false;
            Player.Refresh();
            World.DatabaseManager.SavePlayerHangar(Player, activeHangar);
            World.DatabaseManager.SavePlayerHangar(Player, targetHangar);
        }

        public void Reload()
        {
            var oldConfig = Player.Hangar.Configurations;
            LoadEquipment();
            Player.Hangar.Configurations[0].CurrentShieldLeft = oldConfig[0].CurrentShieldLeft;
            Player.Hangar.Configurations[1].CurrentShieldLeft = oldConfig[1].CurrentShieldLeft;
        }
    }
}
