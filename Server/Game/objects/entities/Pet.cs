using Server.Game.objects.enums;
using Server.Main.objects;

namespace Server.Game.objects.entities
{
    class Pet : Character
    {
        protected Pet(int id, string name, Hangar hangar, Factions factionId, Clan clan = null) : base(id, name, hangar, factionId, clan)
        {
        }
    }
}
