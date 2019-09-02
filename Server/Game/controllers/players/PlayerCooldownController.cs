using Server.Game.controllers.characters;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.server;

namespace Server.Game.controllers.players
{
    class PlayerCooldownController : CharacterCooldownController
    {
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }
        
        public override void OnCooldownStart(Cooldown cooldown)
        {
            switch (cooldown.Type)
            {
                case CooldownTypes.EMP_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, "EMP", cooldown.TotalSeconds);
                    break;
            }
        }
    }
}