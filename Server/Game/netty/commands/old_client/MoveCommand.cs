using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class MoveCommand
    {
        public const short ID = 20502;

        public static Command write(int userId, int x, int y, int timeToTarget)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(timeToTarget);
            return new Command(cmd.ToByteArray(), false);
        }

    }
}
