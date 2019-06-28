using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class AttackHitAssetCommand
    {
        public const short ID = 10080;

        public static Command write(int assetId, int hitpointsNow, int hitpointsMax)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(assetId);
            cmd.Integer(hitpointsNow);
            cmd.Integer(hitpointsMax);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
