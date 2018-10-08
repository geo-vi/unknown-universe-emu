using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AttributeOreCountUpdateCommand
    {
        public const short ID = 10112;

        public static Command write(List<OreCountModule> ores)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(ores.Count);
            foreach (var ore in ores)
                cmd.AddBytes(ore.write());
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
