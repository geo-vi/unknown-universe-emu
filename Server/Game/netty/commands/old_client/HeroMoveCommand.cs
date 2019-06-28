using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class HeroMoveCommand
    {
        public const short ID = 24000;
        public static Command write(int x, int y)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x);
            cmd.Integer(y);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
