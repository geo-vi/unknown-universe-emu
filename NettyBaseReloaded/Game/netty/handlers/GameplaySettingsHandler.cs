using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class GameplaySettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new GameplaySettingsRequest();
            cmd.readCommand(buffer);
            var gameplaySettings = gameSession.Player.Settings.OldClientUserSettingsCommand.GameplaySettingsModule;
            gameplaySettings.autoBoost = cmd.autoBoost;
            gameplaySettings.autoBuyGreenBootyKeys = cmd.autoBuyGreenBootyKeys;
            gameplaySettings.autoChangeAmmo = cmd.autoChangeAmmo;
            gameplaySettings.autoRefinement = cmd.autoRefinement;
            gameplaySettings.autoStart = cmd.autoStart;
            gameplaySettings.doubleclickAttack = cmd.doubleclickAttack;
            gameplaySettings.notSet = false;
            gameplaySettings.quickslotStopAttack = cmd.quickslotStopAttack;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
