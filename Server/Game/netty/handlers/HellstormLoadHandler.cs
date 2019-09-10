using DotNetty.Buffers;
using Server.Game.controllers.characters;
using Server.Game.controllers.players;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    class HellstormLoadHandler : IHandler
    {
        public void Execute(GameSession gameSession, IByteBuffer buffer)
        {
            gameSession.Player.Controller.GetInstance<PlayerRocketLauncherController>().StartLoading();
        }
    }
}
