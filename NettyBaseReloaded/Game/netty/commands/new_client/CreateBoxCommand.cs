using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class CreateBoxCommand
    {
        public const short ID = 20862;

        public static Command write(string type, BoxModule boxModule)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(boxModule.write());
            cmd.UTF(type);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}