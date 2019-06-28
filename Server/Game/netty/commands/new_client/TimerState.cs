using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class TimerState
    {
        public const short ID = 8825;

        public static short COOLDOWN = 2;
        public static short READY = 0;
        public static short ACTIVE = 1;

        public short value;

        public TimerState(short value)
        {
            this.value = value;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(value);
            cmd.Short(18232);
            return cmd.Message.ToArray();
        }

    }
}
