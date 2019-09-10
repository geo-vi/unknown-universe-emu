using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets.triggered.stations
{
    class BaseStation : Station
    {
        public BaseStation(int id, Vector pos, Spacemap map, Factions faction) : base(id, pos, map, faction)
        {
        }
    }
}