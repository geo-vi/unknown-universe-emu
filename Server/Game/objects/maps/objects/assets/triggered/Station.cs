using System;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.maps.objects.assets.triggered
{
    /// <summary>
    /// It's sort of asset however on old version it's not recognised as asset unlike on the new one :??????
    /// > noone cares anyways since we're creating our own client and new client is abandoned
    /// </summary>
    abstract class Station : GameObject, ITriggerable
    {
        public event EventHandler OnTriggered;

        public Factions Faction { get; }
        
        public Station(int id, Vector pos, Spacemap map, Factions faction) : base(id, pos, map, 2000)
        {
            Faction = faction;
        }

        public void Trigger()
        {
            OnTriggered?.Invoke(this, EventArgs.Empty);
        }
    }
}
