using System;

namespace Server.Game.objects.entities.players
{
    class JumpSequence
    {
        public Spacemap From { get; set; }

        public Spacemap To { get; set; }

        public DateTime EstimatedTimeOfJump { get; set; }

        public event EventHandler Jumped;

        public JumpSequence(Spacemap from, Spacemap to, DateTime estTimeOfJump)
        {
            From = from;
            To = to;
            EstimatedTimeOfJump = estTimeOfJump;
            
        }

        protected virtual void OnJumped()
        {
            Jumped?.Invoke(this, EventArgs.Empty);
        }
    }
}