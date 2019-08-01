using System;
using Server.Game.objects.server;

namespace Server.Game.objects.entities.players
{
    class Information
    {
        /** EXP */
        public double Experience { get; set; }

        /** HONOR */
        public double Honor { get; set; }
        
        /** CREDITS */
        public double Credits { get; set; }

        /** URIDIUM */
        public double Uridium { get; set; }

        /** JACKPOT */
        public float Jackpot { get; set; }
        
        // PREMIUM FINISH TIME > NOW returns TRUE
        public bool Premium => PremiumFinishTime > DateTime.Now;

        public DateTime PremiumFinishTime { get; set; }

        public Level Level { get; set; }
        
        public Information(double experience, double honor, double credits, double uridium, float jackpot, DateTime premiumFinishTime, Level level)
        {
            Experience = experience;
            Honor = honor;
            Credits = credits;
            Uridium = uridium;
            Jackpot = jackpot;
            PremiumFinishTime = premiumFinishTime;
            Level = level;
        }
    }
}