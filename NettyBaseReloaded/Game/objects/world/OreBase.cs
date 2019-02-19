using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world
{
    abstract class OreBase
    {
        public int Prometium { get; set; }
        public int Endurium { get; set; }
        public int Terbium { get; set; }
        public int Prometid { get; set; }
        public int Duranium { get; set; }
        public int Xenomit { get; set; }
        public int Promerium { get; set; }
        public int Seprom { get; set; }
        public int Palladium { get; set; }

        public OreBase(int Prometium, int Endurium,
                        int Terbium, int Prometid, int Duranium, int Xenomit, int Promerium, int Seprom, int Palladium)
        {
            this.Prometium = Prometium;
            this.Endurium = Endurium;
            this.Terbium = Terbium;
            this.Prometid = Prometid;
            this.Duranium = Duranium;
            this.Xenomit = Xenomit;
            this.Promerium = Promerium;
            this.Seprom = Seprom;
            this.Palladium = Palladium;
        }
    }
}
