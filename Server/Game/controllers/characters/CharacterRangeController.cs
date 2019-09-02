using System.Linq;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class CharacterRangeController : AbstractedSubController
    {
        public override void OnTick()
        {
        }

        /// <summary>
        /// Loads all relevant characters from range
        /// </summary>
        public void LoadCharactersInRange()
        {
            foreach (var character in Character.Spacemap.Entities.Where(x =>
                x.Value != Character && !Character.RangeView.CharactersInRenderRange.ContainsKey(x.Key) &&
                Character.InCalculatedRange(x.Value)))
            {
                LoadCharacter(character.Value);
                
                CharacterRangeManager.Instance.UpdateCharacterRange(character.Value);
            }
        }

        /// <summary>
        /// Filters all irrelevant entities from range
        /// </summary>
        public void FilterRange()
        {
            foreach (var character in Character.RangeView.CharactersInRenderRange.Where(x =>
                Character.Spacemap != x.Value.Spacemap ||
                !Character.InCalculatedRange(x.Value)))
            {
                RemoveCharacter(character.Value);
                
                CharacterRangeManager.Instance.UpdateCharacterRange(character.Value);
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