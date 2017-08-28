using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class WindowsCommand
    {
        public const short ID = 12242;
        public static Command write(List<commandKn> slotbars)
        {
            var cmd = new ByteArray(ID);
            cmd.Short(14628);
            cmd.Integer(slotbars.Count);
            foreach (commandKn c in slotbars)
            {
                cmd.AddBytes(c.write());
            }
            cmd.Short(32289);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
