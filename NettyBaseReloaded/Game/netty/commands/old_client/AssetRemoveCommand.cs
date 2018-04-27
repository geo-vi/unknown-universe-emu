using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AssetRemoveCommand
    {
        public const short ID = 22922;

        public static Command write(AssetTypeModule assetType, int uid)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(assetType.write());
            cmd.Integer(uid);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
