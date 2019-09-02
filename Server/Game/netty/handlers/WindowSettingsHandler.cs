using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.netty.packet;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class WindowSettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var reader = new WindowSettingsRequest();
            reader.readCommand(buffer);

            var windowSettings = gameSession.Player.Settings.GetSettings<WindowSettings>();
            windowSettings.Unset = false;
            windowSettings.BarStatus = reader.barStatus;
            windowSettings.MainmenuPosition = reader.mainmenuPosition;
            windowSettings.MinimapScale = reader.minimapScale;
            windowSettings.SlotmenuPosition = reader.slotmenuPosition;
            windowSettings.ClientResolutionId = reader.clientResolutionId;
            windowSettings.ResizableWindowsString = reader.resizableWindows;
            windowSettings.SlotMenuOrder = reader.slotMenuOrder;
            windowSettings.SlotmenuPremiumPosition = reader.slotmenuPremiumPosition;
            windowSettings.WindowSettingsString = reader.windowSettings;
            windowSettings.SlotMenuPremiumOrder = reader.slotMenuPremiumOrder;
            gameSession.Player.Settings.SaveSettings();
            
            Out.WriteLog("Successfully saved WindowSettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);

        }
    }
}