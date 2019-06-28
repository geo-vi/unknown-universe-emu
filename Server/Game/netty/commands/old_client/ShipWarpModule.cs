using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ShipWarpModule
    {
        public const short ID = 26497;

        public int shipId;
        public int shipTypeId;
        public string shipDesignName;
        public int uridiumPrice;
        public int voucherPrice;
        public int hangarId;
        public string hangarName;

        public ShipWarpModule(int shipId, int shipTypeId, string shipDesignName, int uridiumPrice, int voucherPrice, int hangarId, string hangarName)
        {
            this.shipId = shipId;
            this.shipTypeId = shipTypeId;
            this.shipDesignName = shipDesignName;
            this.uridiumPrice = uridiumPrice;
            this.voucherPrice = voucherPrice;
            this.hangarId = hangarId;
            this.hangarName = hangarName;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(shipId);
            cmd.Integer(shipTypeId);
            cmd.UTF(shipDesignName);
            cmd.Integer(uridiumPrice);
            cmd.Integer(voucherPrice);
            cmd.Integer(hangarId);
            cmd.UTF(hangarName);
            return cmd.Message.ToArray();
        }

    }
}