using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class ShipWarpModule
    {
        public const short ID = 2113;

        public int shipId;
        public string shipLootId;
        public string shipDesignName;
        public int uridiumPrice;
        public int voucherPrice;
        public int hangarId;
        public string hangarName;

        public ShipWarpModule(int shipId, string shipLootId, string shipDesignName, int uridiumPrice, int voucherPrice, int hangarId, string hangarName)
        {
            this.shipId = shipId;
            this.shipLootId = shipLootId;
            this.shipDesignName = shipDesignName;
            this.uridiumPrice = uridiumPrice;
            this.voucherPrice = voucherPrice;
            this.hangarId = hangarId;
            this.hangarName = hangarName;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(this.shipLootId);
            cmd.Integer(this.shipId >> 8 | this.shipId << 24);
            cmd.Integer(this.uridiumPrice >> 10 | this.uridiumPrice << 22);
            cmd.Integer(this.voucherPrice >> 6 | this.voucherPrice << 26);
            cmd.UTF(this.hangarName);
            cmd.Integer(this.hangarId << 12 | this.hangarId >> 20);
            cmd.UTF(this.shipDesignName);
            return cmd.Message.ToArray();
        }
    }
}