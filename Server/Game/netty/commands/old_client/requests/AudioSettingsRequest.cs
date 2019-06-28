using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    class AudioSettingsRequest
    {
        public const short ID = 7694;

        public bool sound = false;

        public bool music = false;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            this.sound = cmd.readBool();
            this.music = cmd.readBool();
        }
    }
}
