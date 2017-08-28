using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class EquipReadyCommand
    {
        public const short ID = 31370;

        public static byte[] write(bool ready)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(ready);
            return cmd.ToByteArray();
        }
    }
}
