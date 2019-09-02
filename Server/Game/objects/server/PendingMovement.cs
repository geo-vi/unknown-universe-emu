using Server.Game.objects.entities;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class PendingMovement
    {
        public Character ParentCharacter;

        public Vector StartPosition;

        public Vector EndPosition;

        public bool MovementRendered;
        
        public PendingMovement(Character parent, Vector start, Vector end)
        {
            ParentCharacter = parent;
            StartPosition = start;
            EndPosition = end;
        }
    }
}