using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class PetHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var petRequest = new PetRequest();
            petRequest.readCommand(bytes);

            switch (petRequest.petRequestType)
            {
                case PetRequest.LAUNCH:
                    gameSession.Player.Pet.Controller.Activate();
                    break;
                case PetRequest.DEACTIVATE:
                    //gameSession.Player.Pet.Controller.DeActivate();
                    break;
                case PetRequest.REPAIR_DESTROYED_PET:
                    //gameSession.Player.Pet.Controller.Repair();
                    break;
            }
        }
    }
}