using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ForceInitHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            var id = int.Parse(param[1]);
            var targetShip = gameSession.Player.Spacemap.Entities[id];
            if (targetShip != null && targetShip.InRange(gameSession.Player))
                Packet.Builder.ShipCreateCommand(gameSession, targetShip);
        }
    }
}