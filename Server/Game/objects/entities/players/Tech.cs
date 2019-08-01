using System;
using Server.Game.objects.enums;

namespace Server.Game.objects.entities.players
{
    abstract class Tech
    {
        protected Techs Type { get; set; }

        protected bool Active { get; set; }

        protected int DurationTime { get; set; }

        protected DateTime TimeStarted { get; set; }
        
        protected DateTime TimeEnding { get; set; }

        protected int Count { get; set; }
        
        protected Tech(Techs type, int durationTime, int playerCount)
        {
            Type = type;
            DurationTime = durationTime;
        }
    }
}