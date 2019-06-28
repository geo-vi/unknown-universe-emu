using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;

namespace Server.Game.netty.handlers
{
    class TradeHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                return;
            }

            var orePrices = World.StorageManager.OrePrices;

            if (gameSession.Player.State.InTradeArea || gameSession.Player.Extras.Any(x => x.Value is TradeDrone))
            {
                var palladiumPrice = gameSession.Player.Spacemap.Id == 92 ? 15 : -1;
                Packet.Builder.OrePriceCommand(gameSession, orePrices.Prometium, orePrices.Endurium, orePrices.Terbium, orePrices.Prometid, orePrices.Duranium, orePrices.Promerium, palladiumPrice);
                Packet.Builder.TradeWindowActivationCommand(gameSession);
            }
        }
    }
}
