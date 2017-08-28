using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class MapAssetActionAvailableCommand
    {
        public const short OFF = 1;
        public const short ON = 0;

        public const short ID = 12696;

        public static Command write(int param1, short state, bool activatable, ClientUITooltip toolTip, commandu1C param5)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(param5.write());
            cmd.Integer(param1 << 13 | param1  >> 19);
            cmd.Boolean(activatable);
            cmd.Short(state);
            cmd.AddBytes(toolTip.write());
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
