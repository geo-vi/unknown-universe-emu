using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class EquipReadyCommand
    {
        public const short ID = 31370;

        public static Command write(bool ready)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(ready);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
