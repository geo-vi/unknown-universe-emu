using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class KamikazeGear : Gear
    {
        public KamikazeGear(PetController controller, bool enabled, int level, int amount = 1) : base(controller, enabled, level, amount)
        {
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override void Check()
        {
            throw new NotImplementedException();
        }

        public override void End(bool shutdown = false)
        {
            throw new NotImplementedException();
        }
    }
}
