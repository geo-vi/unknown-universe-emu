using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.controllers.server;
using Server.Game.objects.entities;
using Server.Main;
using Server.Main.objects;

namespace Server.Game.controllers
{
    abstract class AbstractCharacterController : ITick, IDisposable
    {              
        private ConcurrentBag<AbstractedSubController> _abstractedSubControllers = new ConcurrentBag<AbstractedSubController>();
        
        protected readonly Character Character;

        public int TickId { get; set; }

        protected AbstractCharacterController(Character character)
        {
            Character = character;
        }

        public void Initiate()
        {
            CreateControllers();
            CreateTickInstance();
            OnInitFinish();
        }

        protected virtual void CreateControllers()
        {
            CreateControlledInstance<ConfigurationController>();
            CreateControlledInstance<CharacterRangeController>();
            CreateControlledInstance<RocketLauncherController>();
            CreateControlledInstance<CharacterSelectionController>();
            CreateControlledInstance<ShipController>();
            CreateControlledInstance<ShipEffectController>();
            CreateControlledInstance<StateController>();
            CreateControlledInstance<CharacterMovementController>();
            CreateControlledInstance<CharacterCombatController>();
            CreateControlledInstance<CharacterDamageController>();
        }

        private void RemoveControllers()
        {
            foreach (var controller in _abstractedSubControllers)
            {
                controller.OnRemoved();
            }
            _abstractedSubControllers.Clear();
        }
        
        private void CreateTickInstance()
        {
            Global.TickManager.Add(this, out var tickId);
            TickId = tickId;
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
        /// Creating a controlled instance (for characters)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void CreateControlledInstance<T>() where T: new()
        {
            var instance = new T();
            
            var controller = instance as AbstractedSubController;
            if (controller == null) return;
            
            controller.Character = Character;
            controller.OnAdded();
            
            _abstractedSubControllers.Add(controller);
        }
        
        /// <summary>
        /// Overrides the controlled instance
        /// </summary>
        /// <typeparam name="T">Old controlled instance</typeparam>
        /// <typeparam name="T2">Overriding instance</typeparam>
        protected void OverrideControlledInstance<T,T2>()   where T : AbstractedSubController
                                                            where T2 : new()
        {
            var oldController = _abstractedSubControllers.FirstOrDefault(x => x is T);
            if (typeof(T2) == typeof(T) || oldController == null)
            {
                // nothing to override 
                return;
            }
            
            var instance = new T2();
            
            var controller = instance as AbstractedSubController;
            if (controller == null) return;
            
            controller.Character = Character;
            
            oldController.OnOverwritten();

            _abstractedSubControllers =
                new ConcurrentBag<AbstractedSubController>(_abstractedSubControllers.Except(new[] { oldController }))
                {
                    controller
                };

            controller.OnAdded();
        }

        /// <summary>
        /// Getting and returning the controller instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInstance<T>() where T : AbstractedSubController
        {
            var instance = _abstractedSubControllers.FirstOrDefault(x => x is T);
            return instance as T;
        }

        /// <summary>
        /// Ticking
        /// </summary>
        public void TickControllers()
        {
            foreach (var controller in _abstractedSubControllers)
            {
                controller.OnTick();
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
            Global.TickManager.Remove(this);
            ServerController.Get<MapController>().RemoveCharacterFromMap(Character);
            RemoveControllers();
        }
    }
}
