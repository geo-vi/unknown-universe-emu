using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class KillScreenPostCommand
    {
        public const short ID = 4800;

        public static Command write(string killerName, string killerEpppLink, string playerShipAlias, DestructionTypeModule cause, List<KillScreenOptionModule> options)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(killerName);
            cmd.UTF(killerEpppLink);
            cmd.UTF(playerShipAlias);
            cmd.AddBytes(cause.write());
            cmd.Integer(options.Count);
            foreach (var option in options)
            {
                cmd.AddBytes(option.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
