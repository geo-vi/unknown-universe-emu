using System.Collections.Generic;
using Server.Game.controllers.implementable;
using Server.Game.controllers.server;
using Server.Game.objects;

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

        private Dictionary<string, ServerImplementedController> ServerImplementedControllers;

        public ServerController()
        {
            CreateControllers();
        }

        public void CreateControllers()
        {
            ServerImplementedControllers = new Dictionary<string, ServerImplementedController>();
            ServerImplementedControllers.Add("attackController", new AttackController());
            ServerImplementedControllers.Add("rangeController", new RangeController());
            ServerImplementedControllers.Add("movementController", new MovementController());
            ServerImplementedControllers.Add("spawnController", new SpawnController());
            ServerImplementedControllers.Add("healingController", new HealingController());
            ServerImplementedControllers.Add("explosivesController", new ExplosivesController());
            ServerImplementedControllers.Add("effectsControllers", new EffectsController());
            ServerImplementedControllers.Add("destructionController", new DestructionController());
        }

        public void CreateMapController(Spacemap spacemap)
        {
            var name = "map" + spacemap.Id + "Controller";
            ServerImplementedControllers.Add(name, new MapController(spacemap));
        }

        public ServerImplementedController GetController(string key)
        {
            return ServerImplementedControllers[key];
        }
    }
}