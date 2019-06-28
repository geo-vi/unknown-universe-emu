using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class LegacyModule
    {
        public const short ID = 29052;

        public string message;
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            message = parser.readUTF();
        }

        public static Command write(string message)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(message);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
