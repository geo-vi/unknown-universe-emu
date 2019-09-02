using DotNetty.Buffers;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    class AttackAbortLaserHandler : ILegacyHandler
    {
        public void Execute(GameSession gameSession, string[] packet)
        {
        }
    }
}