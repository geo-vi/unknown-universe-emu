using System;
using System.Collections.Concurrent;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.objects.implementable
{
    abstract class AbstractAttackable
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

        public virtual int VisibilityRange => 2000;
        
        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public bool Invisible { get; set; }

        public bool IsDead { get; set; }
        
        public ConcurrentDictionary<ShipVisuals, VisualEffect> Visuals =
            new ConcurrentDictionary<ShipVisuals, VisualEffect>();

        public event EventHandler<PendingDamage> OnDamageReceived;
        
        protected AbstractAttackable(int id)
        {
            Id = id;
            Targetable = true;
        }

        public void SetTickId(int id)
        {
            TickId = id;
        }

        /// <summary>
        /// Checking if attackable is truly in range
        /// </summary>
        /// <param name="abstractAttackable"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool InCalculatedRange(AbstractAttackable abstractAttackable)
        {
            if (abstractAttackable == null || abstractAttackable.Spacemap != Spacemap || abstractAttackable == this ||
                abstractAttackable.Id == Id)
            {
                Out.QuickLog("Something went wrong with attackable specifieid in InRange parameter", LogKeys.ERROR_LOG);
                throw new ArgumentException("Something went wrong with attackable specified in parameter");    
            }

            if (VisibilityRange == -1 || abstractAttackable.Spacemap.RangeDisabled)
            {
                return true;
            }
            
            return Position.DistanceTo(abstractAttackable.Position) <= VisibilityRange;
        }

        /// <summary>
        /// Called to execute damage receive event
        /// </summary>
        /// <param name="pendingDamage"></param>
        public void OnDamaged(PendingDamage pendingDamage)
        {
            OnDamageReceived?.Invoke(this, pendingDamage);
        }
    }
}
