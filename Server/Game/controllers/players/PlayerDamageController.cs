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

        protected override void DamageReceived(object sender, PendingDamage e)
        {
            base.DamageReceived(sender, e);
            PrebuiltCombatCommands.Instance.DamageCommand(Player, e);
        }
    }
}