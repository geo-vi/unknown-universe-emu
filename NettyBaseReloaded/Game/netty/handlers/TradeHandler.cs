using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class TradeHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient)
            {
                return;
            }

            var orePrices = World.StorageManager.OrePrices;

            if (gameSession.Player.State.InTradeArea)
            {
                Packet.Builder.OrePriceCommand(gameSession, orePrices.Prometium, orePrices.Endurium, orePrices.Terbium, orePrices.Prometid, orePrices.Duranium, orePrices.Promerium, 0);
                Packet.Builder.TradeWindowActivationCommand(gameSession);
            }
        }
    }
}
