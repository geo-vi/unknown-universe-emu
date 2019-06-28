using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
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
