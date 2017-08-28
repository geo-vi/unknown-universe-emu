using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AttributeShipSpeedUpdateCommand
    {
        public const short ID = 3657;

        public static Command write(int newSpeed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(newSpeed);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}