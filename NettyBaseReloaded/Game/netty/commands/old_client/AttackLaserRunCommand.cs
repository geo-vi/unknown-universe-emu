using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AttackLaserRunCommand
    {
        public const short ID = 30791;
        public static Command write(int attackerId, int targetId, int laserColor, bool isDiminishedBySkillShield, bool skilledLaser)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(attackerId);
            cmd.Integer(targetId);
            cmd.Integer(laserColor);
            cmd.Boolean(isDiminishedBySkillShield);
            cmd.Boolean(skilledLaser);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
