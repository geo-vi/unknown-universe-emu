using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Group
    {
        /// <summary>
        /// Group leader
        /// </summary>
        public Player Leader { get; set; }

        /// <summary>
        /// Group members & invites
        /// </summary>
        public Dictionary<int, Player> Members = new Dictionary<int, Player>();
        public Dictionary<int, Player> Invites = new Dictionary<int, Player>();

        public bool LeaderInvitesOnly { get; set; }

        public Group(Player player, Player acceptedPlayer)
        {
            Leader = player;
            Members.Add(acceptedPlayer.Id, acceptedPlayer);
        }

        public void Ping(Vector position)
        {
            
        }

        public Vector Follow(Player player)
        {
            return player.Position;
        }
    }
}
