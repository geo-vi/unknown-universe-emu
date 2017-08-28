using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.commands
{
    class MapChangeRequest
    {
        public const short ID = 8;

        public static byte[] write(int newMapId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(newMapId);
            return cmd.ToByteArray();
        }
    }
}
