using Server.Game.objects.entities;

namespace Server.Game.controllers
{
    class PlayerController : AbstractCharacterController
    {
        private Player Player;

        public PlayerController(Player player) : base(player)
        {
            Player = player;
        }
    }
}
