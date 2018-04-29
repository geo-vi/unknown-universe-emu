using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AbilityStatusSingleCommand
    {
        public const short ID = 27007;

        public int abilityTypeId;
        public bool isActivatable;

        public AbilityStatusSingleCommand(int abilityTypeId, bool isActivatable)
        {
            this.abilityTypeId = abilityTypeId;
            this.isActivatable = isActivatable;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(abilityTypeId);
            cmd.Boolean(isActivatable);
            return cmd.Message.ToArray();
        }

        public static Command write(int abilityTypeId, bool isActivatable)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(abilityTypeId);
            cmd.Boolean(isActivatable);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
