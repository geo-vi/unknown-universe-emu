using Server.Game.controllers;
using Server.Game.managers;

namespace Server.Game
{
    class World
    {        
        public static void InitiateManagers()
        {
            //todo
            GameStorageManager.Instance.Initiate();
            ServerController.Instance.CreateInstances();
        }
    }
}
