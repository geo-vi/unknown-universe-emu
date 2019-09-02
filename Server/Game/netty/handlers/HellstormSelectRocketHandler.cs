using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class HellstormSelectRocketHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var request = new HellstormSelectRocketRequest();
            request.readCommand(buffer);
            var ammo = AmmoConverter.AmmoTypeToString(request.rocketType.type);
            gameSession.Player.RocketLauncher?.ChangeLoad(ammo);
        }
    }
}
