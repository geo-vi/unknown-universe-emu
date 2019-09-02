using System;
using Server.Game.objects.server;

namespace Server.Game.objects.implementable
{
    abstract class AbstractAttacker : AbstractAttackable
    {
        public event EventHandler<PendingAttack> OnLaserShot;

        public event EventHandler<string> OnLaserAmmunitionChange;
        
        public virtual int Damage { get; set; }

        public int MaximalDamageReduce => 500;

        public int MaximalDamageIncrease => 300;

        public float HitRate => 0.92f;
        
        public virtual int RocketDamage { get; set; }
        
        public virtual int LaserColor { get; set; }
        
        protected AbstractAttacker(int id) : base(id)
        {
        }

        /// <summary>
        /// Once laser is shot it will strike the event
        /// </summary>
        /// <param name="pendingAttack">Attack that was used when striking</param>
        public void OnLaserShoot(PendingAttack pendingAttack)
        {
            OnLaserShot?.Invoke(this, pendingAttack);
        }

        /// <summary>
        /// Once laser ammo is changed
        /// </summary>
        /// <param name="lootId">New laser ammo</param>
        public void OnLaserAmmoChange(string lootId)
        {
            OnLaserAmmunitionChange?.Invoke(this, lootId);
        }
    }
}