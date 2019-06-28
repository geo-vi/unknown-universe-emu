using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class QualitySettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new QualitySettingsRequest();
            cmd.readCommand(buffer);

            var pQuality = gameSession.Player.Settings.OldClientUserSettingsCommand.QualitySettingsModule;

            pQuality.qualityAttack = cmd.qualityAttack;
            pQuality.qualityBackground = cmd.qualityBackground;
            pQuality.qualityCollectables = cmd.qualityCollectables;
            pQuality.qualityCustomized = cmd.qualityCustomized;
            pQuality.qualityEffect = cmd.qualityEffect;
            pQuality.qualityEngine = cmd.qualityEngine;
            pQuality.qualityExplosion = cmd.qualityExplosion;
            pQuality.qualityPOIzone = cmd.qualityPOIzone;
            pQuality.qualityPresetting = cmd.qualityPresetting;
            pQuality.qualityShip = cmd.qualityShip;

            gameSession.Player.Settings.SaveSettings();
        }
    }
}
