using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.WebSocks.packets.handlers;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Drone
    {
        /**********
         * BASICS *
         **********/
        public int Id { get; }

        public Player Player;

        public DroneType DroneType { get; }

        /*********
         * STATS *
         *********/
        public Level Level { get; set; }
        public int Experience { get; set; }

        private int _damage;
        public int Damage
        {
            get { return _damage; }
            //Damage percentage (0-100)
            set { _damage = (value <= 100) ? value : 100; }
        }

        public int UpgradeLevel { get; set; }

        public Dictionary<int, EquipmentItem> EquipmentItems => Player.Equipment.GetDroneEquipment(this);

        public Drone(int id, Player player, DroneType droneType, Level level, int experience, int damage,
            int upgradeLevel)
        {
            Id = id;
            Player = player;
            DroneType = droneType;
            Level = level;
            Experience = experience;
            Damage = damage;
            UpgradeLevel = upgradeLevel;
        }

        public double GetDamageBoost()
        {
            double droneLevelBoost = 1;
            switch (Level.Id)
            {
                case 2:
                    droneLevelBoost = 1.02;
                    break;
                case 3:
                    droneLevelBoost = 1.04;
                    break;
                case 4:
                    droneLevelBoost = 1.06;
                    break;
                case 5:
                    droneLevelBoost = 1.08;
                    break;
                case 6:
                    droneLevelBoost = 1.1;
                    break;
            }

            return droneLevelBoost;
        }

        public double GetShieldBoost()
        {
            double droneLevelBoost = 1;
            switch (Level.Id)
            {
                case 2:
                    droneLevelBoost = 1.04;
                    break;
                case 3:
                    droneLevelBoost = 1.08;
                    break;
                case 4:
                    droneLevelBoost = 1.12;
                    break;
                case 5:
                    droneLevelBoost = 1.16;
                    break;
                case 6:
                    droneLevelBoost = 1.2;
                    break;
            }

            return droneLevelBoost;
        }

        public int GetDroneDesign()
        {
            var droneDesigns = EquipmentItems.Where(x =>
                x.Value.HangarIds.Contains(Player.Equipment.ActiveHangar.Id) && x.Value.Item.TypeId == 50);
            var currentConfig = Player.CurrentConfig;
            EquipmentItem design = null;
            switch (currentConfig)
            {
                case 1:
                    design = droneDesigns.FirstOrDefault(x =>
                        x.Value.OnDroneId1.DroneIds.Contains(Id) &&
                        x.Value.OnDroneId1.Hangars.Contains(Player.Equipment.ActiveHangar.Id)).Value;
                    if (design == null) return 0;
                    switch (design.Item.Id)
                    {
                        case 86:
                            return 1;
                        case 95:
                            return 2;
                    }
                    return 0;
                case 2:
                    design = droneDesigns.FirstOrDefault(x =>
                        x.Value.OnDroneId2.DroneIds.Contains(Id) &&
                        x.Value.OnDroneId2.Hangars.Contains(Player.Equipment.ActiveHangar.Id)).Value;
                    if (design == null) return 0;

                    switch (design.Item.Id)
                    {
                        case 86:
                            return 1;
                        case 95:
                            return 2;
                    }
                    return 0;
                default:
                    return 0;
            }
        }

        public void AddPoint(Ship ship, Ship destroyedTarget)
        {
            if (ship == null || destroyedTarget == null)
            {
                Experience += 1;
            }
            else
            {
                //todo: FINISH THE SYSTEM>>>
                switch (destroyedTarget.Id)
                {
                    case 84:
                        switch (ship.Id)
                        {
                            case 1:
                                Experience += 4;
                                break;
                            case 2:
                                Experience += 3;
                                break;
                            case 3:
                                Experience += 2;
                                break;
                            case 4:
                                Experience += 1;
                                break;
                        }

                        break;
                    case 71:
                        switch (ship.Id)
                        {
                            case 1:
                                Experience += 6;
                                break;
                            case 2:
                                Experience += 5;
                                break;
                            case 3:
                                Experience += 4;
                                break;
                            case 4:
                                Experience += 3;
                                break;
                            case 5:
                                Experience += 2;
                                break;
                            case 6:
                                Experience += 1;
                                break;
                        }

                        break;
                    default:
                        Experience += 1;
                        break;
                }
            }

            if (Experience > Level.Experience)
            {
                var determined = World.StorageManager.Levels.DetermineDroneLvl(Experience);
                if (determined != null && Level != determined)
                {
                    LevelUp(determined);
                }
            }

            World.DatabaseManager.UpdateDrone(this);
        }

        private void LevelUp(Level determined)
        {
            Level = determined;
            foreach (var rangeSession in GameSession.GetRangeSessions(Player))
            {
                if (rangeSession.Value != null)
                {
                    Packet.Builder.DronesCommand(rangeSession.Value, Player);
                }
            }
        }

        public void DamageDrone()
        {
            Damage += 2;
            var droneCpu = Player.Extras.FirstOrDefault(x => x.Value is DROCpu);
            if (Damage == 100)
            {
                if (droneCpu.Value != null && droneCpu.Value.Amount > 0)
                {
                    Damage = 0;
                    droneCpu.Value.Amount -= 1;
                }
                else
                {
                    DestroyDrone();  
                }
            }
        }

        public void DestroyDrone()
        {
            //TODO
        }
    }

}
