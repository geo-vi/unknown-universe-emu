using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;

namespace Server.Game.controllers.players
{
    class PlayerRangeController : CharacterRangeController
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

        /// <summary>
        /// After loaded, sending packet
        /// </summary>
        /// <param name="targetCharacter"></param>
        public override void LoadCharacter(Character targetCharacter)
        {
            base.LoadCharacter(targetCharacter);
            PrebuiltRangeCommands.Instance.CreateShipCommand(Player, targetCharacter);
            if (CharacterStateManager.Instance.IsInState(targetCharacter, CharacterStates.MOVING))
            {
                PrebuiltRangeCommands.Instance.MoveCommand(Player, targetCharacter);
            }
        }

        /// <summary>
        /// After unloading, sending packet
        /// </summary>
        /// <param name="targetCharacter"></param>
        public override void RemoveCharacter(Character targetCharacter)
        {
            base.RemoveCharacter(targetCharacter);
            PrebuiltRangeCommands.Instance.RemoveShipCommand(Player, targetCharacter);
        }
    }
}