using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuickSlotPremiumCommand
    {
        public const short ID = 31908;

        public static Command write(bool active)
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(active);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
