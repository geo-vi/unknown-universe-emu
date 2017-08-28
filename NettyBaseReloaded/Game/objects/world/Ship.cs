using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.objects.world
{
    class Ship
    {
        /**********
         * BASICS *
         **********/
        public int Id { get; }

        public string Name { get; set; }

        public string LootId { get; set; }

        /*********
         * STATS *
         *********/
        public int Health { get; }
        public int Nanohull { get; set; }
        public int Shield { get; set; }

        public int Speed { get; }

        public double ShieldAbsorption { get; set; }

        private int MinDamage { get; set; }
        private int MaxDamage { get; set; }

        public int Damage { get; set; }

        public bool IsNeutral { get; set; }

        public int LaserColor { get; set; }

        public int Batteries { get; set; }
        public int Rockets { get; set; }

        public int Cargo { get; set; }

        public Reward Reward { get; set; }

        public DropableRewards DropableRewards { get; set; }

        public int AI { get; set; }

        public Ship(int id, string name, string lootId, int health, int nanohull, int speed, int shield, double shieldAbsorb, int minDamage, int maxDamage, bool neutral, int laserColor,
            int batteries, int rockets, int cargo, Reward reward, DropableRewards dropableRewards, int ai)
        {
            Id = id;
            Name = name;
            LootId = lootId;
            Health = health;
            Nanohull = nanohull;
            Speed = speed;
            Shield = shield;
            ShieldAbsorption = shieldAbsorb;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            IsNeutral = neutral;
            LaserColor = laserColor;
            Batteries = batteries;
            Rockets = rockets;
            Cargo = cargo;
            Reward = reward;
            DropableRewards = dropableRewards;
            AI = ai;
            Damage = CalculateDamage();
        }

        private int CalculateDamage()
        {
            return Damage = (MaxDamage - MinDamage) + MinDamage;
        }

        public double GetHealthBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_leonov":
                    if (player.IsOnHomeMaps())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetDamageBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_leonov":
                    if (player.IsOnHomeMaps())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetShieldBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_leonov":
                    if (player.IsOnHomeMaps())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetExpBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_leonov":
                    if (player.IsOnHomeMaps())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public double GetHonorBonus(Player player)
        {
            switch (LootId)
            {
                case "ship_leonov":
                    if (player.IsOnHomeMaps())
                        return 2.0;
                    break;
            }
            return 1;
        }

        public string ToStringLoot()
        {
            if (LootId == "ship_goliath") return "ship_goliath_design_goliath-frost";
            if (LootId != "") return LootId;
            return Id.ToString();
        }
    }
}
