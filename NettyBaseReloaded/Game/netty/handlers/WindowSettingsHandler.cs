using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class WindowSettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var playerWindowSettings = gameSession.Player.Settings.OldClientUserSettingsCommand.WindowSettingsModule;
            var cmd = new WindowSettingsRequest();
            cmd.readCommand(buffer);

            playerWindowSettings.clientResolutionId = cmd.clientResolutionId;
            playerWindowSettings.barStatus = cmd.barStatus;
            playerWindowSettings.mainmenuPosition = cmd.mainmenuPosition;
            playerWindowSettings.minmapScale = cmd.minimapScale;
            playerWindowSettings.notSet = false;
            playerWindowSettings.resizableWindows = cmd.resizableWindows;
            playerWindowSettings.slotMenuOrder = cmd.slotMenuOrder;
            playerWindowSettings.slotMenuPremiumOrder = cmd.slotMenuPremiumOrder;
            playerWindowSettings.slotmenuPosition = cmd.slotmenuPosition;
            playerWindowSettings.slotmenuPremiumPosition = cmd.slotmenuPremiumPosition;
            playerWindowSettings.windowSettings = cmd.windowSettings;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}
