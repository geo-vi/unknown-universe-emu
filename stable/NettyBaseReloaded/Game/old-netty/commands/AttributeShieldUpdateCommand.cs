using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AttributeShieldUpdateCommand
    {
        public const short ID = 21243;

        public static byte[] write(int shieldNow, int shieldMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(shieldNow);
            cmd.Integer(shieldMax);
            return cmd.ToByteArray();
        }
    }
}
