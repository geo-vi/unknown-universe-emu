using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetDeactivationCommand
    {
        public const short ID = 2419;

        public static Command write(int petId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
