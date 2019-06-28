using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
