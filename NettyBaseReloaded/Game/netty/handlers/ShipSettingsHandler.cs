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
