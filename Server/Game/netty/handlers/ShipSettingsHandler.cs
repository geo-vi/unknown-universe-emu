using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class ShipSettingsHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new ShipSettingsRequest();
            request.readCommand(buffer);

            var slotbarSettings = gameSession.Player.Settings.OldClientShipSettingsCommand;

            slotbarSettings.quickbarSlots = request.quickbarSlots;
            slotbarSettings.quickbarSlotsPremium = request.quickbarSlotsPremium;
            slotbarSettings.selectedHellstormRocket = request.selectedHellstormRocket;
            slotbarSettings.selectedLaser = request.selectedLaser;
            slotbarSettings.selectedRocket = request.selectedRocket;

            gameSession.Player.Settings.SaveSettings();

        }
    }
}
