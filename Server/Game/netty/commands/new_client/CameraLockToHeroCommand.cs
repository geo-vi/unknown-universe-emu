using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class CameraLockToHeroCommand
    {
        public const short ID = 21062;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
