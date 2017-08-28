using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloadedController.Main.global.objects
{
    class Player : Character
    {
        public int RankId { get; set; }

        public int FactionId { get; set; }

        public Player(int id, string name, int rankId, int factionId, Spacemap spacemap, Vector pos)
            : base(id, name, spacemap, pos)
        {
            RankId = rankId;
            FactionId = factionId;
        }
    }
}
