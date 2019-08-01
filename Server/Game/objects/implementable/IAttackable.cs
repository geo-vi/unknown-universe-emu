using System;
using System.Collections.Concurrent;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;

namespace Server.Game.objects.implementable
{
    abstract class IAttackable
    {
        public int Id { get; }

        public int TickId { get; set; }

        public abstract Vector Position { get; set; }

        public abstract Spacemap Spacemap { get; set; }

        public abstract Factions FactionId { get; set; }

        public abstract int CurrentHealth { get; set; }

        public abstract int MaxHealth { get; set; }

        public abstract int CurrentNanoHull { get; set; }

        public abstract int MaxNanoHull { get; }

        public abstract int CurrentShield { get; set; }

        public abstract int MaxShield { get; set; }

        public abstract double ShieldAbsorption { get; set; }

        public abstract double ShieldPenetration { get; set; }

        public DateTime LastCombatTime { get; set; }

        public int CollectedDamage { get; set; }

        public virtual int AttackRange => 700;
        
        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public bool Invisible { get; set; }

        public ConcurrentDictionary<ShipVisuals, VisualEffect> Visuals =
            new ConcurrentDictionary<ShipVisuals, VisualEffect>();

        protected IAttackable(int id)
        {
            Id = id;
            Targetable = true;
        }

        public void SetTickId(int id)
        {
            TickId = id;
        }

        public bool InRange(IAttackable attackable, int range = 2000)
        {
            if (attackable == null || attackable.Spacemap != Spacemap || attackable == this ||
                attackable.Id == Id) return false;
            if (range == -1 || attackable.Spacemap.RangeDisabled) return true;
            return Position.DistanceTo(attackable.Position) <= range;
        }
    }
}
