using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.npc
{
    abstract class NpcSubController : AbstractedSubController
    {
        protected Npc Npc;
        
        protected NpcSubController(Npc npc) : base(npc)
        {
            Npc = npc;
        }
    }
}