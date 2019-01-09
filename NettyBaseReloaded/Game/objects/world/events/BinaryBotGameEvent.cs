using NettyBaseReloaded.Game.objects.world.npcs;
using NettyBaseReloaded.Main.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.events
{
    class BinaryBotGameEvent : GameEvent
    {
        private int TickId;

        private EventNpc Santa;

        public BinaryBotGameEvent(int id, string name, EventTypes eventType, bool active) : base(id, name, eventType, active)
        {
        }

        public override void Start()
        {
            var randomMap = RandomInstance.getInstance(this);
            var map = World.StorageManager.Spacemaps[randomMap.Next(1, 29)];
            map.CreateBinaryBot(Vector.Random(map), true);
        }
    }
}
