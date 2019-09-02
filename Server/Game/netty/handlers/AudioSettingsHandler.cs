using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class AudioSettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var reader = new AudioSettingsRequest();
            reader.readCommand(buffer);

            var audioSettings = gameSession.Player.Settings.GetSettings<AudioSettings>();
            audioSettings.Unset = false;
            audioSettings.Music = reader.music;
            audioSettings.Sound = reader.sound;
            gameSession.Player.Settings.SaveSettings();
            
            Out.WriteLog("Successfully saved AudioSettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);

        }
    }
}