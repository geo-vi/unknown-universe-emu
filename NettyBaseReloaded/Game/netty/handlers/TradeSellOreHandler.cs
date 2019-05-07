using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.map.collectables;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class TradeSellOreHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                return;
            }

            var request = new TradeSellOreRequest();
            request.readCommand(buffer);

            var oreType = (OreTypes) request.toSell.oreType.typeValue;
            var cargo = gameSession.Player.Information.Cargo;
            var count = Convert.ToInt32(Math.Abs(request.toSell.count));
            if (count <= 0) return;
            var cred = gameSession.Player.Information.Credits;
            var value = 0;
            switch (oreType)
            {
                case OreTypes.PROMETIUM:
                    if (count > cargo.Prometium)
                    {
                        return;
                    }
                    cargo.Prometium -= count;
                    value = count * World.StorageManager.OrePrices.Prometium;
                    break;
                case OreTypes.ENDURIUM:
                    if (count > cargo.Endurium)
                    {
                        return;
                    }
                    cargo.Endurium -= count;
                    value = count * World.StorageManager.OrePrices.Endurium;
                    break;
                case OreTypes.TERBIUM:
                    if (count > cargo.Terbium)
                    {
                        return;
                    }
                    cargo.Terbium -= count;
                    value = count * World.StorageManager.OrePrices.Terbium;
                    break;
                case OreTypes.PROMETID:
                    if (count > cargo.Prometid)
                    {
                        return;
                    }
                    cargo.Prometid -= count;
                    value = count * World.StorageManager.OrePrices.Terbium;
                    break;
                case OreTypes.DURANIUM:
                    if (count > cargo.Duranium)
                    {
                        return;
                    }
                    cargo.Duranium -= count;
                    value = count * World.StorageManager.OrePrices.Duranium;
                    break;
                case OreTypes.PROMERIUM:
                    if (count > cargo.Promerium)
                    {
                        return;
                    }
                    cargo.Promerium -= count;
                    value = count * World.StorageManager.OrePrices.Promerium;
                    break;
            }
            Packet.Builder.AttributeOreCountUpdateCommand(gameSession, cargo);
            cred.Add(value);
            Packet.Builder.LegacyModule(gameSession, "0|A|STD|Sold " + request.toSell.count + " " + oreType.ToString() + " for " + value + " C.");
            World.DatabaseManager.SaveCargo(gameSession.Player, cargo);
        }
    }
}
