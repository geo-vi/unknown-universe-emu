using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class UserSettingsCommand
    {
        public const short ID = 11343;

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
