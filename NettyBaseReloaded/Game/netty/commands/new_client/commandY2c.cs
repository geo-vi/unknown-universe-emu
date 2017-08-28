using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    public class commandY2c
    {
        public const short ID = 18926;

        public static byte[] write(int id, int type, int param3, int posX, int posY)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(posX >> 12 | posX << 20);
            cmd.Integer(posY << 4 | posY >> 28);
            cmd.Integer(type << 11 | type >> 21);
            cmd.Integer(id >> 3 | id << 29);
            cmd.Integer(param3 << 16 | param3 >> 16);
            cmd.Short(-26519);
            return cmd.ToByteArray();
        }
    }
}