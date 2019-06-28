using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PetFuelUpdateCommand
    {
        public const short ID = 22079;

        public static Command write(int petFuelNow, int petFuelMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(petFuelNow);
            cmd.Integer(petFuelMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
