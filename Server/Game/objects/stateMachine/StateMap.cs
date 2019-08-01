using Server.Game.objects.enums;

namespace Server.Game.objects.stateMachine
{
    class StateMap
    {
        public CharacterStates[] from;
        public CharacterStates[] to;
        public StateRules rule = StateRules.KEEP;
    }
}