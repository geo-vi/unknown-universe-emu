using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;

namespace NettyBaseReloaded.Game.controllers.pet
{
    abstract class IGear : IChecker
    {
        public PetController baseController { get; }

        public IGear(PetController controller)
        {
            baseController = controller;
        }

        public abstract void Activate();

        public abstract void Check();
    }
}
