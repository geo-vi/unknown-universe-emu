using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestCancelledCommand
    {
        public const short ID = 17478;
        public static Command write(int id)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(id);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
