using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class PetHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            var types = parser.Short();
            switch (types)
            {
                case PetRequest.LAUNCH:
                    gameSession.Player.Pet.Controller.Activate();
                    break;
                case PetRequest.DEACTIVATE:
                    gameSession.Player.Pet.Controller.DeActivate();
                    break;
                case PetRequest.REPAIR_DESTROYED_PET:
                    gameSession.Player.Pet.Controller.Repair();
                    break;
            }
        }
    }
}
