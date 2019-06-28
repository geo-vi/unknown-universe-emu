using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AmmunitionCountModule
    {
        public const short ID = 14644;

        public AmmunitionTypeModule type;
        public double amount;

        public AmmunitionCountModule(AmmunitionTypeModule type, double amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(type.write());
            cmd.Double(amount);
            return cmd.Message.ToArray();
        }

    }
}