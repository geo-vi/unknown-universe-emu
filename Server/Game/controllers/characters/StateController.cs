using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects.entities;
using Server.Game.objects.enums;

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
         */
        
        private ConcurrentDictionary<CharacterStates, bool> CharacterStates = new ConcurrentDictionary<CharacterStates, bool>();
        
        private ConcurrentDictionary<CharacterStates, bool> AllowedStates = new ConcurrentDictionary<CharacterStates, bool>();
        
        public StateController(Character character) : base(character)
        {
        }

        public void AddState(CharacterStates key, bool inState, bool allow)
        {
            if (inState)
            {
                if (!AllowedStates.ContainsKey(key))
                {
                    AllowedStates.TryAdd(key, true);
                }

                if (!CharacterStates.ContainsKey(key))
                {
                    CharacterStates.TryAdd(key, true);
                }
            }
            else
            {
                if (!AllowedStates.ContainsKey(key))
                {
                    AllowedStates.TryAdd(key, false);
                }
            }
        }

        public bool IsInState(CharacterStates key)
        {
            return CharacterStates.ContainsKey(key);
        }

        public bool CanGoToState(CharacterStates key)
        {
            return AllowedStates.ContainsKey(key);
        }

        public void ForbidState(CharacterStates key)
        {
            if (AllowedStates.ContainsKey(key))
            {
                AllowedStates.TryRemove(key, out _);
            }
        }
        
        public void RemoveState(CharacterStates key)
        {
            if (CharacterStates.ContainsKey(key))
            {
                CharacterStates.TryRemove(key, out _);
            }
        }
    }
}