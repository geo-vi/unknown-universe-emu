using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.events;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.controllers
{
    class EventController
    {
        public PlayerEvent Event { get; set; }
        private IEvent CurrentEvent { get; set; }

        public EventController(PlayerEvent gameEvent)
        {
            Event = gameEvent;
        }

        public void Set()
        {
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        public void Finish()
        {
            
        }
    }
}
