using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Group
    {
        public const int LOOT_MODE_RANDOM = 1;
      
        public const int LOOT_MODE_NEED_BEFORE_GREED = 2;

        /// <summary>
        /// What You Think Is What You Get
        /// </summary>
        public const int LOOT_MODE_WYTIWYG = 3;

        public const int DEFAULT_MAX_GROUP_SIZE = 7;

        public int Id { get; }
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

        public int LootMode { get; set; }

        public Group(Player player, Player acceptedPlayer)
        {
            Id = FindId();
            LootMode = 2;

            Leader = player;
            AddToGroup(Leader);
            Members.Add(acceptedPlayer.Id, acceptedPlayer);
            AddToGroup(acceptedPlayer);

            Packet.Builder.GroupInitializationCommand(player.GetGameSession());
            Packet.Builder.GroupInitializationCommand(acceptedPlayer.GetGameSession());
        }

        private int FindId()
        {
            var id = World.StorageManager.Groups.FirstOrDefault(x => x.Value == null).Key;
            World.StorageManager.Groups.Add(id, this);
            return id;
        }

        private void AddToGroup(Player player)
        {
            player.Group = this;
        }

        public void Tick()
        {
            
        }

        public void Ping(Vector position)
        {
            
        }

        public Vector Follow(Player player)
        {
            return player.Position;
        }

        public void Invite(Player player)
        {
            
        }

        public void RevokeInvitation(Player player)
        {
            
        }

        public void Accept(Player acceptedPlayer)
        {
            //Members.Add(acceptedPlayer.Id, acceptedPlayer);
            //AddToGroup(acceptedPlayer);
            //Packet.Builder.GroupInitializationCommand(acceptedPlayer.GetGameSession());
        }

        public void Reject(Player player)
        {
            
        }

        public void Kick(Player player)
        {
            
        }

        public void Destroy()
        {
            World.StorageManager.Groups.Remove(Id);
        }
    }
}
