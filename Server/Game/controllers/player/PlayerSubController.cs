using Server.Game.controllers.characters;
using Server.Game.objects.entities;

namespace Server.Game.controllers.player
{
    abstract class PlayerSubController : AbstractedSubController
    {
        protected PlayerSubController(Player player) : base(player)
        {
        }
    }
}