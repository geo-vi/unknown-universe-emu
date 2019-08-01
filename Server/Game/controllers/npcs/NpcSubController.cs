using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.npcs
{
    abstract class NpcSubController : AbstractedSubController
    {
        protected Npc Npc
        {
            get
            {
                var npc = Character as Npc;
                return npc;
            }
        }
    }
}