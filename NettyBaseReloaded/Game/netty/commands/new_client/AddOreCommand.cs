using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class AddOreCommand
    {
        public const short ID = 13463;

        public static Command write(BoxModule boxModule, OreTypeModule oreType)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(boxModule.write());
            cmd.AddBytes(oreType.write());
            cmd.Short(25679);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}