using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class VersionRequest
    {
        public const short ID = 666;

        public int playerId;
        public string sessionId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            playerId = parser.readInt();
            sessionId = parser.readUTF();
        }
    }
}
