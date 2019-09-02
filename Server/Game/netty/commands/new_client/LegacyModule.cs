using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class LegacyModule
    {
        public const short ID = 32601;

        public static Command write(string message)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(message);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
