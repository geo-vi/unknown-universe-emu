using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class UILayerAdvertisementCommand
    {
        public static Command write(AlignmentModule alignment, bool closeable, bool moveable, string advertisementKey)
        {
            var cmd = new ByteArray();
            cmd.AddBytes(alignment.write());
            cmd.Boolean(closeable);
            cmd.Boolean(moveable);
            cmd.UTF(advertisementKey);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
