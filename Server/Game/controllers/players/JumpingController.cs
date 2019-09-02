using Server.Game.objects;
using Server.Game.objects.entities.players;

namespace Server.Game.controllers.players
{
    class JumpingController : PlayerSubController
    {
        public JumpSequence Sequence { get; private set; }
        
        private bool InJumpProcess = false;
        
        public void Execute(Spacemap to, int timeToJump)
        {
            
        }

        public void EscapeJumpingLoop()
        {
            if (InJumpProcess) return;
        }

        private void ProcessMapMove()
        {
            
        }
    }
}
