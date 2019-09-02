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
