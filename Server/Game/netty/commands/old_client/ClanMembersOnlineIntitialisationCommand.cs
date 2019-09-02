using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class ClanMembersOnlineIntitialisationCommand
    {
        public const short ID = 2104;

        public static Command write(List<int> clanMembersOnline)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(clanMembersOnline.Count);
            foreach (var clanMember in clanMembersOnline)
            {
                cmd.Integer(clanMember);
            }
            return new Command(cmd.ToByteArray(), false);
        }

    }
}
