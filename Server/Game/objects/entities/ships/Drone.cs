using System;
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
        
        public int AssignedDroneDesign { get; set; }

        /*********
         * EVENTS *
         *********/

        /// <summary>
        /// On Drone level-up
        /// parameter = new level
        /// </summary>
        public event EventHandler<Level> OnDroneLevelChange;

        public event EventHandler OnDroneDamaged;
        
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

        /// <summary>
        /// Getting drone boost
        /// </summary>
        /// <returns></returns>
        public double GetDamageBoost()
        {
            var percentage = 10;
            return 1 + (percentage / 100);
        }
        
        /// <summary>
        /// Getting shield boost
        /// </summary>
        /// <returns></returns>
        public double GetShieldBoost()
        {
            var percentage = 20;
            return 1 + (percentage / 100);
        }

        /// <summary>
        /// Setting drone damage
        /// </summary>
        /// <param name="damage">Damage from 1-100</param>
        public void SetDroneDamage(int damage)
        {
            Damage = damage;
            OnDroneDamaged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Setting new drone level
        /// </summary>
        /// <param name="level">Level</param>
        public void SetDroneLevel(Level level)
        {
            Level = level;
            OnDroneLevelChange?.Invoke(this, level);
        }
    }
}