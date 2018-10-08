using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Spaceball : INpc
    {
        private NpcController Controller;
        public Spaceball(NpcController controller)
        {
            Controller = controller;
        }

        public void Tick()
        {
            Active();
        }

        public void Active()
        {
            
        }

        public void Inactive()
        {
            throw new NotImplementedException();
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
        }
    }
}
