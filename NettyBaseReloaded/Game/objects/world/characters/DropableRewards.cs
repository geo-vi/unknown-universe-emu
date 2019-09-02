using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class DropableRewards : OreBase
    {
        public DropableRewards(int Prometium, int Endurium,
            int Terbium, int Prometid, int Duranium, int Xenomit, int Promerium, int Seprom, int Palladium):base(Prometium, Endurium, Terbium, Prometid, Duranium, Xenomit, Promerium, Seprom, Palladium)
        {
        }

        public bool Empty => Prometium + Endurium + Terbium + Prometid + Duranium + Promerium + Seprom + Palladium <= 0;

        public DropableRewards Multiply(double p)
        {
            return new DropableRewards((int)(Prometium * p), (int)(Endurium * p), (int)(Terbium * p), (int)(Prometid * p), (int)(Duranium * p), (int)(Xenomit * p),
                (int)(Promerium * p), (int)(Seprom * p), (int)(Palladium * p));
        }
    }
}
