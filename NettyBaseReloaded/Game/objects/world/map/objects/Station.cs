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

        public List<Asset> ToAssets(Spacemap spacemap)
        {
            var assets = new List<Asset>();
            assets.Add(new Asset(Modules[0].Id, "", AssetTypeModule.HQ, (int)Faction, "", Modules[0].Id, 0, 0, Position, 0, false, false, false));
            assets.Add(new Asset(Modules[1].Id, "", AssetTypeModule.HANGAR, (int)Faction, "", Modules[1].Id, 0, 0, new Vector(Position.X + 1000, Position.Y), 0, false, false, false));
            assets.Add(new Asset(Modules[2].Id, "", AssetTypeModule.ORE_TRADE, (int)Faction, "", Modules[2].Id, 0, 0, new Vector(Position.X, Position.Y + 1000), 0, false, false, false));
            assets.Add(new Asset(Modules[3].Id, "", AssetTypeModule.REPAIR_DOCK, (int)Faction, "", Modules[3].Id, 0, 0, new Vector(Position.X, Position.Y - 1000), 0, false, false, false));
            return assets;
        }
    }
}
