using System.Collections.Generic;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class KillScreenPostCommand
    {
        public const short ID = 5565;

        public static Command write(string killerName, string killerEpppLink, string playerShipAlias, DestructionTypeModule cause, List<KillScreenOptionModule> options)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(options.Count);
            foreach (var c in options)
            {
                cmd.AddBytes(c.write());
            }
            cmd.UTF(killerEpppLink);
            cmd.Short(-22236);
            cmd.Short(5954);
            cmd.UTF(playerShipAlias);
            cmd.AddBytes(cause.write());
            cmd.UTF(killerName);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}