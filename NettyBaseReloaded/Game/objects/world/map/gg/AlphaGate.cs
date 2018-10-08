using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map.gg
{
    class AlphaGate : GalaxyGate
    {
        public AlphaGate(Spacemap coreMap, int wave) : base(coreMap, wave)
        {
        }

        public override Dictionary<int, Wave> Waves { get; }
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void SendWave()
        {
            throw new NotImplementedException();
        }

        public override void End()
        {
            throw new NotImplementedException();
        }
    }
}
