using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipWarpModule
    {
        public const short ID = 26497;

        public int shipId { get; set; }
        public int shipTypeId { get; set; }
        public string shipDesignName { get; set; }
        public int uridiumPrice { get; set; }
        public int voucherPrice { get; set; }
        public int hangarId { get; set; }
        public string hangarName { get; set; }

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
