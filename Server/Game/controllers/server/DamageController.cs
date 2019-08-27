using System;
using System.Collections.Concurrent;
using Server.Game.controllers.implementable;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class DamageController : ServerImplementedController
    {
        private readonly ConcurrentQueue<PendingDamage> _pendingDamages = new ConcurrentQueue<PendingDamage>();
        
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Damage Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
            while (!_pendingDamages.IsEmpty)
            {
                _pendingDamages.TryDequeue(out var pendingDamage);
                Console.WriteLine("Pending damage: " + pendingDamage.Damage);
                //etc...
            }
        }

        /// <summary>
        /// Enforcing a damage where the attacker exists
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="attacker">Attacker</param>
        /// <param name="damage">Damage (if percentage 1-100)</param>
        /// <param name="absorbDamage">Damage absorb = shield transfer (if percentage 1-100)</param>
        /// <param name="calculationType">Calculation type (defined or percentage)</param>
        /// <param name="attackType">Type of attack</param>
        public void EnforceDamage(AbstractAttackable target, AbstractAttacker attacker, int damage, int absorbDamage,
            DamageCalculationTypes calculationType, AttackTypes attackType)
        {
            _pendingDamages.Enqueue(new PendingDamage(target, attacker, damage, absorbDamage, calculationType, attackType));
        }

        /// <summary>
        /// Enforcing a damage where there is no attacker (ag: Fell on a mine or some sort of shit)
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="damage">Damage (if percentage 1-100)</param>
        /// <param name="absorbDamage">Damage absorb = shield transfer (if percentage 1-100)</param>
        /// <param name="calculationType">Calculation type (defined or percentage)</param>
        /// <param name="attackType">Type of attack</param>
        public void EnforceDamage(AbstractAttackable target, int damage, int absorbDamage, DamageCalculationTypes calculationType,
            AttackTypes attackType)
        {
            _pendingDamages.Enqueue(new PendingDamage(target, damage, absorbDamage, calculationType, attackType));
        }
    }
}
