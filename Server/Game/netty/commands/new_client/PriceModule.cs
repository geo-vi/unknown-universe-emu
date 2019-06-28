using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class PriceModule
    {
        public const short ID = 6467;

        public const short CREDITS = 0;
        public const short URIDIUM = 1;

        public short type = 0;
        public short amount = 0;

        public PriceModule(short type, short amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(this.type);
            cmd.Integer(this.amount >> 11 | this.amount << 21);
            cmd.Short(7603);
            return cmd.Message.ToArray();
        }

    }
}