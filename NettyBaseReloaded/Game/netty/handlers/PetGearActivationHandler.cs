using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.pets;
using PetGearTypeModule = NettyBaseReloaded.Game.netty.commands.old_client.PetGearTypeModule;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class PetGearActivationHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var cmd = new PetGearActivationRequest();
            cmd.readCommand(buffer);

            gameSession.Player.Pet?.Controller?.SwitchGear((GearType)cmd.gearTypeToActivate.typeValue, cmd.optParam);
        }
    }
}
