using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class HellstormAttackCommand
    {
        public const short ID = 24152;

        public static byte[] write(int attackerId, int targetId, bool hit, int currentLoad, AttackTypeModule rocketType)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(attackerId);
            cmd.Integer(targetId);
            cmd.Boolean(hit);
            cmd.Integer(currentLoad);
            cmd.AddBytes(rocketType.write());
            return cmd.ToByteArray();
        }
    }
}
