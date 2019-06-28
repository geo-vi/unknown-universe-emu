using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class EventActivationStateCommand
    {
        public const short FROSTED_GATES = 0;

        public const short CHRISTMAS_TREES = 1;

        public const short CARNIVAL_2013 = 2;

        public const short APRIL_FOOLS = 3;

        public const short ID = 30631;
        public static Command write(short type, bool active)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            cmd.Boolean(active);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
