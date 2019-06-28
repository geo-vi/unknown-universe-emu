using Server.Utils;

namespace Server.Game.netty.commands.old_client
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
