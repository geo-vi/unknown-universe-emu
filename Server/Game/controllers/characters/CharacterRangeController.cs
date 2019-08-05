using System;
using System.Linq;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class CharacterRangeController : AbstractedSubController
    {
        
        /// <summary>
        /// Adds all characters to range
        /// </summary>
        public void LoadCharactersInRange()
        {
            foreach (var character in Character.Spacemap.Entities.Where(x =>
                x.Value != Character && 
                Character.InCalculatedRange(x.Value)))
            {
                LoadCharacter(character.Value);
            }
        }

        /// <summary>
        /// Removes all characters from range
        /// </summary>
        public void RemoveCharactersFromRange()
        {
            foreach (var character in Character.RangeView.CharactersInRenderRange)
            {
                RemoveCharacter(character.Value);
            }
        }

        /// <summary>
        /// Loads the Character into range (Creates and displays)
        /// </summary>
        /// <param name="targetCharacter"></param>
        public virtual void LoadCharacter(Character targetCharacter)
        {
            if (Character.RangeView.CharactersInRenderRange.ContainsKey(targetCharacter.Id))
            {
                Out.WriteLog("Already in range of " + Character.Id, LogKeys.ALL_CHARACTER_LOG, targetCharacter.Id);
                return;
            }
            
            Character.RangeView.CharactersInRenderRange.TryAdd(targetCharacter.Id, targetCharacter);
            Console.WriteLine("LoadCharacter");
        }

        /// <summary>
        /// Removes the Character from range (Remove from display)
        /// </summary>
        /// <param name="targetCharacter"></param>
        public virtual void RemoveCharacter(Character targetCharacter)
        {
            if (!Character.RangeView.CharactersInRenderRange.ContainsKey(targetCharacter.Id))
            {
                Out.WriteLog("Cannot find in range of " + Character.Id, LogKeys.ALL_CHARACTER_LOG, targetCharacter.Id);
                return;
            }
            
            Character.RangeView.CharactersInRenderRange.TryRemove(targetCharacter.Id, out _);
        }
    }
}