using System;
using System.Linq;
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
        /// <param name="abstractAttackable">Attackable trying to select</param>
        /// <exception cref="Exception">If it doesnt exist in range</exception>
        public virtual void SelectAttackable(AbstractAttackable abstractAttackable)
        {
            if (!Character.RangeView.CharactersInRenderRange.ContainsKey(abstractAttackable.Id))
            {
                Out.QuickLog("Something went wrong in select attackable, attackable not rendered", LogKeys.ERROR_LOG);
                throw new Exception("Something went wrong in select attackable, trying to select something that is not rendered");
            }

            Character.Selected = abstractAttackable;
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

        public Character[] FindAllSelectors()
        {
            return Character.RangeView.CharactersInRenderRange.Values.Where(x => x.SelectedCharacter == Character)
                .ToArray();
        }
    }
}
