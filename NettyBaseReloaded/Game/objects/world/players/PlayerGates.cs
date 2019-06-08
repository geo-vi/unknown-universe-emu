using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class PlayerGates : PlayerBaseClass
    {
        public bool AlphaReady { get; set; }
        public int AlphaWave { get; set; }
        public int AlphaLives { get; set; }

        public bool BetaReady { get; set; }
        public int BetaWave { get; set; }
        public int BetaLives { get; set; }

        public bool GammaReady { get; set; }
        public int GammaWave { get; set; }
        public int GammaLives { get; set; }

        public bool DeltaReady { get; set; }
        public int DeltaWave { get; set; }
        public int DeltaLives { get; set; }

        public bool EpsilonReady { get; set; }
        public int EpsilonWave { get; set; }
        public int EpsilonLives { get; set; }

        public bool ZetaReady { get; set; }
        public int ZetaWave { get; set; }
        public int ZetaLives { get; set; }

        public bool KappaReady { get; set; }

        public bool KronosReady { get; set; }

        public bool LambdaReady { get; set; }

        public bool HadesReady { get; set; }

        public int AlphaComplete { get; set; }

        public int BetaComplete { get; set; }

        public int GammaComplete { get; set; }

        public int DeltaComplete { get; set; }

        public int EpsilonComplete { get; set; }

        public int ZetaComplete { get; set; }

        public int KappaComplete { get; set; }

        public int KronosComplete { get; set; }

        public int LambdaComplete { get; set; }

        public int HadesComplete { get; set; }
        
        public PlayerGates(Player player) : base(player)
        {            
            World.DatabaseManager.LoadPlayerGates(this);
            RefreshGates();
        }

        public void Tick()
        {
            if (LastRefresh.AddSeconds(30) < DateTime.Now)
            {
                RefreshGates();
            }
        }

        private DateTime LastRefresh = DateTime.Now;
        private void RefreshGates()
        {
            World.DatabaseManager.LoadPlayerGates(this);
            LastRefresh = DateTime.Now;
        }

        public void Save()
        {
            World.DatabaseManager.SaveGalaxyGates(this);
        }
        
        public int CalculateRings()
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

        public int GetWave(int gateID)
        {
            switch ((GalaxyGates)gateID)
            {
                case GalaxyGates.ALPHA:
                    return AlphaWave;
                case GalaxyGates.BETA:
                    return BetaWave;
                case GalaxyGates.GAMMA:
                    return GammaWave;
                case GalaxyGates.DELTA:
                    return DeltaWave;
                case GalaxyGates.EPSILON:
                    return EpsilonWave;
                case GalaxyGates.ZETA:
                    return ZetaWave;
            }

            return 0;
        }

        public void RemoveLife(int gateID)
        {
            switch ((GalaxyGates)gateID)
            {
                case GalaxyGates.ALPHA:
                    if (AlphaLives > 0) AlphaLives--;
                    else
                    {
                        AlphaReady = false;
                    }
                    break;
                case GalaxyGates.BETA:
                    if (BetaLives > 0) BetaLives--;
                    else
                    {
                        BetaReady = false;
                    }
                    break;
                case GalaxyGates.GAMMA:
                    if (GammaLives > 0) GammaLives--;
                    else
                    {
                        GammaReady = false;
                    }
                    break;
                case GalaxyGates.DELTA:
                    if (DeltaLives > 0) DeltaLives--;
                    else
                    {
                        DeltaReady = false;
                    }
                    break;
                case GalaxyGates.EPSILON:
                    if (EpsilonLives > 0) EpsilonLives--;
                    else
                    {
                        EpsilonReady = false;
                    }
                    break;
                case GalaxyGates.ZETA:
                    if (ZetaLives > 0) ZetaLives--;
                    else
                    {
                        ZetaReady = false;
                    }
                    break;
            }
            Save();
        }

        public void PushWave(int gateID, int p)
        {
            switch ((GalaxyGates) gateID)
            {
                case GalaxyGates.ALPHA:
                    AlphaWave += p;
                    break;
                case GalaxyGates.BETA:
                    BetaWave += p;
                    break;
                case GalaxyGates.GAMMA:
                    GammaWave += p;
                    break;
                case GalaxyGates.DELTA:
                    DeltaWave += p;
                    break;
                case GalaxyGates.EPSILON:
                    EpsilonWave += p;
                    break;
                case GalaxyGates.ZETA:
                    ZetaWave += p;
                    break;
            }
        }
    }
}
