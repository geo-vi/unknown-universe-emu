using Server.Game.managers;

namespace Server.Game
{
    class World
    {        
        public static void InitiateManagers()
        {
            //todo
            GameStorageManager.Instance.Initiate();
        }
    }
}
