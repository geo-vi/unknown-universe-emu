using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using System;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class PetHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var petRequest = new PetRequest();
            petRequest.readCommand(bytes);

            try
            {
                switch (petRequest.petRequestType)
                {
                    case PetRequest.LAUNCH:
                        gameSession.Player.Pet.Controller.Activate();
                        break;
                    case PetRequest.DEACTIVATE:
                        gameSession.Player.Pet.Controller.Deactivate();
                        break;
                    case PetRequest.REPAIR_DESTROYED_PET:
                        gameSession.Player.Pet.Controller.Repair();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.StackTrace);
            }
        }
    }
}