using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class AttributeShipSpeedUpdateCommand
    {
        public const short ID = 1506;

        public static Command write(int speed, int realSpeed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(speed >> 15 | speed << 17);
            cmd.Short(-20132);
            cmd.Short(22708);
            cmd.Integer(realSpeed << 6 | realSpeed >> 26);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}