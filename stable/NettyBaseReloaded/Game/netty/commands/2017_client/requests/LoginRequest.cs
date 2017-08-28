using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client.requests
{
    class LoginRequest
    {
        public const short ID = 12666;

        public int factionID;
        public string version;
        public int instanceId;
        public int playerId;
        public string sessionId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.playerId = parser.readInt();
            this.playerId = this.playerId >> 4 | this.playerId << 28;
            this.version = parser.readUTF();
            this.instanceId = parser.readInt();
            this.instanceId = this.instanceId << 15 | this.instanceId >> 17;
            this.sessionId = parser.readUTF();
            this.factionID = parser.readShort();
        }
    }
}
