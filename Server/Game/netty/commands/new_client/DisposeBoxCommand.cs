using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class DisposeBoxCommand
    {
        public const short ID = 24098;

        public static Command write(string hash, bool param2)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(-15321);
            cmd.Boolean(param2);
            cmd.UTF(hash);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
