using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class PriceModule
    {
        public const short CREDITS = 0;

        public const short URIDIUM = 1;

        public const short ID = 4446;

        public short type;

        public int amount;

        public PriceModule(short type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(type);
            cmd.Integer(amount);
            return cmd.Message.ToArray();
        }
    }
}
