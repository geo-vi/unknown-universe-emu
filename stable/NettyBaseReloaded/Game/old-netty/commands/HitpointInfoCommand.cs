using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class HitpointInfoCommand
    {
        public const short ID = 30056;

        public static byte[] write(int hitpoints, int hitpointsMax, int nanoHull, int nanoHullMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(hitpoints);
            cmd.Integer(hitpointsMax);
            cmd.Integer(nanoHull);
            cmd.Integer(nanoHullMax);
            return cmd.ToByteArray();
        }
    }
}
