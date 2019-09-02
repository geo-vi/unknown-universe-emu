using System.Collections.Generic;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.enums;
using Server.Game.objects.server;

namespace Server.Game.objects.entities.ships
{
    class Drone
    {
        /**********
         * BASICS *
         **********/
        public int Id { get; }
        
        public DroneTypes DroneType { get; }

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
        
        public Drone(int id, DroneTypes droneType, Level level, int experience, int damage,
            int upgradeLevel)
        {
            Id = id;
            DroneType = droneType;
            Level = level;
            Experience = experience;
            Damage = damage;
            UpgradeLevel = upgradeLevel;
        }

        public double GetDamageBoost()
        {
            var percentage = 10;
            return 1 + (percentage / 100);
        }
        
        public double GetShieldBoost()
        {
            var percentage = 20;
            return 1 + (percentage / 100);
        }
    }
}