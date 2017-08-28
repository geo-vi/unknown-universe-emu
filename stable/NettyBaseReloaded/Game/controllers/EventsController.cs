using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers
{
    class EventsController
    {
        public enum Types
        {
            SPACEBALL,
            PUZZLE    
        }

        interface IEvent
        {
            void Start();
            void Update();
            void Finish();
        }

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

        class Spaceball : IEvent
        {
            public Spaceball()
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

        class Puzzle : IEvent
        {
            public Puzzle()
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
}
