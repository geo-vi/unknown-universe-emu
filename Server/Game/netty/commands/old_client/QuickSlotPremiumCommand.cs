using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class QuickSlotPremiumCommand
    {
        public const short ID = 31908;

        public static Command write(bool active)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(active);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
