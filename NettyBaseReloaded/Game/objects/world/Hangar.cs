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
        /// <summary>
        /// Hangar ID
        /// </summary>
        public int Id { get; set; }

        public Ship Ship { get; set; }

        public Ship ShipDesign { get; set; }

        public Dictionary<int, Drone> Drones { get; set; }
        public Configuration[] Configurations { get; set; }

        public Vector Position { get; set; }

        public Spacemap Spacemap { get; set; }

        public int Health { get; set; }

        public int Nanohull { get; set; }

        public Dictionary<int, EquipmentItem> Items = new Dictionary<int, EquipmentItem>();

        public bool Active = false;

        //Configurations can be null because npcs will use this class too
        public Hangar(int id, Ship ship, Dictionary<int, Drone> drones, Vector position, Spacemap spacemap, int hp, int nano, bool active = true)
        {
            Id = id;
            Ship = ship;
            ShipDesign = Ship;
            Drones = drones;
            Position = position;
            Spacemap = spacemap;
            Health = hp;
            Nanohull = nano;
            Active = active;
        }

        public void AddDronePoint(Ship destroyedTarget)
        {
            foreach (var drone in Drones)
            {
                drone.Value.AddPoint(Ship, destroyedTarget);
            }
        }
    }
}
