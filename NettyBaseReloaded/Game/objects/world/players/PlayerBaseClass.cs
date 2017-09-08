using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class PlayerBaseClass
    {
        /// <summary>
        /// Base class for all the /players/ classes
        /// </summary>

        internal Player Player { get; }

        public PlayerBaseClass(Player player)
        {
            Player = player;
        }
    }
}
