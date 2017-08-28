using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AttributeOreCountUpdateCommand
    {
        public const short ID = 10112;

        public static byte[] write(List<OreCountModule> oreCountList)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(oreCountList.Count);
            foreach (var loc in oreCountList)
            {
                cmd.AddBytes(loc.write());
            }
            return cmd.ToByteArray();
        }
    }
}
