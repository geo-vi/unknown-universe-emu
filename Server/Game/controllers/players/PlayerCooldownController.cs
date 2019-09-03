using Server.Game.controllers.characters;
using Server.Game.netty.commands;
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
        
        protected override void OnCooldownAdded(Cooldown cooldown)
        {
            switch (cooldown.Type)
            {
                case CooldownTypes.EMP_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.EMP_COOLDOWN, cooldown.TotalSeconds);
                    break;
                case CooldownTypes.SECONDARY_LASER_SHOT_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.RSB_COOLDOWN, cooldown.TotalSeconds);
                    break;
                case CooldownTypes.ROCKET_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.ROCKET_COOLDOWN, cooldown.TotalSeconds);
                    break;
                case CooldownTypes.PLASMA_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.PLASMA_DISCONNECT_COOLDOWN, cooldown.TotalSeconds);
                    break;
                case CooldownTypes.DECELERATION_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.DCR_ROCKET, cooldown.TotalSeconds);
                    break;
                case CooldownTypes.WIZARD_COOLDOWN:
                    PrebuiltLegacyCommands.Instance.SendCooldown(Player, ServerCommands.WIZ_ROCKET, cooldown.TotalSeconds);
                    break;
            }
        }
    }
}