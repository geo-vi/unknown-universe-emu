using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class CollectableModule
    {
        public const short ID = 27942;

        public static byte[] write(string hash, int x, int y)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x >> 5 | x << 27);
            cmd.Short(23312);
            cmd.UTF(hash);
            cmd.Integer(y >> 3 | y << 29);
            return cmd.ToByteArray();
        }
    }
}
