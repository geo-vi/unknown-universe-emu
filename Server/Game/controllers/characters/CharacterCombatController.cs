using System;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class CharacterCombatController : AbstractedSubController
    {
        private bool InLaserCombat { get; set; }
        
        /// <summary>
        /// Creating the laser attack
        /// </summary>
        /// <param name="target"></param>
        /// <exception cref="Exception"></exception>
        public void OnLaserAttackStart(AbstractAttackable target)
        {
            if (InLaserCombat)
            {
                Out.QuickLog("Currently ongoing laser attack and trying to re-enable it", LogKeys.ERROR_LOG);
                throw new Exception("Laser attack is already ongoing");
            }

            Character.OnLaserShot += OnLaserShotComplete;
            Character.OnLaserAmmunitionChange += OnLaserAmmunitionChanged;
            InLaserCombat = true;

            OnLaserCombat(target);
        }

        /// <summary>
        /// Creating the combat - can be overriden for Player for instance
        /// </summary>
        /// <param name="target"></param>
        protected virtual void OnLaserCombat(AbstractAttackable target)
        {
            CombatManager.Instance.CreateCombat(Character, target, AttackTypes.LASER);
        }

        /// <summary>
        /// After laser shot is complete and if still subscribed to the attack it will continue
        /// </summary>
        /// <param name="sender">AbstractAttacker</param>
        /// <param name="pendingAttack">Attack from event</param>
        /// <exception cref="Exception">Still subscribed to the event</exception>
        private void OnLaserShotComplete(object sender, PendingAttack pendingAttack)
        {
            if (!InLaserCombat)
            {
                Out.QuickLog("Event didn't unsubscribe on AttackController", LogKeys.ERROR_LOG);
                throw new Exception("Still listening to LaserShot event while there is no ongoing attack");
            }
            
            OnLaserCooldown(pendingAttack);
        }

        protected virtual void OnLaserCooldown(PendingAttack pendingAttack)
        {
            if (ItemMap.IsSecondaryAmmunition(pendingAttack.LootId))
            {
                if (!CooldownManager.Instance.Exists(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN))
                {
                    CombatManager.Instance.CreateAttackCombat(pendingAttack);
                }
                else
                {
                    var cooldown = CooldownManager.Instance.Get(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN);
                    cooldown.SetOnCompleteAction(() => CombatManager.Instance.CreateAttackCombat(pendingAttack));
                }
            }
            else
            {
                if (!CooldownManager.Instance.Exists(Character, CooldownTypes.LASER_SHOT_COOLDOWN))
                {
                    CombatManager.Instance.CreateAttackCombat(pendingAttack);
                }
                else
                {
                    var cooldown = CooldownManager.Instance.Get(Character, CooldownTypes.LASER_SHOT_COOLDOWN);
                    cooldown.SetOnCompleteAction(() => CombatManager.Instance.CreateAttackCombat(pendingAttack));
                }
            }
        }
        
        public void OnLaserAmmunitionChanged(object sender, string lootId)
        {
            if (!InLaserCombat)
            {
                Out.QuickLog("Event didn't unsubscribe on AttackController", LogKeys.ERROR_LOG);
                throw new Exception("Still listening to Laser Ammo Change event while there is no ongoing attack");
            }

            var secondaryAmmo = ItemMap.IsSecondaryAmmunition(lootId);
            
            if (CooldownManager.Instance.Exists(Character, CooldownTypes.LASER_SHOT_COOLDOWN) && !secondaryAmmo)
            {
                CooldownManager.Instance.Get(Character, CooldownTypes.LASER_SHOT_COOLDOWN).SetOnCompleteAction(() => OnLaserCombat(Character.Selected));
            }
            else if (CooldownManager.Instance.Exists(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN) && secondaryAmmo)
            {
                CooldownManager.Instance.Get(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN).SetOnCompleteAction(() => OnLaserCombat(Character.Selected));
            }
            else
            {
                OnLaserCombat(Character.Selected);
            }
        }

        /// <summary>
        /// After laser attack has ended, unsubscribe from event
        /// </summary>
        public void OnLaserAttackEnded()
        {
            InLaserCombat = false;
            Character.OnLaserShot -= OnLaserShotComplete;
            Character.OnLaserAmmunitionChange -= OnLaserAmmunitionChanged;
        }
        
        //todo: ...
        public void OnRocketAttack(AbstractAttackable target)
        {
            
        }

        //todo: ...
        public void OnRocketLauncherAttack(AbstractAttackable target)
        {
            
        }
    }
}