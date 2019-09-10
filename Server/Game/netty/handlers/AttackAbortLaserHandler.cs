using System;
using DotNetty.Buffers;
using Server.Game.managers;
using Server.Game.objects;

namespace Server.Game.netty.handlers
{
    class AttackAbortLaserHandler : ILegacyHandler
    {
        public void Execute(GameSession gameSession, string[] packet)
        {
            CombatManager.Instance.CancelCombat(gameSession.Player);
        }
    }
}