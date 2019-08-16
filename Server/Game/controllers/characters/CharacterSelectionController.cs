using System;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class CharacterSelectionController : AbstractedSubController
    {
        /// <summary>
        /// Trying to select an attackable
        /// </summary>
        /// <param name="attackable">Attackable trying to select</param>
        /// <exception cref="Exception">If it doesnt exist in range</exception>
        public virtual void SelectAttackable(IAttackable attackable)
        {
            if (!Character.RangeView.CharactersInRenderRange.ContainsKey(attackable.Id))
            {
                Out.QuickLog("Something went wrong in select attackable, attackable not rendered", LogKeys.ERROR_LOG);
                throw new Exception("Something went wrong in select attackable, trying to select something that is not rendered");
            }

            Character.Selected = attackable;
        }

        /// <summary>
        /// Removing selection from attackable
        /// </summary>
        /// <exception cref="Exception">Throws if the selection is already nulled</exception>
        public virtual void EndSelection()
        {
            if (Character.Selected == null)
            {
                Out.QuickLog("Trying to deselect nothing", LogKeys.ERROR_LOG);
                throw new Exception("Selection is already null");
            }
            
            Character.Selected = null;
        }
    }
}
