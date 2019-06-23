using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class State : PlayerBaseClass
    {
        /// <summary>
        /// If player is in equipment area
        /// Usually near a station / CBS
        /// </summary>
        private bool inEquipmentArea = false;
        public bool InEquipmentArea
        {
            get => inEquipmentArea;
            set
            {
                inEquipmentArea = value;
                UpdateEquipmentArea();
            }
        }

        /// <summary>
        /// If player is in a trading area
        /// Usually near a station
        /// </summary>
        private bool inTradeArea;
        public bool InTradeArea
        {
            get => inTradeArea;
            set
            {
                inTradeArea = value;
                UpdateBeacon();
            }
        }

        /// <summary>
        /// If player is in demi / non attack zone
        /// Usually near portal / station
        /// </summary>
        private bool inDemiZone;
        public bool InDemiZone
        {
            get => inDemiZone;
            set
            {
                inDemiZone = value;
                UpdateBeacon();
            }
        }

        /// <summary>
        /// If player is in radiation area
        /// Out of map bounderies
        /// </summary>
        private bool inRadiationArea;
        public bool InRadiationArea
        {
            get => inRadiationArea;
            set
            {
                inRadiationArea = value;
                UpdateBeacon();
            }
        }
        public DateTime RadiationEntryTime = new DateTime();

        /// <summary>
        /// If player is in portal area
        /// </summary>
        private bool inPortalArea;
        public bool InPortalArea
        {
            get => inPortalArea;
            set
            {
                inPortalArea = value;
                UpdateBeacon();
            }
        }

        /// <summary>
        /// If player is in instant repair zone
        /// Usually near a station
        /// </summary>
        public bool InInstaRepairZone { get; set; }

        /// <summary>
        /// If group is initialized it should be true
        /// </summary>
        public bool GroupInitialized { get; set; }

        public bool Jumping;

        public bool WaitingForEquipmentRefresh;

        public bool CollectingLoot;

        public bool LoginProtection;

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
            if (InRadiationArea != inPlayArea)
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

        public void UpdateBeacon()
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                Packet.Builder.BeaconCommand(session);
            }
        }

        public void UpdateEquipmentArea()
        {
            var session = Player.GetGameSession();
            if (session != null)
            {
                if (WaitingForEquipmentRefresh)
                {
                    Player.Equipment.Reload();
                    Player.Refresh();
                    WaitingForEquipmentRefresh = false;
                }
                Packet.Builder.EquipReadyCommand(session, InEquipmentArea);
            }
        }

        public void StartLoginProtection()
        {
            Player.Controller.Effects.SetInvincible(15000, true);
            Player.State.LoginProtection = true;
        }


        public void EndLoginProtection()
        {
            if (LoginProtection)
            {
                var cooldown =
                    Player.Cooldowns.CooldownDictionary.FirstOrDefault(x => x.Value is InvincibilityCooldown);
                cooldown.Value.EndTime = DateTime.Now;
            }
        }
    }
}
