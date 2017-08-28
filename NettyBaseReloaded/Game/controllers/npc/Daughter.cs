using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Daughter : INpc
    {
        private NpcController Controller { get; set; }

        public Daughter(NpcController controller)
        {
            Controller = controller;
        }

        public void Tick()
        {

        }

        public void Active()
        {
            // Start attacking
        }

        public void Inactive()
        {
            // Waiting for someone to approach mothership
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            // Remove from map
        }

    }
}
