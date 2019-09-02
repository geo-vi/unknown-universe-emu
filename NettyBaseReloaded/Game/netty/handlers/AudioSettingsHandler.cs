using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AudioSettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new AudioSettingsRequest();
            cmd.readCommand(buffer);

            var pAudio = gameSession.Player.Settings.OldClientUserSettingsCommand.AudioSettingsModule;

            pAudio.music = cmd.music;
            pAudio.sound = cmd.sound;
            
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
