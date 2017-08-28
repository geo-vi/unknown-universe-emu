using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    public class AttackLaserRequest
    {
        public const short ID = 19306;

        public int selectedId;
        public int x;
        public int y;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            y = parser.readInt();
            y = (int)(((uint)y << 6) | ((uint)y >> 26));
            selectedId = parser.readInt();
            selectedId = (int)(((uint)selectedId >> 16) | ((uint)selectedId << 16));
            x = parser.readInt();
            x = (int)(((uint)x << 7) | ((uint)x >> 25));
        }
    }
}