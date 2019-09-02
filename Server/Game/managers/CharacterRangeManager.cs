using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.managers
{
    class CharacterRangeManager
    {
        public static CharacterRangeManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterRangeManager();
                }

                return _instance;
            }
        }

        private static CharacterRangeManager _instance;

        /// <summary>
        /// Checking if character is in range of another character
        /// </summary>
        /// <param name="character">Primary character</param>
        /// <param name="target">Second character</param>
        /// <returns>True if it's in range</returns>
        public bool IsInCharacterRange(Character character, Character target)
        {
            return character.RangeView.CharactersInRenderRange.ContainsKey(target.Id);
        }

        /// <summary>
        /// Updates the character's range
        /// </summary>
        /// <param name="character"></param>
        public void UpdateCharacterRange(Character character)
        {
            var rangeControl = character.Controller.GetInstance<CharacterRangeController>();
            
            rangeControl.LoadCharactersInRange();
            rangeControl.FilterRange();
        }
    }
}