using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetInitializationCommand
    {
        public const short ID = 20448;

        public static Command write(bool hasPet, bool hasFuel, bool petIsAlive)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(hasPet);
            cmd.Boolean(hasFuel);
            cmd.Boolean(petIsAlive);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}