using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class KillScreenOptionTypeModule
    {
        public const short ID = 24278;

        public const short NEAREST_BASE = 1;

        public static short PAYMENT_LINK = 9;

        public static short HADES_FREE_REPAIR = 7;

        public static short varN3e = 6;

        public static short SPOT = 3;

        public static short JUMP_GATE = 2;

        public static short varj4R = 5;

        public static short varVD = 0;

        public static short FREE_REPAIR_SECS = 4;

        public static short TDM_FREE_REPAIR = 8;

        public short varR46;

        public KillScreenOptionTypeModule(short varR46)
        {
            this.varR46 = varR46;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(varR46);
            cmd.Short(-25050);
            return cmd.Message.ToArray();
        }
    }
}