using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AttributeShieldUpdateCommand
    {
        public const short ID = 5107;

        public static Command write(int currentShield, int maxShield)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(maxShield << 5 | maxShield >> 27);
            cmd.Integer(currentShield >> 11 | currentShield << 21);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}