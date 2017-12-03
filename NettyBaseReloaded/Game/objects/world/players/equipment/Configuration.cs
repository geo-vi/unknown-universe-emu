using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Configuration
    {
        public int Id { get; }

        public int Speed { get; set; }
        public int Damage { get; set; }
        public int MaxShield { get; set; }
        public int CurrentShield { get; set; }
        public double ShieldAbsorbation { get; set; }
        public int LaserCount { get; set; }
        public int LaserTypes { get; set; }

        public Dictionary<string, Item> Consumables { get; set; }

        public Configuration(int id, int damage, int speed, int maxShield, int shield, double shieldAbsorbation, int lazerCount, int laserTypes, Dictionary<string, Item> consumables)
        {
            Id = id;
            Speed = speed;
            CurrentShield = shield;
            Damage = damage;
            MaxShield = maxShield;
            ShieldAbsorbation = shieldAbsorbation;
            LaserCount = lazerCount;
            LaserTypes = laserTypes;
            Consumables = consumables;
        }
    }
}
