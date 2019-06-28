
using Server.Game.objects.implementable;

namespace Server.Game.objects.entities.npcs
{
    class RoutePoint
    {
        public int Id { get; }
        
        public Vector From { get; }
        
        public Vector To { get; }
        
        public int DelayToNext { get; }

        public RoutePoint(int id, Vector from, Vector to, int delayToNext)
        {
            Id = id;
            From = from;
            To = to;
            DelayToNext = delayToNext;
        }
    }
}