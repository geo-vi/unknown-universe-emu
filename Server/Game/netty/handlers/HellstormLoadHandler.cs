using DotNetty.Buffers;

namespace Server.Game.netty.handlers
{
    class HellstormLoadHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var player = gameSession.Player;
            if (player.RocketLauncher != null)
                player.RocketLauncher.Loading = true;
        }
    }
}
