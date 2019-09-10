using System;
using Server.Game.netty;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.server;

namespace Server.Game.controllers.characters
{
    class CharacterRegenerationController : AbstractedSubController
    {
        private DateTime _lastRegenerationTime = new DateTime();

        /// <summary>
        /// Adding the heal received event
        /// </summary>
        public override void OnAdded()
        {
            Character.OnHealReceived += CharacterHealReceived;
        }

        public override void OnOverwritten()
        {
            Character.OnHealReceived -= CharacterHealReceived;
        }

        /// <summary>
        /// Removing the heal received event
        /// </summary>
        public override void OnRemoved()
        {
            Character.OnHealReceived -= CharacterHealReceived;
        }
        
        /// <summary>
        /// Every tick process the regenerator
        /// </summary>
        public override void OnTick()
        {
            if (_lastRegenerationTime.AddSeconds(1) > DateTime.Now ||
                Character.LastCombatTime.AddSeconds(5) > DateTime.Now ||
                Character.Formation == DroneFormation.DIAMOND) return;

            HealthRegenerationProcess();
            ShieldRegenerationProcess();
            _lastRegenerationTime = DateTime.Now;
        }

        /// <summary>
        /// Using health regeneration process (for npcs only) 
        /// </summary>
        protected virtual void HealthRegenerationProcess()
        {
            //todo...
        }

        /// <summary>
        /// Shield regeneration process (for players)
        /// </summary>
        protected virtual void ShieldRegenerationProcess()
        {
            //todo...   
            // Takes 25 secs to recover the shield
            var amount = Character.MaxShield / 25;
            if (Character.Formation == DroneFormation.DIAMOND)
                amount = (int)(Character.MaxShield * 0.01);

            if (Character.Formation == DroneFormation.MOTH)
            {
                if (Character.CurrentShield <= 0) return;
                Character.CurrentShield -= amount;
            }
            else
            {
                if (Character.CurrentShield >= Character.MaxShield)
                    return;

                //If the amount + currentShield is more than the maxShield adjusts it
                if (Character.CurrentShield + amount > Character.MaxShield)
                    amount = Character.MaxShield - Character.CurrentShield;

                Character.CurrentShield += amount;
            }

            foreach (var selector in Character.Controller.GetInstance<CharacterSelectionController>()
                .FindAllSelectors())
            {
                if (selector is Player playerSelector)
                {
                    PrebuiltRangeCommands.Instance.SelectShipCommand(playerSelector, Character);
                }
            }
        }
        
        protected virtual void CharacterHealReceived(object sender, PendingHeal e)
        {
            foreach (var selector in Character.Controller.GetInstance<CharacterSelectionController>()
                .FindAllSelectors())
            {
                if (selector is Player playerSelector)
                {
                    PrebuiltCombatCommands.Instance.HealCommand(playerSelector, e);
                }
            }
        }
    }
}