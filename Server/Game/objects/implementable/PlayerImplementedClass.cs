using Server.Game.objects.entities;

namespace Server.Game.objects.implementable
{
    abstract class PlayerImplementedClass
    {
        protected Player Player { get; }
        
        protected PlayerImplementedClass(Player player)
        {
            Player = player;
        }
    }
}