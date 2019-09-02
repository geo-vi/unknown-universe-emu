using System;
using System.Collections.Concurrent;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
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

                if (pendingDamage.CalculationType == DamageCalculationTypes.RANDOMISED)
                {
                    RandomiseDamage(pendingDamage);
                }
                
                switch (pendingDamage.DamageType)
                {
                    case DamageTypes.AREA:
                    case DamageTypes.INVISIBLE_AREA:
                        ProcessAreaDamage(pendingDamage);
                        break;
                    case DamageTypes.ENTITY:
                    case DamageTypes.INVISIBLE_ENTITY:
                        ProcessEntityDamage(pendingDamage);
                        break;
                }
            }
        }

        private void RandomiseDamage(PendingDamage pendingDamage)
        {
            if (pendingDamage.Attacker == null)
            {
                Out.WriteLog("Trying to randomise a damage without attacker", LogKeys.ERROR_LOG);
                throw new Exception("Trying to randomise a damage where there is no attacker");
            }
                
            var attacker = pendingDamage.Attacker;
            
            var random = RandomInstance.getInstance(attacker).NextDouble();
            if (pendingDamage.Damage > 0)
            {
                if (random > attacker.HitRate)
                {
                    pendingDamage.Damage = 0;
                }
                else
                {
                    var randomDamage = Convert.ToInt32(RandomInstance.getInstance(attacker).Next(pendingDamage.Damage - attacker.MaximalDamageReduce, attacker.MaximalDamageIncrease + pendingDamage.Damage));
                    pendingDamage.Damage = randomDamage;
                }
            }

            if (pendingDamage.AbsorbDamage > 0)
            {
                if (random > attacker.HitRate)
                {
                    pendingDamage.AbsorbDamage = 0;
                }
                else
                {
                    var randomDamage = Convert.ToInt32(RandomInstance.getInstance(attacker).Next(pendingDamage.AbsorbDamage - attacker.MaximalDamageReduce, attacker.MaximalDamageIncrease + pendingDamage.AbsorbDamage));
                    pendingDamage.AbsorbDamage = randomDamage;
                }
            }
        }
        
        private void ProcessEntityDamage(PendingDamage pendingDamage)
        {
            if (pendingDamage.Target.IsDead)
            {
                Out.WriteLog("Trying to damage a dead entity", LogKeys.ERROR_LOG, pendingDamage.Attacker.Id);
                return;
            }
                
            if (pendingDamage.Attacker.Invisible)
            {
                //todo: uncloak
            }

            pendingDamage.Attacker.LastCombatTime =
                DateTime.Now; //To avoid repairing and logging off | My' own logging is set to off in the correspondent handlers

            if (pendingDamage.Target.Invincible)
            {
                // to all selected send that shit
                //PrebuiltCombatCommands.Instance.DamageCommand();
                return;
            }
            
            DamageAttackable(pendingDamage.Target, pendingDamage.Attacker, pendingDamage.Damage, pendingDamage.AbsorbDamage);
            AnnounceDamage(pendingDamage);
        }

        private void ProcessAreaDamage(PendingDamage pendingDamage)
        {
            
        }

        private void DamageAttackable(AbstractAttackable target, int damage, int shieldDamage)
        {
            if (target.CurrentShield > 0)
            {
                int healthDamage;
                if (target.CurrentShield <= damage || target.CurrentShield <= shieldDamage)
                {
                    healthDamage = Math.Abs(target.CurrentShield - damage);
                    target.CurrentShield = 0;
                    target.CurrentHealth -= healthDamage;
                }
                else
                {
                    target.CurrentShield -= shieldDamage; //Correspondent shield damage
                    healthDamage = damage;
                }

                if (target.CurrentNanoHull > 0)
                {
                    //If the player can receive some damage on the nanohull
                    if (target.CurrentNanoHull - healthDamage < 0)
                    {
                        var nanoDamage = healthDamage - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHealth -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= healthDamage;
                }
                else
                    target.CurrentHealth -= healthDamage; //80% shield abs => 20% life
            }
            else //NO SHIELD
            {
                if (target.CurrentNanoHull > 0)
                {
                    //If the player can receive some damage on the nanohull
                    if (target.CurrentNanoHull - damage < 0)
                    {
                        var nanoDamage = damage - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHealth -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= damage; //Full dmg to nanohull
                }
                else
                {
                    target.CurrentHealth -= damage; //Full dmg to health
                }
            }
        }

        private void DamageAttackable(AbstractAttackable target, AbstractAttacker attacker, int hpDamage,
            int shieldDamage)
        {
            if (target.CurrentShield > 0)
            {
                //if (totalAbsDamage > 0)
                //    totalDamage += totalAbsDamage;
                //For example => Target has 80% abs but you' have moth (+20% penetration) :> damage * 0.6

                var totalAbs = Math.Abs(attacker.ShieldPenetration - target.ShieldAbsorption);

                if (totalAbs > 0)
                {
                    var _absDmg = (int) (totalAbs * shieldDamage);
                    target.CurrentShield -= _absDmg;
                    if (target.CurrentShield < 0) target.CurrentShield = 0;
                    if (attacker != null)
                        attacker.CurrentShield += _absDmg;
                }
                
                int healthDamage;
                if (target.CurrentShield <= hpDamage || target.CurrentShield <= shieldDamage)
                {
                    healthDamage = Math.Abs(target.CurrentShield - hpDamage);
                    target.CurrentShield = 0;
                    target.CurrentHealth -= healthDamage;
                }
                else
                {
                    target.CurrentShield -= (int) shieldDamage; //Correspondent shield damage
                    healthDamage = (int) (hpDamage * (1 - totalAbs));
                }

                if (target.CurrentNanoHull > 0)
                {
                    //If the player can receive some damage on the nanohull
                    if (target.CurrentNanoHull - healthDamage < 0)
                    {
                        var nanoDamage = healthDamage - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHealth -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= healthDamage;
                }
                else
                    target.CurrentHealth -= healthDamage; //80% shield abs => 20% life
            }
            else //NO SHIELD
            {
                if (target.CurrentNanoHull > 0)
                {
                    //If the player can receive some damage on the nanohull
                    if (target.CurrentNanoHull - hpDamage < 0)
                    {
                        var nanoDamage = hpDamage - target.CurrentNanoHull;
                        target.CurrentNanoHull = 0;
                        target.CurrentHealth -= nanoDamage;
                    }
                    else
                        target.CurrentNanoHull -= hpDamage; //Full dmg to nanohull
                }
                else
                {
                    target.CurrentHealth -= hpDamage; //Full dmg to health
                }
            }
        }

        private void AnnounceDamage(PendingDamage pendingDamage)
        {
            pendingDamage.Target?.OnDamaged(pendingDamage);
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
            if (target == null)
            {
                Out.QuickLog("Something went wrong enforcing damage, target is null", LogKeys.ERROR_LOG);
                throw new Exception("Target is null, cannot damage");
            }

            if (attacker == null)
            {
                Out.QuickLog("Something went wrong enforcing damage, attacker is null", LogKeys.ERROR_LOG);
                throw new Exception("Attacker is null, cannot damage");
            }

            if (damage == 0 && absorbDamage == 0)
            {
                // damage finished
                return;
            }
            
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
            if (target == null)
            {
                Out.QuickLog("Something went wrong enforcing damage, target is null", LogKeys.ERROR_LOG);
                throw new Exception("Target is null, cannot damage");
            }
            
            if (damage == 0 && absorbDamage == 0)
            {
                // damage finished
                return;
            }

            _pendingDamages.Enqueue(new PendingDamage(target, damage, absorbDamage, calculationType, attackType));
        }
    }
}
