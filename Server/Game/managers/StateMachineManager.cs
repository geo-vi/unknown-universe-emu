using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Server.Game.objects.enums;
using Server.Game.objects.stateMachine;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.managers
{
    class StateMachineManager
    {
        public static StateMachineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StateMachineManager();
                }

                return _instance;
            }
        }

        private static StateMachineManager _instance;

        private HashSet<StateMap> _map = new HashSet<StateMap>();

        private StateMachineManager()
        {
            SetupMap();
        }
        
        /// <summary>
        /// Checking allowed transitions by map
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"></exception>
        public bool IsAllowedTransition(CharacterStates from, CharacterStates to)
        {
            if (@from != to) return _map.Any(x => x.@from.Contains(@from) && x.@to.Contains(to));
            Out.QuickLog("Invalid transition request", LogKeys.ERROR_LOG);
            throw new OverflowException("Invalid transition request");
        }

        private void SetupMap()
        {
            _map = new HashSet<StateMap>
            {
                new StateMap
                {
                    from = new []
                    {
                        CharacterStates.LOGIN
                    },
                    to = new []
                    {
                        CharacterStates.SPAWNED
                    },
                    rule = StateRules.WIPE_ALL_STATES
                },
                new StateMap
                {
                    from = new []
                    {
                        CharacterStates.SPAWNED, CharacterStates.HOME_MAP, CharacterStates.MOVING, CharacterStates.ON_JUMPGATE,
                        CharacterStates.IN_EQUIPMENT_AREA, CharacterStates.ON_BATTLESTATION, CharacterStates.SLEEPING,
                        CharacterStates.SHOOTING, CharacterStates.CLOAKED
                    },
                    to = new []
                    {
                        CharacterStates.ON_JUMPGATE, CharacterStates.ON_COLLECTABLE, CharacterStates.IN_EQUIPMENT_AREA,
                        CharacterStates.ON_BATTLESTATION, CharacterStates.SLEEPING, CharacterStates.JUMPING, CharacterStates.SHOOTING,
                        CharacterStates.HOME_MAP, CharacterStates.CLOAKED, CharacterStates.MOVING,
                        CharacterStates.NO_CLIENT_CONNECTED, CharacterStates.FULLY_DISCONNECTED
                    }
                },
                new StateMap
                {
                    from = new []
                    {
                        CharacterStates.JUMPING
                    },
                    to = new []
                    {
                        CharacterStates.SPAWNED
                    },
                    rule = StateRules.WIPE_ALL_STATES
                },
                new StateMap
                {
                    from = new []
                    {
                        CharacterStates.NO_CLIENT_CONNECTED
                    },
                    to = new []
                    {
                        CharacterStates.LOGIN, CharacterStates.FULLY_DISCONNECTED
                    },
                    rule = StateRules.WIPE_ALL_STATES
                }
            };
        }

        public bool HasMapRule(CharacterStates? from, out StateRules mapRule)
        {
            if (from == null)
            {
                throw new Exception("Something went wrong by checking map rules, origin state is null");
            }
            mapRule = StateRules.KEEP;
            var maps = _map.Where(x => x.from.Any(y => y == from)).ToArray();
            if (maps.Length > 1 && !maps.Any())
            {
                return false;
            }

            var map = maps.FirstOrDefault();
            if (map == null)
            {
                Out.QuickLog("Something went wrong while looking for map rule with state " + @from, LogKeys.ERROR_LOG);
                throw new Exception("Invalid state map for state");
            }
            mapRule = map.rule;
            return mapRule != StateRules.KEEP;
        }
    }
}