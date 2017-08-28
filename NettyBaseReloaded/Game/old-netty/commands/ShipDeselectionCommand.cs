using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipDeselectionCommand
    {
        public const short ID = 6044;

        public static byte[] write()
        {
            var cmd = new ByteArray(ID);
            return cmd.ToByteArray();
        }
    }
}
