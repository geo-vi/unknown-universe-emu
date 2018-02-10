using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class PlayerGates : PlayerBaseClass
    {
        public bool AlphaReady { get; set; }

        public bool BetaReady { get; set; }

        public bool GammaReady { get; set; }

        //TODO: Add the rest

        public bool AlphaComplete { get; set; }

        public bool BetaComplete { get; set; }

        public bool GammaComplete { get; set; }

        public PlayerGates(Player player) : base(player)
        {
            RefreshGates();
        }

        public void Tick()
        {
            if (LastRefresh.AddSeconds(30) < DateTime.Now)
                RefreshGates();
        }

        private DateTime LastRefresh = DateTime.Now;
        private void RefreshGates()
        {
            LastRefresh = DateTime.Now;
        }

        public int CalculateRings()
        {
            return 0;
        }
    }
}
