using System;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.entities.players.settings;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;

namespace Server.Game.controllers.players
{
    class PlayerCombatController : CharacterCombatController
    {        
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        private bool _configurationChanged = false;

        public override void OnAdded()
        {
            Character.OnConfigurationChanged += OnConfigurationChanged;
        }

        public override void OnRemoved()
        {
            Character.OnConfigurationChanged -= OnConfigurationChanged;
        }

        /// <summary>
        /// Creating a different laser combat designed for player where it calculates the lasers equipped for amount
        /// </summary>
        /// <param name="target"></param>
        protected override void OnLaserCombat(AbstractAttackable target)
        {
            var currentLaserSelected = Player.Settings.GetSettings<SlotbarSettings>().SelectedLaserAmmo;

            Console.WriteLine("OnLaserCombat: " + currentLaserSelected);
            
            CombatManager.Instance.CreateCombat(Player, target, AttackTypes.LASER, currentLaserSelected);
        }

        protected override void OnLaserCooldown(PendingAttack pendingAttack)
        {
            pendingAttack.LootId = Player.Settings.GetSettings<SlotbarSettings>().SelectedLaserAmmo;
            if (_configurationChanged)
            {
                CombatManager.Instance.CreateCombat(Player, pendingAttack.To, AttackTypes.LASER, pendingAttack.LootId);
            }
            else
            {
                if (ItemMap.IsSecondaryAmmunition(pendingAttack.LootId))
                {
                    if (!CooldownManager.Instance.Exists(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN))
                    {
                        CombatManager.Instance.CreateAttackCombat(pendingAttack);
                    }
                    else
                    {
                        var cooldown =
                            CooldownManager.Instance.Get(Character, CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN);
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
                        var cooldown = CooldownManager.Instance.Get(Player, CooldownTypes.LASER_SHOT_COOLDOWN);
                        cooldown.SetOnCompleteAction(() => CombatManager.Instance.CreateAttackCombat(pendingAttack));
                    }
                }
            }
        }

        private void OnConfigurationChanged(object sender, int newConfiguration)
        {
        }
    }
}