using System;

namespace Server.Game.objects.entities.players
{
    class JumpSequence
    {
        public Spacemap From;

        public Spacemap To;

        public DateTime EstimatedTimeOfJump;

        public event EventHandler Jumped;

        public JumpSequence(Spacemap from, Spacemap to, DateTime estTimeOfJump)
        {
            From = from;
            To = to;
            EstimatedTimeOfJump = estTimeOfJump;
        }
    }
}