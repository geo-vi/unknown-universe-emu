using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ForceInitHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            var id = int.Parse(param[1]);
            Character targetShip = null;
            if (!gameSession.Player.Spacemap.Entities.TryGetValue(id, out targetShip) && id == gameSession.Player.Id) return;
            if (targetShip != null && targetShip.InRange(gameSession.Player))
            {
                gameSession.Player.Range.RemoveEntity(targetShip);
                gameSession.Player.Controller.Checkers.Tick();
            }
        }
    }
}