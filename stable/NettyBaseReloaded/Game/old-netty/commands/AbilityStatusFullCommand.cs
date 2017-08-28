using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AbilityStatusFullCommand
    {
        public const short ID = 24975;
        public static byte[] write(List<AbilityStatusSingleCommand> abilities)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(abilities.Count);
            foreach (var ability in abilities)
                cmd.AddBytes(ability.write());
            return cmd.ToByteArray();
        }
    }
}
