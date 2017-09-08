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

        /// <summary>
        /// If player is in portal area
        /// </summary>
        public bool InPortalArea { get; set; }

        /// <summary>
        /// If player is in instant repair zone
        /// Usually near a station
        /// </summary>
        public bool InInstaRepairZone { get; set; }

        public State(Player player) : base(player)
        {
            
        }
        
        private Tuple<int, Faction>[] HomeMapIds =
        {
            new Tuple<int, Faction>(0, Faction.NONE), 
            new Tuple<int, Faction>(1, Faction.MMO), new Tuple<int, Faction>(2, Faction.MMO),
            new Tuple<int, Faction>(3, Faction.MMO), new Tuple<int, Faction>(4, Faction.MMO),
            new Tuple<int, Faction>(5, Faction.EIC), new Tuple<int, Faction>(6, Faction.EIC),
            new Tuple<int, Faction>(7, Faction.EIC), new Tuple<int, Faction>(8, Faction.EIC),
            new Tuple<int, Faction>(9, Faction.VRU), new Tuple<int, Faction>(10, Faction.VRU),
            new Tuple<int, Faction>(11, Faction.VRU), new Tuple<int, Faction>(12, Faction.VRU)
        };

        public bool IsOnHomeMap()
        {
            if (HomeMapIds[Player.Spacemap.Id]?.Item2 == Player.FactionId)
            {
                return true;
            }
            return false;
        }
    }
}
