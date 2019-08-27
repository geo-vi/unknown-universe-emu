using System;
using Server.Game.objects.server;

namespace Server.Game.objects.implementable
{
    abstract class AbstractAttacker : AbstractAttackable
    {
        public event EventHandler<PendingAttack> OnLaserShot;
        
        public virtual int Damage { get; set; }
        
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
    }
}