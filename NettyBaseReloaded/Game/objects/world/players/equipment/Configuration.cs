using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.players.equipment
{
    class Configuration : PlayerBaseClass
    {
        public int Id { get; }

        public int Speed { get; set; }
        public int Damage { get; set; }
        public int MaxShield { get; set; }
        public int CurrentShield { get; set; }
        public int ShieldAbsorbation { get; set; }
        public int LaserCount { get; set; }
        public int LaserTypes { get; set; }

        public Dictionary<string, Item> Consumables { get; set; }
        public RocketLauncher RocketLauncher { get; set; }

        public Configuration(Player player, int id, int damage, int speed, int maxShield, int shield, int shieldAbsorbation, int lazerCount, int laserTypes, int[] loadedRocketLaunchers, Dictionary<string, Item> consumables) : base(player)
        {
            Id = id;
            Speed = speed;
            CurrentShield = shield;
            Damage = damage;
            MaxShield = maxShield;
            ShieldAbsorbation = shieldAbsorbation; //Should be percentage wise ( ShieldAbsorbation / MaxShield = DamageABS ) ex. (ShieldAbsorbation = 112000 / MaxShield = 140000)  == 0.8 DamageABS
            //ShieldAbsorbation = shieldAbsorbation / MaxShield; //Should be percentage wise ( ShieldAbsorbation / MaxShield = DamageABS ) ex. (ShieldAbsorbation = 112000 / MaxShield = 140000)  == 0.8 DamageABS
            LaserCount = lazerCount;
            LaserTypes = laserTypes;
            Consumables = consumables;
            if (loadedRocketLaunchers?.Length > 0)
                RocketLauncher = new RocketLauncher(player, loadedRocketLaunchers);
        }
    }
}
