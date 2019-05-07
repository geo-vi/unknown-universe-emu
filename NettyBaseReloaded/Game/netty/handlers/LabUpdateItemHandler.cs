using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.informations;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class LabUpdateItemHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            if (gameSession.Player.UsingNewClient)
            {
                return;
            }
            
            var request = new LabUpdateItemRequest();
            request.readCommand(buffer);

            var ore = (Ores) request.updateWith.oreType.typeValue;
            var amount = (int)request.updateWith.count;
            if (amount < 0)
            {
                // BAN.EXE
                Packet.Builder.LegacyModule(gameSession, "0|A|STD|How is the feeling of sitting on the 3rd leg?");
                return;
            }
            switch (request.itemToUpdate.itemValue)
            {
                case LabItemModule.LASER:
                    if (CheckOre(gameSession.Player.Information.Cargo, ore, amount))
                    {
                        gameSession.Player.Skylab.UpgradeLaser(ore, amount);
                        gameSession.Player.Information.Cargo.ReduceOre(ore, amount);
                    }
                    break;
                case LabItemModule.ROCKETS:
                    if (CheckOre(gameSession.Player.Information.Cargo, ore, amount))
                    {
                        gameSession.Player.Skylab.UpgradeRockets(ore, amount);
                        gameSession.Player.Information.Cargo.ReduceOre(ore, amount);
                    }
                    break;
                case LabItemModule.DRIVING:
                    if (CheckOre(gameSession.Player.Information.Cargo, ore, amount))
                    {
                        gameSession.Player.Skylab.UpgradeGenerators(ore, amount);
                        gameSession.Player.Information.Cargo.ReduceOre(ore, amount);
                    }
                    break;
                case LabItemModule.SHIELD:
                    if (CheckOre(gameSession.Player.Information.Cargo, ore, amount))
                    {
                        gameSession.Player.Skylab.UpgradeShields(ore, amount);
                        gameSession.Player.Information.Cargo.ReduceOre(ore, amount);
                    }
                    break;
            }

            Packet.Builder.LabUpdateItemCommand(gameSession, gameSession.Player.Skylab);
        }

        public bool CheckOre(Cargo cargo, Ores ore, int amount)
        {
            switch (ore)
            {
                case Ores.DURANIUM:
                    if (cargo.Duranium < amount) return false;
                    return true;
                case Ores.PROMETID:
                    if (cargo.Prometid < amount) return false;
                    return true;
                case Ores.PROMERIUM:
                    if (cargo.Promerium < amount) return false;
                    return true;
                case Ores.SEPROM:
                    if (cargo.Seprom < amount) return false;
                    return true;
            }

            return false;
        }
    }
}
