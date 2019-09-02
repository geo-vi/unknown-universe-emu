using Server.Game.controllers;
using Server.Game.objects.entities.players;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities
{
    class Npc : Character
    {
        /****** BASICS */
        
        public new NpcController Controller { get; set; }

        /*********
         * STATS *
         *********/
        public override int MaxHealth
        {
            get
            {
                return Hangar.Ship.Health;
            }
        }

        public sealed override int Damage { get; set; }

        public override int LaserColor => Hangar.Ship.LaserColor;
        
        public sealed override int CurrentShield { get; set; }

        public sealed override int MaxShield { get; set; }

        public sealed override double ShieldAbsorption { get; set; }

        public sealed override double ShieldPenetration { get; set; }

        public Npc MotherShip { get; set; }

        public int RespawnTime { get; set; }
        public bool Respawning { get; set; }

        public override int AttackRange => Hangar.Ship.GetAttackRange();

        public bool IsRegenerating;

        public Npc(int id, string name, Hangar hangar, Factions factionId, Vector position, Spacemap spacemap, int maxHealth, int currentNanoHull,
            int maxShield, int currentShield, int currentHealth, int damage, Reward reward, int respawnTime = 0, bool respawning = true, Npc motherShip = null)
            : base(id, name, hangar, factionId)
        {
            CurrentShield = currentShield;
            MaxShield = maxShield;
            ShieldAbsorption = 0.5;
            MaxHealth = maxHealth;
            CurrentNanoHull = currentNanoHull;
            CurrentHealth = currentHealth;
            Damage = damage;
            MotherShip = motherShip;
            RespawnTime = respawnTime;
            Respawning = respawning;
            Reward = reward;
            IsRegenerating = true;
        }
        
        public int GetMotherShipId()
        {
            if (MotherShip != null)
            {
                return MotherShip.Id;
            }

            return 0;
        }
    }
}
