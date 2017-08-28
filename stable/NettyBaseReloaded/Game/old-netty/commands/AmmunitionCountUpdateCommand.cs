using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AmmunitionCountUpdateCommand
    {
        public const short ID = 7158;

        public static byte[] write(List<AmmunitionCountModule> ammunitionItems)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(ammunitionItems.Count);
            foreach (var item in ammunitionItems)
            {
                cmd.AddBytes(item.write());
            }
            return cmd.ToByteArray();
        }
    }
}
