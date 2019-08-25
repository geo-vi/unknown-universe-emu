using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class QualitySettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var reader = new QualitySettingsRequest();
            reader.readCommand(buffer);

            var qualitySettings = gameSession.Player.Settings.GetSettings<QualitySettings>();
            qualitySettings.Unset = false;
            qualitySettings.QualityAttack = reader.qualityAttack;
            qualitySettings.QualityBackground = reader.qualityBackground;
            qualitySettings.QualityCollectables = reader.qualityCollectables;
            qualitySettings.QualityCustomized = reader.qualityCustomized;
            qualitySettings.QualityEffect = reader.qualityEffect;
            qualitySettings.QualityEngine = reader.qualityEngine;
            qualitySettings.QualityExplosion = reader.qualityExplosion;
            qualitySettings.QualityPresetting = reader.qualityPresetting;
            qualitySettings.QualityShip = reader.qualityShip;
            gameSession.Player.Settings.SaveSettings();
            
            Out.WriteLog("Successfully saved QualitySettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);
        }
    }
}