using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class PetGearSelectCommand
    {
        public const short ID = 19459;

        public static byte[] write(PetGearTypeModule gearType, List<int> optParams)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(gearType.write());
            cmd.Integer(optParams.Count);
            foreach (var param in optParams)
            {
                cmd.Integer(param);
            }
            return cmd.ToByteArray();
        }
    }
}
