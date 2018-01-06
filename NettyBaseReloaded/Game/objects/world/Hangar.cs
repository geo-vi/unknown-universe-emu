using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.equipment;

namespace NettyBaseReloaded.Game.objects.world
{
    class Hangar
    {
        public Ship Ship { get; set; }

        public Ship ShipDesign { get; set; }

        public List<Drone> Drones { get; set; }
        public Configuration[] Configurations { get; set; }

        public Vector Position { get; set; }

        public Spacemap Spacemap { get; set; }

        public int Health { get; set; }

        public int Nanohull { get; set; }

        public Dictionary<string, Item> Items { get; set; }

        public bool Active = false;

        //Configurations can be null because npcs will use this class too
        public Hangar(Ship ship, List<Drone> drones, Vector position, Spacemap spacemap, int hp, int nano, Dictionary<string, Item> items, bool active = true, Configuration[] configurations = null)
        {
            Ship = ship;
            ShipDesign = Ship;
            Drones = drones;
            Position = position;
            Spacemap = spacemap;
            Health = hp;
            Nanohull = nano;
            Items = items;
            Active = active;
            Configurations = configurations;
        }

        public void DronesLevelChecker(Player player)
        {
            if (Drones.Count == 0) return;
            foreach (var drone in Drones)
            {
                if (!World.StorageManager.Levels.DroneLevels.ContainsKey(drone.Level.Id + 1))
                    continue;

                foreach (var level in World.StorageManager.Levels.DroneLevels)
                {
                    if (drone.Experience > level.Value.Experience && level.Key > drone.Level.Id)
                        drone.LevelUp(player);
                }

                drone.Update(player);
            }
        }

        public void AddDronePoints(int points)
        {
            if (Ship.Id == 1) points*=10;
            if (Ship.Id == 2) points *= 5;
            if (Ship.Id == 3 || Ship.Id == 7 || Ship.Id == 8) points *= 3;
            if (Ship.Id == 9) points *= 2;

            foreach (var drone in Drones)
            {
                drone.Experience += points;
            }
        }
    }
}
