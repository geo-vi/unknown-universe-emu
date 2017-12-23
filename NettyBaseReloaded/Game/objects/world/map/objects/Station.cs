using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Station : Object, IClickable
    {
        public List<StationModule> Modules { get; }

        public Faction Faction { get; }

        public Vector Position { get; }

        public Station(int id, List<StationModule> modules, Faction faction, Vector position) : base(id, position)
        {
            Modules = modules;
            Faction = faction;
            Position = position;
        }

        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
        }

        public string GetType()
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
            return "0|s|" + Modules[0] + "|1|" + GetType() + "|" + (int)Faction + "|1500|" + Position.X + "|" + Position.Y;
        }
    }
}
