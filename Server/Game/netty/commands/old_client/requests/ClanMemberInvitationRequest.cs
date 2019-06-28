using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class ClanMemberInvitationRequest
    {
        public const short ID = 24022;

        public int inviterId;

        public int candidateId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            inviterId = parser.readInt();
            candidateId = parser.readInt();
        }
    }
}
