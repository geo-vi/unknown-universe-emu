using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.events;

namespace NettyBaseReloaded.Game.controllers
{
    class EventsController
    {
        private IEvent CurrentEvent { get; set; }

        public EventsController()
        {
        }

        public void Set(Types types)
        {
            switch (types)
            {
                case Types.SPACEBALL:
                    CurrentEvent = new Spaceball();
                    break;
                case Types.PUZZLE:
                    CurrentEvent = new Puzzle();
                    break;
            }
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
