using System;

namespace Server.Game.objects.entities.players
{
    class PlayerGates
    {
        /* ALPHA GATE */
        public bool AlphaReady { get; set; }
        public int AlphaWave { get; set; }
        public int AlphaLives { get; set; }
        public int AlphaComplete { get; set; }

        /* BETA GATE */
        public bool BetaReady { get; set; }
        public int BetaWave { get; set; }
        public int BetaLives { get; set; }
        public int BetaComplete { get; set; }

        /* GAMMA GATE */
        public bool GammaReady { get; set; }
        public int GammaWave { get; set; }
        public int GammaLives { get; set; }
        public int GammaComplete { get; set; }

        /* DELTA GATE */
        public bool DeltaReady { get; set; }
        public int DeltaWave { get; set; }
        public int DeltaLives { get; set; }
        public int DeltaComplete { get; set; }

        /* EPSILON GATE */
        public bool EpsilonReady { get; set; }
        public int EpsilonWave { get; set; }
        public int EpsilonLives { get; set; }
        public int EpsilonComplete { get; set; }

        /* ZETA GATE */
        public bool ZetaReady { get; set; }
        public int ZetaWave { get; set; }
        public int ZetaLives { get; set; }
        public int ZetaComplete { get; set; }

        /* KAPPA GATE */
        public bool KappaReady { get; set; }
        public int KappaWave { get; set; }
        public int KappaLives { get; set; }
        public int KappaComplete { get; set; }

        /* KRONOS GATE GATE */
        public bool KronosReady { get; set; }
        public int KronosWave { get; set; }
        public int KronosLives { get; set; }
        public int KronosComplete { get; set; }

        /* LAMBDA GATE GATE */
        public bool LambdaReady { get; set; }
        public int LambdaWave { get; set; }
        public int LambdaLives { get; set; }
        public int LambdaComplete { get; set; }

        /* HADES GATE GATE */
        public bool HadesReady { get; set; }
        public int HadesWave { get; set; }
        public int HadesLives { get; set; }
        public int HadesComplete { get; set; }
        
        public int GetGateRings()
        {
            var i = 0;
            if (AlphaComplete > 0) i += (int)Math.Pow(2, 0);
            if (BetaComplete > 0) i += (int)Math.Pow(2, 1);
            if (GammaComplete > 0) i += (int)Math.Pow(2, 2);
            if (DeltaComplete > 0) i += (int)Math.Pow(2, 3);
            if (EpsilonComplete > 0) i += (int)Math.Pow(2, 4);
            if (ZetaComplete > 0) i += (int)Math.Pow(2, 5);
            if (KronosComplete > 0) i += (int)Math.Pow(2, 0);
            return i;
        }
    }
}