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
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class AttackController : ServerImplementedController
    {
        private readonly ConcurrentQueue<PendingAttack> _pendingAttacksQueue = new ConcurrentQueue<PendingAttack>();

        private readonly ConcurrentDictionary<int, AbstractAttacker> _pendingRemoval = new ConcurrentDictionary<int, AbstractAttacker>();
        
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Attack Controller", LogKeys.SERVER_LOG);
        }

        public override void Tick()
        {
            PendingQueue();
        }

        
        /// <summary>
        /// Processing the attack queue
        /// </summary>
        /// <exception cref="Exception">Attack loop failed</exception>
        private void PendingQueue()
        {
            while (!_pendingAttacksQueue.IsEmpty)
            {
                if (!_pendingAttacksQueue.TryDequeue(out var pendingAttack))
                {
                    Out.QuickLog("Something went wrong in attack loop, cannot dequeue", LogKeys.ERROR_LOG);
                    throw new Exception("Failed dequeue attack");
                }

                if (_pendingRemoval.ContainsKey(pendingAttack.From.Id))
                {
                    _pendingRemoval.TryRemove(pendingAttack.From.Id, out _);
                    continue;
                }
                
                if (pendingAttack.To.Invincible ||
                    pendingAttack.From.Position.DistanceTo(pendingAttack.To.Position) > pendingAttack.From.AttackRange)
                {
                    continue;
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
                
                pendingAttack.To.LastCombatTime = DateTime.Now;
            }
        }
        
        /// <summary>
        /// Processing the laser attacks
        /// </summary>
        /// <param name="pendingAttack"></param>
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
                    break;
            }

            PrebuiltCombatCommands.Instance.LaserAttackCommand(pendingAttack, laserColor);
            
            ServerController.Get<DamageController>().EnforceDamage(pendingAttack.To, pendingAttack.From, 
                damage, absorbDamage, CalculationTypes.RANDOMISED, AttackTypes.LASER);

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

        /// <summary>
        /// Processing the rocket attacks
        /// </summary>
        /// <param name="pendingAttack"></param>
        private void PendingRocketAttack(PendingAttack pendingAttack)
        {
            var specialRocket = ItemMap.IsSpecialRocket(pendingAttack.LootId);
            
            if (CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.ROCKET_COOLDOWN) && !specialRocket)
            {
                return;
            }
            
            var damage = 0;
            var absorbDamage = 0;
            var rocketColor = 0;
            var rocketShot = true;
            
            switch (pendingAttack.LootId)
            {
                case "ammunition_rocket_r-310":
                    rocketColor = 1;
                    damage = pendingAttack.From.RocketDamage;
                    break;
                case "ammunition_rocket_plt-2026":
                    rocketColor = 2;
                    damage = pendingAttack.From.RocketDamage * 2;
                    break;
                case "ammunition_rocket_plt-2021":
                    rocketColor = 3;
                    damage = pendingAttack.From.RocketDamage * 3;
                    break;
                case "ammunition_rocket_plt-3030":
                    rocketColor = 4;
                    damage = pendingAttack.From.RocketDamage * 4; 
                    break;
                case "ammunition_specialammo_pld-8":
                    rocketColor = 5;
                    PlasmaAttack(pendingAttack, out rocketShot);
                    break;
                case "ammunition_specialammo_dcr-250":
                    rocketColor = 6;
                    DecelerationAttack(pendingAttack, out rocketShot);
                    break;
                case "ammunition_specialammo_wiz-x":
                    rocketColor = 8;
                    WizardAttack(pendingAttack, out rocketShot);
                    break;
                default:
                    rocketColor = 1;
                    damage = pendingAttack.From.RocketDamage;
                    break;
            }

            if (!rocketShot) return;
            
            PrebuiltCombatCommands.Instance.RocketAttack(pendingAttack, rocketColor);

            if (!specialRocket)
            {
                CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From,
                    CooldownTypes.ROCKET_COOLDOWN, 3000));

                ServerController.Get<DamageController>().EnforceDamage(pendingAttack.To, pendingAttack.From,
                    damage, absorbDamage, CalculationTypes.RANDOMISED, AttackTypes.ROCKET);
            }

            pendingAttack.From.OnRocketShoot(pendingAttack);
        }

        /// <summary>
        /// Plasma attacks
        /// --- dont fucking know whats causing
        /// </summary>
        /// <param name="pendingAttack"></param>
        /// <param name="rocketShot"></param>
        private void PlasmaAttack(PendingAttack pendingAttack, out bool rocketShot)
        {
            if (CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.PLASMA_COOLDOWN))
            {
                rocketShot = false;
                return;
            }
            
            rocketShot = true;
            CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From, CooldownTypes.PLASMA_COOLDOWN, 30000));
        }

        /// <summary>
        /// Deceleration attacks
        /// Causing ship to slowdown
        /// </summary>
        /// <param name="pendingAttack"></param>
        /// <param name="rocketShot"></param>
        private void DecelerationAttack(PendingAttack pendingAttack, out bool rocketShot)
        {
            if (CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.DECELERATION_COOLDOWN))
            {
                rocketShot = false;
                return;
            }

            rocketShot = true;
            CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From, CooldownTypes.DECELERATION_COOLDOWN, 30000));
        }

        /// <summary>
        /// Wizard attacks
        /// Causing ship graphic change
        /// </summary>
        /// <param name="pendingAttack"></param>
        /// <param name="rocketShot"></param>
        private void WizardAttack(PendingAttack pendingAttack, out bool rocketShot)
        {
            if (CooldownManager.Instance.Exists(pendingAttack.From, CooldownTypes.WIZARD_COOLDOWN))
            {
                rocketShot = false;
                return;
            }

            rocketShot = true;
            CooldownManager.Instance.CreateCooldown(new Cooldown(pendingAttack.From, CooldownTypes.WIZARD_COOLDOWN, 30000));
        }
        
        /// <summary>
        /// Rocket launcher attack
        /// </summary>
        /// <param name="pendingAttack"></param>
        private void PendingRocketLauncherAttack(PendingAttack pendingAttack)
        {
            if (pendingAttack.From is Character originCharacter)
            {
                // because nobody besides character can actually own rocket launcher
                if (originCharacter.RocketLauncher == null)
                {
                    return;
                }   
                
                int damage = 0;
                int absorbDamage = 0;

                var loadedRockets = originCharacter.RocketLauncher.LoadedRockets;
                if (loadedRockets == 0)
                {
                    //shot complete no rockets
                    return;
                }

                if (pendingAttack.Amount != loadedRockets)
                {
                    Out.QuickLog("Something is wrong with Rocket Launcher amount", LogKeys.ERROR_LOG);
                    throw new Exception("Inconsistent Rocket launcher ammo to loaded rockets");
                }

                switch (originCharacter.RocketLauncher.LoadLootId)
                {
                    case "ammunition_rocketlauncher_eco-10":
                        damage = 2000 * loadedRockets;
                        break;
                    case "ammunition_rocketlauncher_hstrm-01":
                        damage = 4000 * loadedRockets;
                        break;
                    case "ammunition_rocketlauncher_ubr-100":
                        var baseDamage = 4000;
                        if (pendingAttack.To is Npc) baseDamage = 7500;
                        damage = baseDamage * loadedRockets;
                        break;
                    case "ammunition_rocketlauncher_sar-01":
                        absorbDamage = 1200 * loadedRockets;
                        break;
                    case "ammunition_rocketlauncher_sar-02":
                        absorbDamage = 5000 * loadedRockets;
                        break;
                }
                
                PrebuiltCombatCommands.Instance.RocketLauncherAttack(pendingAttack, loadedRockets);

                ServerController.Get<DamageController>().EnforceDamage(pendingAttack.To, pendingAttack.From,
                    damage, absorbDamage, CalculationTypes.RANDOMISED, AttackTypes.ROCKET_LAUNCHER);

                originCharacter.RocketLauncher.OnRocketsLaunch();
            }
            else
            {
                Out.QuickLog("Trying to shoot rocket launcher of something which cannot have rocket launcher");
                throw new Exception("Trying to shoot rocket launcher from somewhere where rocket launcher doesn't exist");
            }
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

        /// <summary>
        /// Getting all active attackers
        /// </summary>
        /// <param name="attacker"></param>
        /// <returns></returns>
        public PendingAttack[] GetActiveAttacksByAttacker(AbstractAttacker attacker)
        {
            return _pendingAttacksQueue.Where(x => x.From == attacker).ToArray();
        }

        public void RemoveCombat(AbstractAttacker attacker)
        {
            _pendingRemoval.TryAdd(attacker.Id, attacker);
        }
    }
}
