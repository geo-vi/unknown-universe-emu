using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class Station : Object, IClickable
    {
        public List<int> AssignedIds { get; }

        public Faction Faction { get; }

        public Vector Position { get; }

        public Station(int id, List<int> assignedIds, Faction faction, Vector position) : base(id, position)
        {
            AssignedIds = assignedIds;
            Faction = faction;
            Position = position;
        }

        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
            throw new System.NotImplementedException();
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
            return "0|s|" + AssignedIds[0] + "|1|" + GetType() + "|" + (int)Faction + "|1500|" + Position.X + "|" + Position.Y;
        }

        public List<Asset> ToAssets(Spacemap spacemap)
        {
            var assets = new List<Asset>();
            assets.Add(new Asset(AssignedIds[0], "", AssetTypeModule.HQ, (int)Faction, "", AssignedIds[0], 0, 0, Position, 0, false, false, false));
            assets.Add(new Asset(AssignedIds[1], "", AssetTypeModule.HANGAR, (int)Faction, "", AssignedIds[1], 0, 0, new Vector(Position.X + 1000, Position.Y), 0, false, false, false));
            assets.Add(new Asset(AssignedIds[2], "", AssetTypeModule.ORE_TRADE, (int)Faction, "", AssignedIds[2], 0, 0, new Vector(Position.X, Position.Y + 1000), 0, false, false, false));
            assets.Add(new Asset(AssignedIds[3], "", AssetTypeModule.REPAIR_DOCK, (int)Faction, "", AssignedIds[3], 0, 0, new Vector(Position.X, Position.Y - 1000), 0, false, false, false));
            return assets;
        }
    }
}
