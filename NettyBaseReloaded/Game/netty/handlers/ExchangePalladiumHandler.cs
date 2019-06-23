using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ExchangePalladiumHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            if (gameSession == null || gameSession.Player.Information.Cargo.Palladium < World.StorageManager.OrePrices.Palladium) return;
            var player = gameSession.Player;

            var valueExchanged = Math.Abs(Convert.ToInt32(param[1]));
            if (player.Information.Cargo.Palladium - valueExchanged >= 0)
            {
                var rate = World.StorageManager.OrePrices.Palladium;
                var ggEnergy = valueExchanged / rate;
                var cargo = player.Information.Cargo;
                cargo.Palladium -= valueExchanged;

                Packet.Builder.AttributeOreCountUpdateCommand(gameSession, cargo);
                World.DatabaseManager.AddGGEnergy(player, ggEnergy);
                Packet.Builder.LegacyModule(gameSession, "0|A|STM|pricetag_extra-energy_compact|%VALUE%|" + ggEnergy);
                World.DatabaseManager.SaveCargo(player, cargo);
            }
        }
    }
}
