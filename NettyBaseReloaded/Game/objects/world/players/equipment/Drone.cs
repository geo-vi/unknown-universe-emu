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

        public int Design { get; }

        public Drone(int id, int accountId, DroneType droneType, Level level, int experience, int damage, int design)
        {
            Id = id;
            AccountId = accountId;
            DroneType = droneType;
            Level = level;
            Experience = experience;
            Damage = damage;
            Design = design;
        }

        public void LevelUp(Player player)
        {
            if (!World.StorageManager.Levels.DroneLevels.ContainsKey(Level.Id + 1))
                return;

            Level = World.StorageManager.Levels.DroneLevels[Level.Id + 1];
            new UserHandler().execute(new []{"user", "drones", player.Id.ToString()});
        }

        private DateTime LastUpdate = new DateTime();
        public void Update(Player player)
        {
            if (LastUpdate.AddSeconds(4) > DateTime.Now) return;
            World.DatabaseManager.UpdateDrone(this);
        }
    }

}
