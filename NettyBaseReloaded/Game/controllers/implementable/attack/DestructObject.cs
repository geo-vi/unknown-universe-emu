using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.killscreen;

namespace NettyBaseReloaded.Game.controllers.implementable.attack
{
    class DestructObject
    {
        public Character Target;

        public DeathType DeathType = DeathType.MISC;
    }
}
