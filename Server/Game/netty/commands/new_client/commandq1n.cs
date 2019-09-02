using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class commandq1n
    {
        public const short ID = 23240;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
