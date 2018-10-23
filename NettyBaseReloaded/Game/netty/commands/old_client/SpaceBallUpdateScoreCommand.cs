using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class SpaceBallUpdateScoreCommand
    {
        public const short ID = 25318;

        public static Command write(int factionId, int score, int gate)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(factionId);
            cmd.Integer(score);
            cmd.Integer(gate);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
