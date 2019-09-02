using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.netty.commands.old_client.requests;
using Server.Game.objects;
using Server.Game.objects.entities.players.settings;

namespace Server.Game.netty.handlers
{
    class SelectRocketHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new SelectRocketRequest();
            request.readCommand(buffer);

            var rocketLootId = AmmoConvertManager.AmmoTypeToString(request.rocketType.type);
            gameSession.Player.Settings.GetSettings<SlotbarSettings>().SelectedRocketAmmo = rocketLootId;
            gameSession.Player.Settings.SaveSettings();
        }
    }
}