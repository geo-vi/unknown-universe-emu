using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class KillScreenUpdateCommand
    {
        public const short ID = 9383;

        public static Command write(List<KillScreenOptionModule> options)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(options.Count);
            foreach (var option in options)
            {
                cmd.AddBytes(option.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
