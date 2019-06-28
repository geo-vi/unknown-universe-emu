using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class commandX35
    {
        public const short ID = 26385;
        public static Command write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(-30552);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
