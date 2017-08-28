using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AbilityStartCommand
    {
        public const short ID = 25358;

        public static byte[] write(int selectedAbilityId, int activatorId, bool noStopCommand)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(selectedAbilityId);
            cmd.Integer(activatorId);
            cmd.Boolean(noStopCommand);
            return cmd.ToByteArray();
        }
    }
}
