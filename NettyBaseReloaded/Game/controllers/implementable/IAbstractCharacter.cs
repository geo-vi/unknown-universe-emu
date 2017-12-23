using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    abstract class IAbstractCharacter
    {
        public AbstractCharacterController Controller { get; }

        internal Character Character => Controller.Character;

        public IAbstractCharacter(AbstractCharacterController controller)
        {
            Controller = controller;
        }

        public abstract void Tick();

        public abstract void Stop();
    }
}
