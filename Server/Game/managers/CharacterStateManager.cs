using System;
using Server.Game.controllers.characters;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.managers
{
    class CharacterStateManager
    {
        public static CharacterStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterStateManager();
                }

                return _instance;
            }
        }

        private static CharacterStateManager _instance;
        
        /// <summary>
        /// Request a new state, outputs a bool stateChanged
        /// </summary>
        /// <param name="character"></param>
        /// <param name="requestedState"></param>
        /// <param name="stateChanged"></param>
        /// <exception cref="Exception"></exception>
        public void RequestStateChange(Character character, CharacterStates requestedState, out bool stateChanged)
        {
            CharacterStates? fromState = null;
            var stateController = character.Controller.GetInstance<StateController>();
            var currentStates = stateController.GetCharacterStates();
            stateChanged = false;
            if (currentStates.Length == 0)
            {
                stateChanged = true;
            }
            else
            {
                foreach (var state in currentStates)
                {
                    if (StateMachineManager.Instance.IsAllowedTransition(state, requestedState) &&
                        !stateController.IsInState(requestedState))
                    {
                        stateChanged = true;
                        fromState = state;
                    }
                    else
                    {
                        Console.WriteLine("All states: " + string.Join(", ", currentStates));
                        Out.QuickLog("No permission to move from state: " + state + " to state " + requestedState,
                            LogKeys.ERROR_LOG);
                        throw new Exception("No permission to move to requested state");
                    }
                }
            }

            if (stateChanged)
            {
                if (fromState != null)
                {
                    if (StateMachineManager.Instance.HasMapRule(fromState, out var mapRule))
                    {
                        switch (mapRule)
                        {
                            case StateRules.WIPE_ALL_STATES:
                                WipeStates(character);
                                break;
                        }
                    }
                }
                
                stateController.AddState(requestedState);

                Out.WriteLog("State added: " + requestedState, LogKeys.ALL_CHARACTER_LOG, character.Id);
            }
        }

        /// <summary>
        /// Which state it currently is
        /// </summary>
        /// <param name="character"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool IsInState(Character character, CharacterStates state)
        {
            var stateController = character.Controller.GetInstance<StateController>();
            return stateController.IsInState(state);
        }

        public void WipeStates(Character character)
        {
            var stateController = character.Controller.GetInstance<StateController>();
            stateController.RemoveAllStates();
            Out.WriteLog("Wiped all states for character", LogKeys.ALL_CHARACTER_LOG, character.Id);
        }

        public void RemoveState(Character character, CharacterStates state)
        {
            var stateController = character.Controller.GetInstance<StateController>();
            stateController.RemoveState(state);
            Out.WriteLog("Removed state for character " + state, LogKeys.ALL_CHARACTER_LOG, character.Id);
        }
    }
}