using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AbilityStopCommand
    {
        public const short ID = 27069;

        public static Command write(int selectedAbilityId ,int activatorId, List<int> targetIds)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(selectedAbilityId);
            cmd.Integer(activatorId);
            cmd.Integer(targetIds.Count);
            foreach (var target in targetIds)
                cmd.Integer(target);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
