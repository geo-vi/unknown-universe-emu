using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ClanMemberLeftCommand
    {
        public const short ID = 10954;
        public static Command write(int userId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
