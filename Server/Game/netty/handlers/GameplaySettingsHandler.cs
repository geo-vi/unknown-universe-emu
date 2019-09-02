using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class GameplaySettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var reader = new GameplaySettingsRequest();
            reader.readCommand(buffer);

            var gameplaySettings = gameSession.Player.Settings.GetSettings<GameplaySettings>();
            gameplaySettings.Unset = false;
            gameplaySettings.AutoBoost = reader.autoBoost;
            gameplaySettings.AutoRefinement = reader.autoRefinement;
            gameplaySettings.AutoStart = reader.autoStart;
            gameplaySettings.DoubleclickAttack = reader.doubleclickAttack;
            gameplaySettings.AutoChangeAmmo = reader.autoChangeAmmo;
            gameplaySettings.QuickslotStopAttack = reader.quickslotStopAttack;
            gameplaySettings.AutoBuyGreenBootyKeys = reader.autoBuyGreenBootyKeys;
            gameSession.Player.Settings.SaveSettings();
            
            Out.WriteLog("Successfully saved GameplaySettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);

        }
    }
}