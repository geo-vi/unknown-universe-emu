using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AssetInfoCommand
    {
        public const short ID = 22884;

        public static Command write(int assetId, AssetTypeModule type, int designId, int expansionStage, int hp, int maxHp, bool shielded, int shield, int maxShield)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(assetId);
            cmd.AddBytes(type.write());
            cmd.Integer(designId);
            cmd.Integer(expansionStage);
            cmd.Integer(hp);
            cmd.Integer(maxHp);
            cmd.Boolean(shielded);
            cmd.Integer(shield);
            cmd.Integer(maxShield);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
