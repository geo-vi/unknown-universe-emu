using Server.Game.objects.entities.players;
using Server.Game.objects.entities.ships;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities
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

        public DropRewards CargoDrop { get; set; }

        public int AI { get; set; }

        public int RootId
        {
            get
            {
                switch (Id)
                {
                    case 16:
                    case 17:
                    case 18:
                    case 58:
                    case 60:
                        return 8;
                    case 52:
                    case 53:
                    case 54:
                    case 56:
                    case 57:
                    case 59:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 65:
                    case 66:
                    case 67:
                    case 68:
                    case 86:
                    case 87:
                    case 88:
                    case 109:
                    case 110:
                        return 10;
                    default: return Id;
                }
            }
        }

        public Ship(int id, string name, string lootId, int health, int nanohull, int speed, int shield, double shieldAbsorb, int minDamage, int maxDamage, bool neutral, int laserColor,
            int batteries, int rockets, int cargo, Reward reward, DropRewards cargoDrop, int ai)
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
            CargoDrop = cargoDrop;
            AI = ai;
            Damage = GetDamage();
        }

        private int GetDamage()
        {
            return Damage = (MaxDamage - MinDamage) / 2 + MinDamage;
        }

        public double GetHealthBonus()
        {
            switch (LootId)
            {
                case "ship_goliath_design_saturn":
                    return 1.2;
                case "ship_goliath_design_centaur":
                    return 1.1;
                case "ship_leonov":
                    break;
            }
            return 1;
        }

        public double GetDamageBonus()
        {
            switch (LootId)
            {
                case "ship_goliath_design_diminisher":
                case "ship_goliath_design_venom":
                case "ship_goliath_design_referee":
                case "ship_goliath_design_enforcer":
                    return 1.05;
                case "ship_goliath_design_crimson":
                case "ship_goliath_design_independence":
                    return 1.07;
                case "ship_vengeance_design_revenge":
                case "ship_vengeance_design_lightning":
                    return 1.1;
                case "ship_leonov":
                    break;
            }
            return 1;
        }

        public double GetShieldBonus()
        {
            switch (LootId)
            {
                case "ship_goliath_design_bastion":
                case "ship_vengeance_design_avenger":
                case "ship_goliath_design_solace":
                case "ship_goliath_design_spectrum":
                case "ship_goliath_design_sentinel":
                case "ship_goliath_design_kick":
                    return 1.1;
                case "ship_leonov":
                    break;
            }
            return 1;
        }

        public double GetExpBonus()
        {
            double totalBonus = 1;
            switch (LootId)
            {
                case "ship_vengeance_design_adept":
                case "ship_goliath_design_veteran":
                case "ship_goliath_design_ignite":
                case "ship_goliath_design_goal":
                    totalBonus += 0.1;
                    break;
                case "ship_leonov":
                    break;
            }
            return totalBonus;
        }

        public double GetHonorBonus(Player player)
        {
            double totalBonus = 1;
            switch (LootId)
            {
                case "ship_goliath_design_crimson":
                case "ship_goliath_design_independence":
                    totalBonus += 0.03;
                    break;
                case "ship_vengeance_design_corsair":
                case "ship_goliath_design_exalted":
                case "ship_goliath_design_ignite":
                    totalBonus += 0.1;
                    break;
                case "ship_leonov":
                    break;
            }
            return totalBonus;
        }

        public string ToStringLoot()
        {
            if (LootId == "ship_goliath") return "ship_goliath_design_goliath-frost";
            if (LootId != "") return LootId;
            return Id.ToString();
        }

//        public ConcurrentDictionary<ShipAbilities, Ability> Abilities(Player player)
//        {
//            List<Ability> abilities = new List<Ability>();
//            switch (LootId)
//            {
//                case "ship_spearhead":
//                    abilities.Add(new SpearheadDoubleMinimap(player));
//                    abilities.Add(new SpearheadJAMX(player));
//                    abilities.Add(new SpearheadMarkTarget(player));
//                    abilities.Add(new SpearheadUltimateCloak(player));
//                    break;
//                case "ship_aegis":
//                    abilities.Add(new AegisHealBeam(player));
//                    abilities.Add(new AegisShieldRecharge(player));
//                    abilities.Add(new AegisHealPod(player));
//                    break;
//                case "ship_citadel":
//                    abilities.Add(new CitadelDrawFire(player));
//                    abilities.Add(new CitadelFortify(player));
//                    abilities.Add(new CitadelProtection(player));
//                    abilities.Add(new CitadelTravelMode(player));
//                    break;
//                case "ship_goliath_design_solace":
//                    abilities.Add(new NanoClusterRepairer(player));
//                    break;
//                case "ship_goliath_design_sentinel":
//                    abilities.Add(new SentinelFortress(player));
//                    break;
//                case "ship_goliath_design_venom":
//                    abilities.Add(new VenomSingularity(player));
//                    break;
//                case "ship_goliath_design_diminisher":
//                    abilities.Add(new DiminisherWeakenShields(player));
//                    break;
//                case "ship_goliath_design_spectrum":
//                    abilities.Add(new SpectrumPrismaticShielding(player));
//                    break;
//                case "ship_vengeance_design_lightning":
//                    abilities.Add(new VengeanceLightning(player));
//                    break;
//            }
//
//            return new ConcurrentDictionary<Abilities, Ability>(abilities.ToDictionary(a => a.AbilityType, b => b));
//        }

        public int GetAttackRange()
        {
            switch (Id)
            {
                case 67:
                    return 700;
                default: return 500;
            }
        }

        public RocketLaunchers[] GetRocketLauncher(Npc npc)
        {
            if (Id == 114)
            {
               return new[] {RocketLaunchers.HST_02};
            }

            return new RocketLaunchers[0];
        }

        public Ship Strengthen(double p)
        {
            return new Ship(Id, Name, LootId, (int)(Health * p), (int)(Nanohull * p), Speed, (int)(Shield * p),
                ShieldAbsorption, (int)(MinDamage * p), (int)(MaxDamage * p), IsNeutral,
                LaserColor, Batteries, Rockets, Cargo, Reward, CargoDrop.Multiply(2), AI);
        }
    }
}
