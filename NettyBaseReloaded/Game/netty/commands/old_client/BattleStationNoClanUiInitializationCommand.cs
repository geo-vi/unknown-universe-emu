using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class BattleStationNoClanUiInitializationCommand
    {
        public const short ID = 28532;

        public static Command write(int mapAssetId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapAssetId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
