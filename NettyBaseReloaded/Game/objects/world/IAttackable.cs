using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.objects.world
{
    abstract class IAttackable : ITick
    {
        public int Id { get; }

        public abstract Vector Position { get; set; }

        public abstract Spacemap Spacemap { get; set; }

        public abstract Faction FactionId { get; set; }

        public abstract int CurrentHealth { get; set; }

        public abstract int MaxHealth { get; set; }

        public abstract int CurrentNanoHull { get; set; }

        public abstract int MaxNanoHull { get; }

        public abstract int CurrentShield { get; set; }

        public abstract int MaxShield { get; set; }

        public abstract double ShieldAbsorption { get; set; }

        public abstract double ShieldPenetration { get; set; }

        public DateTime LastCombatTime { get; set; }

        public virtual int AttackRange => 700;

        public EntityStates EntityState { get; set; }

        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public bool Invisible { get; set; }

        protected IAttackable(int id)
        {
            Id = id;
            EntityState = EntityStates.ALIVE;
            Targetable = true;
            LastCombatTime = DateTime.Now;
        }

        public abstract void Tick();

        public abstract void Destroy();

        public abstract void Destroy(Character destroyer);


        public bool InRange(IAttackable attackable, int range = 2000)
        {
            if (attackable == null || attackable.Spacemap.Id != Spacemap.Id) return false;
            if (range == -1 || attackable.Spacemap.RangeDisabled) return true;
            return attackable.Id != Id &&
                   Position.DistanceTo(attackable.Position) <= range;
        }
    }
}
