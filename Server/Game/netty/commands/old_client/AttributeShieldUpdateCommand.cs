using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AttributeShieldUpdateCommand
    {
        public const short ID = 21243;

        public static Command write(int shieldNow, int shieldMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(shieldNow);
            cmd.Integer(shieldMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}