using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class DisplaySettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var reader = new DisplaySettingsRequest();
            reader.readCommand(buffer);

            var displaySettings = gameSession.Player.Settings.GetSettings<DisplaySettings>();
            displaySettings.Unset = false;
            displaySettings.DisplayBoxes = reader.displayBoxes;
            displaySettings.DisplayCargoboxes = reader.displayCargoboxes;
            displaySettings.DisplayDrones = reader.displayDrones;
            displaySettings.DisplayChat = reader.displayChat;
            displaySettings.DisplayNotifications = reader.displayNotifications;
            displaySettings.DisplayResource = reader.displayResources;
            displaySettings.ShipHovering = reader.shipHovering;
            displaySettings.AlwaysDraggableWindows = reader.alwaysDraggableWindows;
            displaySettings.DisplayHitpointBubbles = reader.displayHitpointBubbles;
            displaySettings.DisplayPenaltyCargoboxes = reader.displayPenaltyCargoboxes;
            displaySettings.DisplayPlayerName = reader.displayPlayerName;
            displaySettings.DisplayWindowBackground = reader.displayWindowBackground;
            displaySettings.PreloadUserShips = reader.preloadUserShips;
            displaySettings.UseAutoQuality = reader.useAutoQuality;
            displaySettings.ShowSecondQuickslotBar = reader.showSecondQuickslotBar;
            gameSession.Player.Settings.SaveSettings();
            
            Out.WriteLog("Successfully saved DisplaySettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);
        }
    }
}