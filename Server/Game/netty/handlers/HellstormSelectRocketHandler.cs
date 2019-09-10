using DotNetty.Buffers;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;

namespace Server.Game.netty.handlers
{
    class HellstormSelectRocketHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var player = gameSession.Player;
            var request = new HellstormSelectRocketRequest();
            request.readCommand(buffer);

            var slotbarSettings = player.Settings.GetSettings<SlotbarSettings>();
            
            var ammoLootId = AmmoConvertManager.AmmoTypeToString(request.rocketType.type);

            slotbarSettings.SelectedHellstormRocketAmmo = ammoLootId;
            
            player.Settings.SaveSettings();

            player.RocketLauncher.SetLoad(ammoLootId);
        }
    }
}
