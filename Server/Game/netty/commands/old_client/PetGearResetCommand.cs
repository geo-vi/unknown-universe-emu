using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetGearResetCommand
    {
        public const short ID = 9267;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
