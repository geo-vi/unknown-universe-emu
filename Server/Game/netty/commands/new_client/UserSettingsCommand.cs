using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class UserSettingsCommand
    {
        public const short ID = 19507;

        public static Command write(QualitySettingsModule qs, AudioSettingsModule asm, WindowSettingsModule ws, GameplaySettingsModule gs, QuestSettingsModule z9, DisplaySettingsModule ds)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(qs.write());
            cmd.AddBytes(asm.write());
            cmd.Short(365);
            cmd.Short(25913);
            cmd.AddBytes(ws.write());
            cmd.AddBytes(gs.write());
            cmd.AddBytes(z9.write());
            cmd.AddBytes(ds.write());
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
