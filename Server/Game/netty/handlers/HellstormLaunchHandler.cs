using DotNetty.Buffers;

namespace Server.Game.netty.handlers
{
    class HellstormLaunchHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.RocketLauncher != null)
                gameSession.Player.Controller.Attack.LaunchRocketLauncher();
        }
    }
}
