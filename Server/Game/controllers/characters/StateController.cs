using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class StateController : AbstractedSubController
    {
        /*
         *    States:
         *        onJumpgate
         *        onCollectable
         *        inEquipmentArea
         *        jumping
         *        shooting
         *        sleeping
         *        homeMap
         *        onBattleStation
         *
         *     .. discontinued documentation, more states on CharacterStates enum )
         */
        
        private HashSet<CharacterStates> CharacterStates = new HashSet<CharacterStates>();
        
        public void AddState(CharacterStates key)
        {
            if (CharacterStates.Contains(key))
            {
                Out.WriteLog("Something is wrong, trying to add an existing state?", LogKeys.ERROR_LOG);
                return;
            }
            
            CharacterStates.Add(key);
        }

        public bool IsInState(CharacterStates key)
        {
            return CharacterStates.Contains(key);
        }

        public CharacterStates[] GetCharacterStates()
        {
            var states = CharacterStates.ToArray();
            return states;
        }
        
        public void RemoveState(CharacterStates key)
        {
            if (CharacterStates.Contains(key))
            {
                CharacterStates.Remove(key);
            }
            else
            {
                Out.WriteLog("Something is wrong, trying to remove an non-existing state?", LogKeys.ERROR_LOG);
            }
        }

        public void RemoveAllStates()
        {
            CharacterStates.Clear();
        }
    }
}