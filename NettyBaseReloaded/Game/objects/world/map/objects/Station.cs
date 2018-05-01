using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Station : Object, IClickable
    {
        public List<StationModule> Modules { get; }

        public Faction Faction { get; }

        public Station(int id, List<StationModule> modules, Faction faction, Vector position, Spacemap map) : base(id, position, map)
        {
            Modules = modules;
            Faction = faction;
        }

        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
        }

        public string TypeOfStation()
        {
            switch (Faction)
            {
                case Faction.MMO:
                    return "redStation";
                case Faction.EIC:
                    return "blueStation";
                case Faction.VRU:
                    return "greenStation";
                default:
                    return "";
            }
        }

        public string GetString()
        {
            if (this is PirateStation) return "0|s|" + Id + "|1|pirateStation|6|1500|" + Position.X + "|" + Position.Y;
            if (this is HealthStation) return "0|s|" + Id + "|1|healthStation|4|1500|" + Position.X + "|" + Position.Y;
            if (this is ReadyRelayStation) return "0|s|" + Id + "|1|relayStation|5|1500|" + Position.X + "|" + Position.Y;

            return "0|s|" + Modules[0].Id + "|1|" + TypeOfStation() + "|" + (int)Faction + "|1500|" + Position.X + "|" + Position.Y;
        }
    }
}
