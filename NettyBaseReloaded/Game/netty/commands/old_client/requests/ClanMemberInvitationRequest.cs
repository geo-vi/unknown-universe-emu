using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class ClanMemberInvitationRequest
    {
        public const short ID = 24022;

        public int inviterId;

        public int candidateId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            inviterId = parser.readInt();
            candidateId = parser.readInt();
        }
    }
}
