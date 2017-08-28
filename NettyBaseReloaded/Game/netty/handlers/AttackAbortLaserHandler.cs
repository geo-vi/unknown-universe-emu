using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackAbortLaserHandler : IHandler, ILegacyHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            gameSession.Player.Controller.Attacking = false;
        }

        public void execute(GameSession gameSession, string[] packet)
        {
            gameSession.Player.Controller.Attacking = false;
        }
    }
}