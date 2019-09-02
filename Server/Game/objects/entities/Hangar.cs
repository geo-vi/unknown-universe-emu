using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities.ships;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities
{
    class Hangar
    {
        /// <summary>
        /// Hangar ID
        /// </summary>
        public int Id { get; set; }

        public Ship Ship { get; set; }

        public Ship ShipDesign { get; set; }

        public ConcurrentDictionary<int, Drone> Drones { get; set; }
        public Configuration[] Configurations { get; set; }

        public Vector Position { get; set; }

        public Spacemap Spacemap { get; set; }

        public int Health { get; set; }

        public int Nanohull { get; set; }

        public Dictionary<int, EquipmentItem> Items = new Dictionary<int, EquipmentItem>();

        public bool Active = false;

        //Configurations can be null because npcs will use this class too
        public Hangar(int id, Ship ship, ConcurrentDictionary<int, Drone> drones, Vector position, Spacemap spacemap, int hp, int nano, bool active = true)
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
    }
}
