﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers.pet.gears
{
    class Guard : Gear
    {
        internal Guard(PetController controller) : base(controller) { }

        public override void Activate()
        {

        }

        public override void Check()
        {
            Follow(baseController.Pet.GetOwner());
        }
    }
}
