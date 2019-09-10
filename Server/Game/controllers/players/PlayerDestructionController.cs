using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.server;

namespace Server.Game.controllers.players
{
    class PlayerDestructionController : CharacterDestructionController
    {
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }

        protected override void CharacterOnDestroyed(object sender, PendingDestruction e)
        {
            base.CharacterOnDestroyed(sender, e);
            PrebuiltLegacyCommands.Instance.ServerMessage(Player, "We've temporarily disabled the killscreen, use refresh to respawn for free.");
            Player.Controller.GetInstance<SessionController>().Kill();
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            PrebuiltPlayerCommands.Instance.ShipInitializationCommand(Player);
        }
    }
}