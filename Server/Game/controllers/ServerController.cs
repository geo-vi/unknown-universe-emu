using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.controllers.server;
using Server.Game.objects;
using Server.Game.objects.server;
using RangeController = Server.Game.controllers.server.RangeController;

namespace Server.Game.controllers
{
    class ServerController
    {
        private static ServerController _instance;

        public static ServerController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServerController();
                return _instance;
            }
        }
        
        public static T Get<T>() where T : ServerImplementedController
        {
            return Instance.GetInstance<T>();
        }

        private ConcurrentBag<ServerImplementedController> _abstractedSubControllers =
            new ConcurrentBag<ServerImplementedController>();

        private void CreateControlledInstance<T>() where T : new()
        {
            var instance = new T();

            var controller = instance as ServerImplementedController;
            if (controller == null) return;

            controller.Initiate();

            _abstractedSubControllers.Add(controller);
        }

        public void CreateInstances()
        {
            CreateControlledInstance<AttackController>();
            CreateControlledInstance<DamageController>();
            CreateControlledInstance<RangeController>();
            CreateControlledInstance<MovementController>();
            CreateControlledInstance<SpawnController>();
            CreateControlledInstance<HealingController>();
            CreateControlledInstance<ExplosivesController>();
            CreateControlledInstance<EffectsController>();
            CreateControlledInstance<DestructionController>();
            CreateControlledInstance<MapController>();
            CreateControlledInstance<CooldownController>();
        }

        public T GetInstance<T>() where T : ServerImplementedController
        {
            var instance = _abstractedSubControllers.FirstOrDefault(x => x is T);
            if (!(instance is T typeInstance))
            {
                throw new Exception(typeof(T) + " - Server Controller not found");
            }
            return typeInstance;
        }
    }
}