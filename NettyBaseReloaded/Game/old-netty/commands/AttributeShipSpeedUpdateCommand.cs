using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AttributeShipSpeedUpdateCommand
    {
        public const short ID = 3657;

        public static byte[] write(int newSpeed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(newSpeed);
            return cmd.ToByteArray();
        }
    }
}
