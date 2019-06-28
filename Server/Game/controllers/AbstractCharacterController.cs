using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.objects.entities;

namespace Server.Game.controllers
{
    abstract class AbstractCharacterController : IDisposable
    {              
        protected ConcurrentBag<AbstractedSubController> AbstractedSubControllers = new ConcurrentBag<AbstractedSubController>();
        
        protected Character Character;
   
        protected AbstractCharacterController(Character character)
        {
            Character = character;
        }

        public void Initiate()
        {
            OnInitFinish();
        }

        /// <summary>
        /// Calling after all global stuff have finished
        /// </summary>
        public virtual void OnInitFinish()
        {
            
        }
        
        public void Tick()
        {
            TickControllers();
            OnTickFinished();
        }

        /// <summary>
        /// Ticking
        /// </summary>
        public void TickControllers()
        {
            foreach (var controller in AbstractedSubControllers)
            {
            }
        }

        /// <summary>
        /// Controller exit point
        /// </summary>
        public virtual void OnTickFinished()
        {
        
        }

        /// <summary>
        /// Ending everything here :D
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}
