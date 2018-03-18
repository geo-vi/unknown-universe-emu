using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class MapAssetAddBillboardCommand
    {
        public const short ID = 9049;
        public static Command write(string hash, AssetTypeModule type, PartnerTypeModule partnerType, int x, int y, int uid)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(hash);
            cmd.AddBytes(type.write());
            cmd.AddBytes(partnerType.write());
            cmd.Integer(x);
            cmd.Integer(y);
            cmd.Integer(uid);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
