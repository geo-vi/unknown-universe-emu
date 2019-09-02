using System.Collections.Generic;
using Server.Game.objects.enums;

namespace Server.Main.objects
{
    class Clan
    {
        /// <summary>
        /// Clan's id
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Clan's name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Clan's tag
        /// </summary>
        public string Tag { get; }
        
        /// <summary>
        /// Clan's faction
        /// </summary>
        public Factions Faction { get; }
        
        /// <summary>
        /// Clan's news
        /// </summary>
        public string News { get; }
        
        /// <summary>
        /// Members Ids assigned
        /// </summary>
        public List<int> AssignedMemberIds { get; set; }
        
        public Clan(int id, string name, string tag, Factions faction, string news)
        {
            Id = id;
            Name = name;
            Tag = tag;
            Faction = faction;
            News = news;
            AssignedMemberIds = new List<int>();
        }

        public ClanRelationships Compare(Clan targetShipClan)
        {
            if (targetShipClan == this)
            {
                return ClanRelationships.ALLIED;
            }

            return ClanRelationships.NONE;
        }
    }
}
