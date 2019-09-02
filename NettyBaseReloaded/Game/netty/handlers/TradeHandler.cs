using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.netty.handlers
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
