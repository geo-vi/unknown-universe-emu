using Server.Game.objects.enums;

namespace Server.Game.objects.entities.players
{
    class Ability
    {
        public ShipAbilities Type { get; set; }
        
        public int Duration { get; set; }

    }
}