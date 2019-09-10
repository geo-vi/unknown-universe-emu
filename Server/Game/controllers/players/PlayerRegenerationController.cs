using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.server;

namespace Server.Game.controllers.players
{
    class PlayerRegenerationController : CharacterRegenerationController
    {
        /// <summary>
        /// Returning Player and converting it from Character
        /// </summary>
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        public override void OnTick()
        {
            if (CharacterStateManager.Instance.IsInState(Player, CharacterStates.NO_CLIENT_CONNECTED))
            {
                // no client...
                return;
            }
            base.OnTick();
        }

        /// <summary>
        /// Removing the functionality
        /// </summary>
        protected override void HealthRegenerationProcess()
        {
        }

        /// <summary>
        /// Adding shield update
        /// </summary>
        protected override void ShieldRegenerationProcess()
        {
            base.ShieldRegenerationProcess();
            PrebuiltPlayerCommands.Instance.UpdateShieldCommand(Player);
        }

        protected override void CharacterHealReceived(object sender, PendingHeal e)
        {
            base.CharacterHealReceived(sender, e);
            PrebuiltCombatCommands.Instance.HealCommand(Player, e);
        }
    }
}