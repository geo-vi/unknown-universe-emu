using Server.Game.objects.entities;
using Server.Networking;

namespace Server.Game.objects
{
    class GameSession
    {
        public GameClient GameClient;

        public Player Player;
        
        public GameSession(GameClient gameClient)
        {
            GameClient = gameClient;
        }
    }
}
