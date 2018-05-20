using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestCompletedCommand
    {
        public const short ID = 4127;

        public static Command write(int id, int rating)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(id);
            cmd.Integer(rating);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
