using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets.triggered.stations
{
    class PirateStation : Station
    {
        public PirateStation(int id, Vector pos, Spacemap map) : base(id, pos, map, Factions.NONE)
        {
        }
    }
}
