using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    public class BoxModule
    {
        public const short ID = 27942;

        public string hash;
        public int x;
        public int y;

        public BoxModule(string hash, int x, int y)
        {
            this.hash = hash;
            this.x = x;
            this.y = y;
        }

        public byte[] write()
        {
            var cmd = new ByteArray();
            cmd.Integer(this.x >> 5 | this.x << 27);
            cmd.Short(23312);
            cmd.UTF(this.hash);
            cmd.Integer(this.y >> 3 | this.y << 29);
            return cmd.Message.ToArray();
        }
    }
}