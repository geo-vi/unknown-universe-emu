using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class State : PlayerBaseClass
    {
        /// <summary>
        /// If player is in equipment area
        /// Usually near a station / CBS
        /// </summary>
        public bool InEquipmentArea { get; set; }

        /// <summary>
        /// If player is in a trading area
        /// Usually near a station
        /// </summary>
        public bool InTradeArea { get; set; }

        /// <summary>
        /// If player is in demi / non attack zone
        /// Usually near portal / station
        /// </summary>
        public bool InDemiZone { get; set; }

        /// <summary>
        /// If player is in radiation area
        /// Out of map bounderies
        /// </summary>
        public bool InRadiationArea { get; set; }
        public DateTime RadiationEntryTime = new DateTime();

        /// <summary>
        /// If player is in portal area
        /// </summary>
        public bool InPortalArea { get; set; }

        /// <summary>
        /// If player is in instant repair zone
        /// Usually near a station
        /// </summary>
        public bool InInstaRepairZone { get; set; }

        /// <summary>
        /// If group is initialized it should be true
        /// </summary>
        public bool GroupInitialized { get; set; }

        public State(Player player) : base(player)
        {
            AddHomeMaps();
        }

        public void Tick()
        {
            RadiationMonitor();
        }
        
        private void RadiationMonitor()
        {
            var inPlayArea = Player.Spacemap.InNonPlayArea(Player.Position);
            if (!InRadiationArea && inPlayArea)
            {
                RadiationEntryTime = DateTime.Now;
            }
            InRadiationArea = inPlayArea;
        }

        private Dictionary<int, Faction> HomeMapIds = new Dictionary<int, Faction>();

        private void AddHomeMaps()
        {
            HomeMapIds.Add(0, Faction.NONE);
            HomeMapIds.Add(1, Faction.MMO);
            HomeMapIds.Add(2, Faction.MMO);
            HomeMapIds.Add(3, Faction.MMO);
            HomeMapIds.Add(4, Faction.MMO);
            HomeMapIds.Add(5, Faction.EIC);
            HomeMapIds.Add(6, Faction.EIC);
            HomeMapIds.Add(7, Faction.EIC);
            HomeMapIds.Add(8, Faction.EIC);
            HomeMapIds.Add(9, Faction.VRU);
            HomeMapIds.Add(10, Faction.VRU);
            HomeMapIds.Add(11, Faction.VRU);
            HomeMapIds.Add(12, Faction.VRU);
        }

        public bool IsOnHomeMap()
        {
            if (HomeMapIds.ContainsKey(Player.Spacemap.Id))
            {
                var mapFaction = HomeMapIds[Player.Spacemap.Id];
                if (Player.FactionId == mapFaction) return true;
            }
            return false;
        }

        public void Reset()
        {
            InDemiZone = false;
            InTradeArea = false;
            InRadiationArea = false;
            InEquipmentArea = false;
            InInstaRepairZone = false;
            InPortalArea = false;
            RadiationEntryTime = DateTime.Now;
        }
    }
}
