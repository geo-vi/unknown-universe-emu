using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class UserSettingsCommand
    {
        public const short ID = 11343;

        public QualitySettingsModule QualitySettingsModule { get; set; }
        public DisplaySettingsModule DisplaySettingsModule { get; set; }
        public AudioSettingsModule AudioSettingsModule { get; set; }
        public WindowSettingsModule WindowSettingsModule { get; set; }
        public GameplaySettingsModule GameplaySettingsModule { get; set; }

        public UserSettingsCommand(QualitySettingsModule qualitySettingsModule, DisplaySettingsModule displaySettingsModule, AudioSettingsModule audioSettingsModule,
            WindowSettingsModule windowSettingsModule, GameplaySettingsModule gameplaySettingsModule)
        {
            QualitySettingsModule = qualitySettingsModule;
            DisplaySettingsModule = displaySettingsModule;
            AudioSettingsModule = audioSettingsModule;
            WindowSettingsModule = windowSettingsModule;
            GameplaySettingsModule = gameplaySettingsModule;
        }

        public Command write()
        {
            return write(QualitySettingsModule, DisplaySettingsModule, AudioSettingsModule, WindowSettingsModule,
                GameplaySettingsModule);
        }

        public static Command write(QualitySettingsModule qualitySettingsModule, DisplaySettingsModule displaySettingsModule, AudioSettingsModule audioSettingsModule,
            WindowSettingsModule windowSettingsModule, GameplaySettingsModule gameplaySettingsModule)
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(qualitySettingsModule.write());
            cmd.AddBytes(displaySettingsModule.write());
            cmd.AddBytes(audioSettingsModule.write());
            cmd.AddBytes(windowSettingsModule.write());
            cmd.AddBytes(gameplaySettingsModule.write());
            return new Command(cmd.ToByteArray(), false);
        }

    }
}
