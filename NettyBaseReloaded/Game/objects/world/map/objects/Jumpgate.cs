using System;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.objects.jumpgates;
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

        public int DestinationVirtualWorldId { get; set; }

        public bool Visible { get; set; }
        
        public int FactionScrap { get; set; }
        
        public int RequiredLevel { get; set; }

        public int Gfx { get; set; }

        public bool Working { get; set; }

        public Player Owner { get; set; }

        public string DisabledMessage { get; set; }

        public Jumpgate(int id, Faction faction, Vector pos, Spacemap map, Vector destinationPos, int destinationMapId, bool visible, int factionScrap, int requiredLevel, int gfx) : base(id, pos, map)
        {
            Faction = faction;
            Destination = destinationPos;
            DestinationMapId = destinationMapId;
            Visible = visible;
            FactionScrap = factionScrap;
            RequiredLevel = requiredLevel;
            Gfx = gfx;
            Working = true;
            DestinationVirtualWorldId = 0;
        }
       
        public override void execute(Character character)
        {
        }

        public virtual void click(Character character)
        {
            //todo: level required
            var player = character as Player;
            if (player == null) return;
            if (!Working && DisabledMessage != "")
            {
                Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|" + DisabledMessage);
                return;
            }
            player.Controller.Miscs.Jump(DestinationMapId, Destination, Id, DestinationVirtualWorldId);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool GetVisibility(Player player)
        {
            if (this is PirateGate gate)
            {
                if (player.FactionId == Faction && !gate.Broken) return true;
                return false;
            }
            return Visible;
        }

        public string ToPacket(Player player)
        {//1,1 - last param
            return "0|p|" + Id + "|" + (int)Faction + "|" + Gfx + "|" + Position.X + "|" + Position.Y + "|" + Convert.ToInt32(GetVisibility(player)) + "|" + "0";
        }

        public void Disable(string disabledMsg = "")
        {
            Working = false;
            DisabledMessage = disabledMsg;
        }
    }
}
