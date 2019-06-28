using System;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities.players
{
    abstract class Tech
    {
        public Techs Type;

        public bool Active;

        public int DurationTime;

        public DateTime TimeStarted;
        
        public DateTime TimeEnding;

        public int Count;
        
        protected Tech(Techs type, int durationTime, int playerCount)
        {
            Type = type;
            DurationTime = durationTime;
        }
    }
}