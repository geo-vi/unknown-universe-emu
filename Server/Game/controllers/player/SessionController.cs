using Server.Game.objects.entities;

namespace Server.Game.controllers.player
{
    class SessionController : PlayerSubController
    {
        private float ActivityLevel { get; set; }

        public SessionController(Player player) : base(player)
        {
        }
        
        public void CreateSession()
        {
            ActivityLevel = 1;
        }

        public void CalculateActivity()
        {

        }

        public void InactivityKick()
        {

        }

        public void SessionTakeover()
        {

        }

        public void Kill()
        {

        }
    }
}
