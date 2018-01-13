using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class KillScreenUpdateCommand
    {
        public const short ID = 9383;

        public static Command write(List<KillScreenOptionModule> options)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(options.Count);
            foreach (var option in options)
            {
                cmd.AddBytes(option.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
