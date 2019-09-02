using System;
using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;

namespace Server.Game.netty.handlers
{
    class SelectBatteryHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new SelectBatteryRequest();
            request.readCommand(buffer);

            var batteryLootId = AmmoConvertManager.AmmoTypeToString(request.batteryType.type);
            gameSession.Player.Settings.GetSettings<SlotbarSettings>().SelectedLaserAmmo = batteryLootId;
            gameSession.Player.Settings.SaveSettings();
            
            gameSession.Player.OnLaserAmmoChange(batteryLootId);
        }
    }
}