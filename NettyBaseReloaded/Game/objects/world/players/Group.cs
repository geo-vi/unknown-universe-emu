using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class InvitedForGroup
    {
        public GameSession GameSession { get; }

        public InvitedForGroup(GameSession session)
        {
            GameSession = session;
        }
    }
    class Group
    {
        private enum Invitation_Behavior
        {
            BOSS_ONLY,
            FREE_FOR_ALL
        }

        private enum Loot_Mode
        {
            NONE,
            LOOT_MODE_RANDOM,
            LOOT_MODE_NEED_BEFORE_GREED,
            LOOT_MODE_WYTIWYG
        }

        private class Member
        {
            public Player Player { get; }
            public bool Leader { get; }
            public Member(Player player, bool leader = false)
            {
                Player = player;
                Leader = leader;
            }
        }

        public int Id { get; set; }
        private Dictionary<int, Member> GroupMembers { get; }
        private int LeaderId { get; set; }
        private Invitation_Behavior Behavior = Invitation_Behavior.BOSS_ONLY;
        private Loot_Mode LootMode = Loot_Mode.LOOT_MODE_RANDOM;

        public Group(Player leader, Player member) : this()
        {
            Create(leader, member);
        }

        private Group()
        {
            GroupMembers = new Dictionary<int, Member>();
            LeaderId = 0;
        }

        void Create(Player leader, Player member)
        {
            //Id = leader.Id;

            //GroupMembers.Add(leader.Id, new Member(leader, true));
            //GroupMembers.Add(member.Id, new Member(member));

            //ServerManager.Groups.Add(Id, this);
            //leader.Group = this;
            //member.Group = this;
            //Send(GroupMembers[leader.Id], PacketBuilder.LegacyModule("0|ps|inv|del|none|" + member.Id));
            //Send(GroupMembers[member.Id], PacketBuilder.LegacyModule("0|ps|inv|del|none|" + member.Id + "|" + leader.Id));

            //SendToAll(PacketBuilder.LegacyModule(GetBaseString().Append(CreateGroupString()).ToString()));

            //Update(member);
            //Update(leader);
        }

        private const char SPLIT = '|';

        StringBuilder GetBaseString()
        {
            StringBuilder baseString = new StringBuilder();
            baseString.Append("0|ps|init|grp");
            baseString.Append(SPLIT);
            baseString.Append(GroupMembers.Count); // Current Size
            baseString.Append(SPLIT);
            baseString.Append(5); // Max Size
            baseString.Append(SPLIT);
            baseString.Append((int)Behavior);
            baseString.Append(SPLIT);
            baseString.Append((int)LootMode);
            baseString.Append(SPLIT);
            return baseString;
        }

        StringBuilder CreateGroupString()
        {
            StringBuilder baseString = new StringBuilder();
            foreach (var member in GroupMembers)
            {
                //var player = member.Value.Player;
                //baseString.Append(member.Key);
                //baseString.Append(SPLIT);
                //baseString.Append(Base64.Base64Encode(player.Name));
                //baseString.Append(SPLIT);
                //baseString.Append(player.CurrentHealth);
                //baseString.Append(SPLIT);
                //baseString.Append(player.MaxHealth);
                //baseString.Append(SPLIT);
                //baseString.Append(player.CurrentNanoHull);
                //baseString.Append(SPLIT);
                //baseString.Append(player.MaxNanoHull);
                //baseString.Append(SPLIT);
                //baseString.Append(player.CurrentShield);
                //baseString.Append(SPLIT);
                //baseString.Append(player.MaxShield);
                //baseString.Append(SPLIT);
                //baseString.Append(player.Spacemap.Id);
                //baseString.Append(SPLIT);
                //baseString.Append(player.Position.x);
                //baseString.Append(SPLIT);
                //baseString.Append(player.Position.y);
                //baseString.Append(SPLIT);
                //baseString.Append(player.Level);
                //baseString.Append(SPLIT);
                //baseString.Append(ToNumber.Boolean(true)); //active
                //baseString.Append(SPLIT);
                //baseString.Append(ToNumber.Boolean(player.Controller.Invisible));
                //baseString.Append(SPLIT);
                //baseString.Append(ToNumber.Boolean(player.Controller.Attacking));
                //baseString.Append(SPLIT);
                //baseString.Append((int)player.FactionId);
                //baseString.Append(SPLIT);
                //baseString.Append(player.GetTargetId());
                //baseString.Append(SPLIT);
                //baseString.Append(player.GetClanTag());
                //baseString.Append(SPLIT);
                //baseString.Append(player.Hangar.Ship.Id);
                //baseString.Append(SPLIT);
                //baseString.Append(ToNumber.Boolean(player.Controller.StopController));
                //baseString.Append(SPLIT);
                //baseString.Append(ToNumber.Boolean(member.Value.Leader));
                //baseString.Append(SPLIT);
            }
            Console.WriteLine(baseString.ToString());
            return baseString;
        }

        public void Join(Player player)
        {
            throw new NotImplementedException();
        }

        public void Invite(Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        public void Kick(Player targetPlayer)
        {
            throw new NotImplementedException();
        }

        public void Ping(Vector targetLocation)
        {
            throw new NotImplementedException();
        }

        public Vector Follow(Player groupMember)
        {
            throw new NotImplementedException();
        }

        public void Update(Player targetPlayer)
        {
            // TODO: Use XML Creator to form the packet.
            //<1 hp=\"" + hp + "\" hpM=\"" + hpMax + "\" nh=\"" + nanohull + "\" nhM=\"" + nanomax + "\" sh=\"" + shd + "\" shM=\"" + shdmax + "\" pos=\"" + x + "," + y + "\" map=\"" + mapid + "\" lev=\"" + level + "\" fra=\"" + factionID + "\" act=\"" + activity + "\" clk=\"" + isCloaked + "\" shp=\"" + ship + "\" fgt=\"" + fightState + "\" lgo=\"" + isOffline + "\" tgt=\"" + targetObject + "\"></1>
            //Send(GroupMembers[targetPlayer.Id], PacketBuilder.LegacyModule("0|ps|upd|" + targetPlayer.Id + "|<1 hp=\"" + targetPlayer.CurrentHealth + "\" hpM=\"" + targetPlayer.MaxHealth + "\" nh=\"" + targetPlayer.CurrentNanoHull + "\" nhM=\"" + targetPlayer.MaxNanoHull + "\" sh=\"" + targetPlayer.CurrentShield + "\" shM=\"" + targetPlayer.MaxShield + "\" pos=\"" + targetPlayer.Position.x + "," + targetPlayer.Position.y + "\" map=\"" + targetPlayer.Spacemap.Id + "\" lev=\"" + targetPlayer.Level.Id + "\" fra=\"" + targetPlayer.FactionId + "\" act=\"" + ToNumber.Boolean(true) + "\" clk=\"" + ToNumber.Boolean(!targetPlayer.Controller.Invisible) + "\" shp=\"" + targetPlayer.Hangar.Ship.Id + "\" fgt=\"" + ToNumber.Boolean(!(targetPlayer.Controller.Attacking)) + "\" lgo=\"" + ToNumber.Boolean(!targetPlayer.Controller.StopController) + "\" tgt=\"" + targetPlayer.GetTargetId() + "\"></1>"));
        }

        void Send(Member member, byte[] bytes)
        {
            World.StorageManager.GetGameSession(member.Player.Id).Client.Send(bytes);
        }

        void SendToAll(byte[] bytes)
        {
            foreach (var member in GroupMembers)
            {
                World.StorageManager.GetGameSession(member.Key).Client.Send(bytes);
            }
        }
    }
}
