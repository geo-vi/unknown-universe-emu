using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers
{
    class NpcController : AbstractCharacterController
    {
        private Npc _npc;
        
        public NpcController(Npc npc) : base(npc)
        {
            _npc = npc;
        }

        protected override void CreateControllers()
        {
            //create custom ones
        }

        /// <summary>
        /// Default route that the NPC will follow during it's lifespan
        /// </summary>
        public void CreateRoutes()
        {

        }
    }
}
