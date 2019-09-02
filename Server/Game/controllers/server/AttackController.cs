using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class AttackController : ServerImplementedController
    {
        private readonly ConcurrentQueue<PendingAttack> _pendingAttacksQueue = new ConcurrentQueue<PendingAttack>();

        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Attack Controller", LogKeys.SERVER_LOG);
        }

        public override void Tick()
        {
            PendingQueue();
        }

        private void PendingQueue()
        {
            while (!_pendingAttacksQueue.IsEmpty)
            {
                if (!_pendingAttacksQueue.TryDequeue(out var pendingAttack))
                {
                    Out.QuickLog("Something went wrong in attack loop, cannot dequeue", LogKeys.ERROR_LOG);
                    throw new Exception("Failed dequeue attack");
                }

                switch (pendingAttack.AttackType)
                {
                    case AttackTypes.LASER:
                        PendingLaserAttack(pendingAttack);
                        break;
                    case AttackTypes.ROCKET:
                        PendingRocketAttack(pendingAttack);
                        break;
                    case AttackTypes.ROCKET_LAUNCHER:
                        PendingRocketLauncherAttack(pendingAttack);
                        break;
                }
            }
        }
        
        private void PendingLaserAttack(PendingAttack pendingAttack)
        {
            var secondaryLaser = ItemMap.IsSecondaryAmmunition(pendingAttack.LootId);

            if (CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.LASER_SHOT_COOLDOWN) && !secondaryLaser || 
                CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN) && secondaryLaser)
            {
                return;
            }

            var damage = 0;
            var absorbDamage = 0;
            var laserColor = 0;
            
            switch (pendingAttack.LootId)
            {
                case "ammunition_laser_lcb-10":
                    damage = pendingAttack.From.Damage;
                    laserColor = 1;
                    break;
                case "ammunition_laser_mcb-25":
                    damage = pendingAttack.From.Damage * 2;
                    laserColor = 1;
                    break;
                case "ammunition_laser_mcb-50":
                    damage = pendingAttack.From.Damage * 3;
                    laserColor = 2;
                    break;
                case "ammunition_laser_ucb-100":
                    damage = pendingAttack.From.Damage * 4;
                    laserColor = 3;
                    break;
                case "ammunition_laser_sab-50":
                    absorbDamage = pendingAttack.From.Damage * 2;
                    laserColor = 4;
                    break;
                case "ammunition_laser_cbo-100":
                    absorbDamage = pendingAttack.From.Damage;
                    damage = pendingAttack.From.Damage * 2;
                    laserColor = 8;
                    break;
                case "ammunition_laser_rsb-75":
                    damage = pendingAttack.From.Damage * 6;
                    laserColor = 6;
                    break;
                case "ammunition_laser_job-100":
                    damage = pendingAttack.From.Damage * 4;
                    laserColor = 9;
                    break;
                default:
                    laserColor = pendingAttack.From.LaserColor;
                    damage = pendingAttack.From.Damage;
                    absorbDamage = 0;
                    break;
            }

            PrebuiltCombatCommands.Instance.LaserAttackCommand(pendingAttack, laserColor);
            
            ServerController.Get<DamageController>().EnforceDamage(pendingAttack.To, pendingAttack.From, 
                damage, absorbDamage, DamageCalculationTypes.RANDOMISED, AttackTypes.LASER);

            if (secondaryLaser)
            {
                CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From,
                    CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN, 3000));
            }
            else
            {
                CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From,
                    CooldownTypes.LASER_SHOT_COOLDOWN, 850));
            }

            pendingAttack.From.OnLaserShoot(pendingAttack);
        }

        private void PendingRocketAttack(PendingAttack pendingAttack)
        {
            
        }

        private void PendingRocketLauncherAttack(PendingAttack pendingAttack)
        {
            
        }

        /// <summary>
        /// Creating a new combat
        /// </summary>
        /// <param name="pendingAttack">Attack pending</param>
        /// <exception cref="Exception">Error is dropped when duplicate in Queue</exception>
        public void CreateCombat(PendingAttack pendingAttack)
        {
            if (_pendingAttacksQueue.Contains(pendingAttack))
            {
                Out.QuickLog("Duplicate entry of Combat, same PendingAttack already exists in Queue", LogKeys.ERROR_LOG);
                throw new Exception("Found a duplicate entry of Combat");
            }
            
            _pendingAttacksQueue.Enqueue(pendingAttack);
        }

        public PendingAttack[] GetActiveAttacksByAttacker(AbstractAttacker attacker)
        {
            return _pendingAttacksQueue.Where(x => x.From == attacker).ToArray();
        }
    }
}
