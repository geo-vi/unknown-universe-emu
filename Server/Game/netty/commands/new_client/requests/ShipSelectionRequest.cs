using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.new_client.requests
{
    class ShipSelectionRequest
    {
        public const short ID = 12642;

        public int selectedId = 0;
        public int selectedX = 0;
        public int selectedY = 0;
        public int x = 0;
        public int y = 0;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            this.selectedId = parser.readInt();
            this.selectedId = (int)(((uint)this.selectedId << 2) | ((uint)this.selectedId >> 30));
            this.selectedY = parser.readInt();
            this.selectedY = (int)(((uint)this.selectedY >> 15) | ((uint)this.selectedY << 17));
            this.y = parser.readInt();
            this.y = (int)(((uint)this.y << 2) | ((uint)this.y >> 30));
            this.selectedX = parser.readInt();
            this.selectedX = (int)(((uint)this.selectedX >> 5) | ((uint)this.selectedX << 27));
            this.x = parser.readInt();
            this.x = (int)(((uint)this.x >> 2) | ((uint)this.x << 30));
        }
    }
}
