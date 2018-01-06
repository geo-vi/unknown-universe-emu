using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Aggressive : INpc
    {
        private NpcController Controller { get; set; }

        public Aggressive(NpcController controller)
        {
            Controller = controller;
            Controller.Checkers.VisibilityRange = -1;
        }

        public void Tick()
        {

        }

        public void Active()
        {
            // Circle & attack the player
        }

        public void Inactive()
        {
            // Look for player / players
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            // No more players on the map -> Self destroy
        }
    }
}
