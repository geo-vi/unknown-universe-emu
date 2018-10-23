using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class SpaceBallInitializeScoreCommand
    {
        public const short ID = 13691;
        public static Command write(int scoreMars, int scoreEarth, int scoreVenus, int ownerFactionId, int speed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(scoreMars);
            cmd.Integer(scoreEarth);
            cmd.Integer(scoreVenus);
            cmd.Integer(ownerFactionId);
            cmd.Integer(speed);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
