using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Mothership : INpc
    {
        private NpcController Controller { get; set; }

        public Mothership(NpcController controller)
        {
            Controller = controller;
        }

        public void Tick()
        {

        }

        public void Active()
        {
            // Create daughters
        }

        public void Inactive()
        {
            // Wait for attack
        }

        public void Paused()
        {
            // Opened but not throwing daughters
        }

        public void Exit()
        {
            // Close itself
        }
    }
}
