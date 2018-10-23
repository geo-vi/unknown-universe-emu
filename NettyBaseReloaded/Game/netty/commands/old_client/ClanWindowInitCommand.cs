using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class ClanWindowInitCommand
    {
        public const short ID = 16080;

        public static Command write(int clanId, string clanName, string clanTag, List<ClanMemberModule> clanMembers, List<int> clanMembersOnMyMap)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(clanId);
            cmd.UTF(clanName);
            cmd.UTF(clanTag);
            cmd.Integer(clanMembers.Count);
            foreach (var clanMember in clanMembers)
            {
                cmd.AddBytes(clanMember.write());
            }
            cmd.Integer(clanMembersOnMyMap.Count);
            foreach (var clanMember in clanMembersOnMyMap)
            {
                cmd.Integer(clanMember);
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
