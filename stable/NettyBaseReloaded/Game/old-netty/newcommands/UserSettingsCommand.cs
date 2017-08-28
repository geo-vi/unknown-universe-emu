using NettyBaseReloaded.Game.netty.newcommands.settingsModules;

namespace NettyBaseReloaded.Game.netty.newcommands
{
	class UserSettingsCommand : SimpleCommand
	{
        public static int ID = 19507;

        public UserSettingsCommand(QualitySettingsModule qs, AudioSettingsModule asm, WindowSettingsModule ws, GameplaySettingsModule gs, QuestSettingsModule z9, DisplaySettingsModule ds)
        {
            writeShort(ID);
            qs.write();
            writeBytes(qs.command.ToArray());
            asm.write();
            writeBytes(asm.command.ToArray());
            writeShort(365);
            writeShort(25913);
            ws.write();
            writeBytes(ws.command.ToArray());
            gs.write();
            writeBytes(gs.command.ToArray());
            z9.write();
            writeBytes(z9.command.ToArray());
            ds.write();
            writeBytes(ds.command.ToArray());
        }
    }
}
