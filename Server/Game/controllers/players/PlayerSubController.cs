using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.players
{
    abstract class PlayerSubController : AbstractedSubController
    {
        public Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }
    }
}