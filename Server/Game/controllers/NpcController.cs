using Server.Game.objects.entities;

namespace Server.Game.controllers
{
    class NpcController : AbstractCharacterController
    {
        private Npc Npc;
        
        public NpcController(Npc npc) : base(npc)
        {
            Npc = npc;
        }
        
        /// <summary>
        /// Default route that the NPC will follow during it's lifespan
        /// </summary>
        public void CreateRoutes()
        {

        }
    }
}
