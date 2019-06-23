using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.objects.world
{
    abstract class IAttackable : ITick
    {
        public int Id { get; }

        private int TickId { get; set; }

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

        public int CollectedDamage { get; set; }

        public virtual int AttackRange => 700;

        public EntityStates EntityState { get; set; }

        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public bool Invisible { get; set; }

        public ConcurrentDictionary<ShipVisuals, VisualEffect> Visuals = new ConcurrentDictionary<ShipVisuals, VisualEffect>();

        protected IAttackable(int id)
        {
            Id = id;
            EntityState = EntityStates.ALIVE;
            Targetable = true;
        }

        public int GetId()
        {
            return TickId;
        }

        public void SetTickId(int id)
        {
            TickId = id;
        }

        public abstract void Tick();

        public abstract void Destroy();

        public abstract void Destroy(Character destroyer);


        public bool InRange(IAttackable attackable, int range = 2000)
        {
            if (attackable == null || attackable.Spacemap != Spacemap || attackable == this || attackable.Id == Id) return false;
            if (range == -1 || attackable.Spacemap.RangeDisabled) return true;
            return Position.DistanceTo(attackable.Position) <= range;
        }

        public virtual void Hit(int totalDamage, int attackerId) { }

        public void TickVisuals()
        {
            foreach (var visual in Visuals.Values)
            {
                visual.Tick();
            }
        }
    }
}
