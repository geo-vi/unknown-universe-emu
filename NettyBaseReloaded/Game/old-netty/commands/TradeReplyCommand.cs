using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class TradeReplyCommand
    {
        public const short ID = 27640;

        public static byte[] write(List<OrePriceModule> priceInfos)
        {
            ByteArray cmd= new ByteArray(ID);
            cmd.Integer(priceInfos.Count);
            foreach (var ore in priceInfos)
            {
                cmd.AddBytes(ore.write());
            }
            return cmd.ToByteArray();
        }
    }
}
