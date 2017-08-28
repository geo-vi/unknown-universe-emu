using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class HealCommand
    {
        public const short HITPOINTS = 0;
        public const short SHIELD = 1;

        public const short ID = 9679;

        public static byte[] write(short healType, int healerId, int healedId, int currentHitpoints, int healAmount)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(healType);
            cmd.Integer(healerId);
            cmd.Integer(healedId);
            cmd.Integer(currentHitpoints);
            cmd.Integer(healAmount);
            return cmd.ToByteArray();
        }
    }
}
