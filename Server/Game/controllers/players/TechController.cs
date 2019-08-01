using System.Collections.Concurrent;
using Server.Game.objects.entities.players;
using Server.Game.objects.enums;

namespace Server.Game.controllers.players
{
    class TechController : PlayerSubController
    {
        public ConcurrentBag<Tech> ActiveTechs = new ConcurrentBag<Tech>();

        public TechController()
        {
            
        }

        public void Execute(Techs techType)
        {
            
        }

        public void Stop(Techs techType)
        {
            
        }

        public void AddTech(Techs tech, int addedCount = 1)
        {
            
        }

        public void RemoveTech(Techs tech, int removedCount = 1)
        {
            
        }
    }
}
