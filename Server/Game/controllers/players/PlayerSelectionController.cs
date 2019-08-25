using System;
using Server.Game.controllers.characters;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.players
{
    class PlayerSelectionController : CharacterSelectionController
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
        /// Selecting an attackable
        /// </summary>
        /// <param name="abstractAttackable">Attackable</param>
        /// <exception cref="NotImplementedException">Soon to be removed</exception>
        public override void SelectAttackable(AbstractAttackable abstractAttackable)
        {
            base.SelectAttackable(abstractAttackable);
            
            if (abstractAttackable is Character attackableAsCharacter)
            {
                PrebuiltRangeCommands.Instance.SelectShipCommand(Player, attackableAsCharacter);
            }
            else
            {
                Out.QuickLog("Trying to select something other than Character", LogKeys.ERROR_LOG);
                throw new NotImplementedException("Trying to select something other than Character");
            }
        }
    }
}