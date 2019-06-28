using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class MineCreateCommand
    {
        public const short ID = 9472;

        public static Command write(string hash, bool pulse, bool param3, int mineType, int y, int x)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(x >> 5 | x << 27);
            cmd.Short(23312);
            cmd.UTF(hash);
            cmd.Integer(y >> 3 | y << 29);
            cmd.Integer(mineType << 1 | mineType >> 31);
            cmd.Boolean(pulse);
            cmd.Boolean(param3);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
