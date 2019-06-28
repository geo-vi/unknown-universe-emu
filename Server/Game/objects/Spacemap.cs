using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Policy;
using Server.Game.controllers.server;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;

namespace Server.Game.objects
{
    class Spacemap
    {
// TODO: Define all the zones in a class
        // DemiZone / PoiZone

        /**********
         * BASICS *
         **********/
        public int Id { get; }
        private int TickId { get; set; }
        public string Name { get; }
        public Factions Faction { get; }
        public bool Pvp { get; }
        public bool Starter { get; }
        public Vector[] Limits { get; private set; }
        public int Level { get; }
        public bool Disabled { get; set; }
        public bool RangeDisabled { get; set; }

        #region Base Storages [DB]
        private List<PortalBase> PortalBase { get; set; }
        private List<BaseNpc> Npcs;
        #endregion

        public Dictionary<int, Zone> Zones = new Dictionary<int, Zone>();

        public ConcurrentDictionary<int, Object> Objects = new ConcurrentDictionary<int, Object>();

        public ConcurrentDictionary<string, Object> HashedObjects = new ConcurrentDictionary<string, Object>();

        public Dictionary<string, POI> POIs = new Dictionary<string, POI>();

        //Used to store all the entities of the map
        public ConcurrentDictionary<int, Character> Entities = new ConcurrentDictionary<int, Character>();

        public Dictionary<int, Spacemap> VirtualWorlds = new Dictionary<int, Spacemap>()
        {
            {0, null} // Adding virtual worlds after id = 0 since it might cause bugs
        };

        public Spacemap(int id, string name, Factions faction, bool pvp, bool starter, int level, List<BaseNpc> npcs, List<PortalBase> portals)
        {
            Id = id;
            Name = name;
            Faction = faction;
            Pvp = pvp;
            Starter = starter;
            Level = level;
            PortalBase = portals;
            Npcs = npcs;

            var tickId = -1;
            TickId = tickId;
        }
    }
}
