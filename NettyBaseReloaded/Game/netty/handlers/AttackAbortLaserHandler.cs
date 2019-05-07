using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class AttackAbortLaserHandler : IHandler, ILegacyHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            gameSession.Player.Controller.Attack.Attacking = false;
        }

        public void execute(GameSession gameSession, string[] packet)
        {
            gameSession.Player.Controller.Attack.Attacking = false;
        }
    }
}