using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.new_client.requests
{
    class LoginRequest
    {
        public const short ID = 21821;

        public int factionID;
        public string version;
        public int instanceId;
        public int playerId;
        public string sessionId;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            this.instanceId = parser.readInt();
            this.instanceId = (int)(((uint)this.instanceId >> 6) | ((uint)this.instanceId << 26));
            this.factionID = parser.readShort();
            this.playerId = parser.readInt();
            this.playerId = (int)(((uint)this.playerId << 3) | ((uint)this.playerId >> 29));
            this.version = parser.readUTF();
            this.sessionId = parser.readUTF();
        }
    }
}
