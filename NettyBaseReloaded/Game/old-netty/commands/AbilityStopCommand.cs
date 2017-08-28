using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AbilityStopCommand
    {
        public const short ID = 27069;

        public static byte[] write(int selectedAbilityId, int activatorId, List<int> targetIds)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(selectedAbilityId);
            cmd.Integer(activatorId);
            cmd.Integer(targetIds.Count);
            foreach (var _loc2_ in targetIds)
            {
                cmd.Integer(_loc2_);
            }
            return cmd.ToByteArray();
        }
    }
}
