using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AttributeHitpointUpdateCommand
    {
        public const short ID = 8638;

        public static byte[] write(int hitpointsNow, int hitpointsMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(hitpointsNow);
            cmd.Integer(hitpointsMax);
            return cmd.ToByteArray();
        }
    }
}
