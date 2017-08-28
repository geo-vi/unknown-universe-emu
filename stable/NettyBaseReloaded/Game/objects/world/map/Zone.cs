using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Zone
    {
        public int Id { get; }

        public Vector TopLeft { get; set; }
        public Vector BottomRight { get; set; }

        public Zone(int id, Vector topLeft, Vector botRight)
        {
            Id = id;
            TopLeft = topLeft;
            BottomRight = botRight;
        }
    }
}
