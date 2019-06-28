using DotNetty.Buffers;
using Server.Utils;
using Debug = System.Diagnostics.Debug;

namespace Server.Game.netty.commands.old_client.requests
{
    class VersionRequest
    {
        public const short ID = 666;
        
        public int playerId;
        public string sessionId;

        public void readCommand(IByteBuffer bytes)
        {
            Debug.WriteLine("Reading command");
            var parser = new ByteParser(bytes);
            playerId = parser.readInt();
            Debug.WriteLine("PlayerID: " + playerId);
            sessionId = parser.readUTF();
            Debug.WriteLine("SessionID: " + sessionId);
        }
    }
}
