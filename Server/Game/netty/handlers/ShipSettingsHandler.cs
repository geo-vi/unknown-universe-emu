using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.netty.handlers
{
    class ShipSettingsHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new ShipSettingsRequest();
            request.readCommand(buffer);

            var slotbarSettings = gameSession.Player.Settings.GetSettings<SlotbarSettings>();

            slotbarSettings.QuickbarSlots = request.quickbarSlots;
            slotbarSettings.QuickbarSlotsPremium = request.quickbarSlotsPremium;
            slotbarSettings.SelectedHellstormRocketAmmo = AmmoConvertManager.GetRocketLootId(request.selectedHellstormRocket);
            slotbarSettings.SelectedLaserAmmo = AmmoConvertManager.GetLaserLootId(request.selectedLaser);
            slotbarSettings.SelectedRocketAmmo = AmmoConvertManager.GetRocketLootId(request.selectedRocket);

            gameSession.Player.Settings.SaveSettings();

            Out.WriteLog("Successfully saved ShipSettings for Player", LogKeys.PLAYER_LOG, gameSession.Player.Id);
        }
    }
}
