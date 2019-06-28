using DotNetty.Buffers;
using Server.Utils;

namespace Server.Game.netty.commands.old_client.requests
{
    public class PetRequest
    {
        public const short LAUNCH = 0;

        public const short DEACTIVATE = 1;

        public const short TOGGLE_ACTIVATION = 2;

        public const short HOTKEY_GUARD_MODE = 3;

        public const short REPAIR_DESTROYED_PET = 4;

        public const short HOTKEY_REPAIR_SHIP = 5;

        public const short ID = 552;

        public short petRequestType = 0;
        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            petRequestType = parser.readShort();
        }
    }
}