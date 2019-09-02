using System;
using Server.Game.controllers.characters;
using Server.Game.netty;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.server;

namespace Server.Game.controllers.players
{
    class PlayerDamageController : CharacterDamageController
    {
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        protected override void OnDamageReceived(object sender, PendingDamage e)
        {
            PrebuiltCombatCommands.Instance.DamageCommand(Player, e);
            var selectors = Player.Controller.GetInstance<CharacterSelectionController>().FindAllSelectors();
            
            foreach (var selector in selectors)
            {
                if (selector is Player playerSelector)
                {
                    PrebuiltCombatCommands.Instance.DamageCommand(playerSelector, e);
                }
            }
        }
    }
}