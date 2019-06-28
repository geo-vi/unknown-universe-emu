using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class ClanTagChangedCommand
    {
        public const short ID = 26175;
        public static Command write(string clanTag)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(clanTag);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
