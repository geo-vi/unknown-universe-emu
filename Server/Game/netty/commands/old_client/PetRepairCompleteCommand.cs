using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetRepairCompleteCommand
    {
        public const short ID = 11351;

        public static Command write()
        {
            var cmd = new ByteArray(ID);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
