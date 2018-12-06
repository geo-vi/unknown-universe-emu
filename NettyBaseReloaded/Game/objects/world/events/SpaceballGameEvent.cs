using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.events
{
    class SpaceballGameEvent : GameEvent
    {
        public SpaceballGameEvent(int id, string name, EventTypes eventType, bool active) : base(id, name, eventType, active)
        {
        }

        public override void Start()
        {
            World.StorageManager.Spacemaps[16].CreateSpaceball(443, Vector.GetMiddle(World.StorageManager.Spacemaps[16]));
            base.Start();
        }
    }
}
