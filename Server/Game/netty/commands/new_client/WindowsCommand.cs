using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
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
