using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class AttackHitCommand
    {
        public const short ID = 27342;

        public static byte[] write(AttackTypeModule attackType, int attackerId, int victimId, int victimHitpoints, int victimShield, int victimNanohull,
            int damage, bool skilled)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(attackType.write());
            cmd.Integer(attackerId);
            cmd.Integer(victimId);
            cmd.Integer(victimHitpoints);
            cmd.Integer(victimShield);
            cmd.Integer(victimNanohull);
            cmd.Integer(damage);
            cmd.Boolean(skilled);
            return cmd.ToByteArray();
        }
    }
}
