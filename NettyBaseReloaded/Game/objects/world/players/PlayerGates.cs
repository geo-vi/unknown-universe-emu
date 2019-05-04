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

        public bool BetaReady { get; set; }

        public int BetaWave { get; set; }

        public bool GammaReady { get; set; }

        public int GammaWave { get; set; }

        public bool DeltaReady { get; set; }

        public bool EpsilonReady { get; set; }

        public bool ZetaReady { get; set; }

        public bool KappaReady { get; set; }

        public bool KronosReady { get; set; }

        public int AlphaComplete { get; set; }

        public int BetaComplete { get; set; }

        public int GammaComplete { get; set; }

        public int DeltaComplete { get; set; }

        public int EpsilonComplete { get; set; }

        public int ZetaComplete { get; set; }

        public int KappaComplete { get; set; }

        public int KronosComplete { get; set; }

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

        public int GetAlphaWave()
        {
            return AlphaWave;
        }
    }
}
