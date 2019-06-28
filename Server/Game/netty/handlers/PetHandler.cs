using System;
using DotNetty.Buffers;
using Server.Game.netty.commands.old_client.requests;

namespace Server.Game.netty.handlers
{
    class PetHandler : IHandler
    {
        public void execute(GameSession gameSession, IByteBuffer buffer)
        {
            var petRequest = new PetRequest();
            petRequest.readCommand(buffer);

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