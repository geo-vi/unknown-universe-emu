using System;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty;
using Server.Game.netty.commands.new_client;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class LabRefinementHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient) return;

            var request = new LabRefinementRequest();
            request.readCommand(buffer);

            var prometium = gameSession.Player.Information.Cargo.Prometium;
            var endurium = gameSession.Player.Information.Cargo.Endurium;
            var terbium = gameSession.Player.Information.Cargo.Terbium;
            var prometid = gameSession.Player.Information.Cargo.Prometid;
            var duranium = gameSession.Player.Information.Cargo.Duranium;
            var xeno = gameSession.Player.Information.Cargo.Xenomit;

            var count = Math.Abs(request.toProduce.count);
            switch (request.toProduce.oreType.typeValue)
            {
                case OreTypeModule.PROMERIUM: // orange
                    if (count > xeno || count * 10 > prometid || count * 10 > duranium) return;
                    gameSession.Player.Information.Cargo.Xenomit -= (int)count;
                    gameSession.Player.Information.Cargo.Prometid -= (int) count * 10;
                    gameSession.Player.Information.Cargo.Duranium -= (int) count * 10;
                    gameSession.Player.Information.Cargo.Promerium += (int) count;
                    break;
                case OreTypeModule.PROMETID: // pink
                    if (count * 20 > prometium || count * 10 > endurium) return;
                    gameSession.Player.Information.Cargo.Prometium -= (int)count * 20;
                    gameSession.Player.Information.Cargo.Endurium -= (int) count * 10;
                    gameSession.Player.Information.Cargo.Prometid += (int) count;
                    break;
                case OreTypeModule.DURANIUM: // green
                    if (count * 20 > terbium || count * 10 > endurium) return;
                    gameSession.Player.Information.Cargo.Terbium -= (int)count * 20;
                    gameSession.Player.Information.Cargo.Endurium -= (int)count * 10;
                    gameSession.Player.Information.Cargo.Duranium += (int) count;
                    break;
            }

            Packet.Builder.AttributeOreCountUpdateCommand(gameSession, gameSession.Player.Information.Cargo);
        }
    }
}
