using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class commandu1C
    {
        public const short ID = 23492;

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            return cmd.Message.ToArray();
        }
    }
}
