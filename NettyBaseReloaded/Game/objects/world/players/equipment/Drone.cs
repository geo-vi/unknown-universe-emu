using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.WebSocks.packets.handlers;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Drone
    {
        /**********
         * BASICS *
         **********/
        public int Id { get; }
        public int AccountId { get; }
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

        private int Design1 { get; }

        private int Design2 { get; }

        public Drone(int id, int accountId, DroneType droneType, Level level, int experience, int damage, int design1, int design2)
        {
            Id = id;
            AccountId = accountId;
            DroneType = droneType;
            Level = level;
            Experience = experience;
            Damage = damage;
            Design1 = design1;
            Design2 = design2;
        }

        public int GetDroneDesign(Character character)
        {
            if (character is Player player)
            {
                if (player.CurrentConfig == 1)
                    return Design2;
            }
            return Design1;
        }

        public void LevelUp(Player player)
        {
            if (!World.StorageManager.Levels.DroneLevels.ContainsKey(Level.Id + 1))
                return;

            Level = World.StorageManager.Levels.DroneLevels[Level.Id + 1];
            new UserHandler().execute(null,new []{"user", "drones", player.Id.ToString()});
        }

        private DateTime LastUpdate = new DateTime();
        public void Update(Player player)
        {
            if (LastUpdate.AddSeconds(4) > DateTime.Now) return;
            World.DatabaseManager.UpdateDrone(this);
        }
    }

}
