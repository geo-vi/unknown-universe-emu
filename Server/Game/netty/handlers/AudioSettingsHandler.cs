using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.handlers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
