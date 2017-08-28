using System;
using NettyBaseReloaded.Game.netty;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class PortalBase
    {
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int newX { get; set; }
        public int newY { get; set; }
        public int Map { get; set; }

        public PortalBase(int Id, int x, int y, int newX, int newY, int Map)
        {
            this.Id = Id;
            this.x = x;
            this.y = y;
            this.newX = newX;
            this.newY = newY;
            this.Map = Map;
        }
    }

    class Jumpgate : Object, IClickable
    {
        public Faction Faction { get; set; }

        public int LevelRequired { get; set; }

        public Vector Destination { get; set; }

        public int DestinationMapId { get; set; }

        public bool Visible { get; set; }
        
        public int FactionScrap { get; set; }
        
        public int RequiredLevel { get; set; }

        public int Gfx { get; set; }

        public bool Working { get; set; }

        public Jumpgate(int id, Faction faction, Vector pos, Vector destinationPos, int destinationMapId, bool visible, int factionScrap, int requiredLevel, int gfx) : base(id, pos)
        {
            Faction = faction;
            Destination = destinationPos;
            DestinationMapId = destinationMapId;
            Visible = visible;
            FactionScrap = factionScrap;
            RequiredLevel = requiredLevel;
            Gfx = gfx;
            Working = true;
        }
       
        public override void execute(Character character)
        {
        }

        public void click(Character character)
        {
            var player = character as Player;
            player?.Controller.Miscs.Jump(DestinationMapId, Destination, Id);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToPacket()
        {
            return "0|p|" + Id + "|" + (int)Faction + "|" + Gfx + "|" + Position.X + "|" + Position.Y + "|" + Convert.ToInt32(Visible) + "|" + FactionScrap;
        }
    }
}
