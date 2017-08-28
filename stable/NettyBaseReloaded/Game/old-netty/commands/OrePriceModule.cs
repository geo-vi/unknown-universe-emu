using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class OrePriceModule
    {
        public const short ID = 3363;
        public OreTypeModule type;
        public int price = 0;

        public OrePriceModule(OreTypeModule type, int price)
        {
            this.type = type;
            this.price = price;
        }

        public byte[] write()
        {
            ByteArray cmd = new ByteArray(ID);
            cmd.AddBytes(type.write());
            cmd.Integer(price);
            return cmd.Message.ToArray();
        }
    }
}
