using System.Collections.Generic;

namespace NettyBaseReloaded.Main.objects
{
    class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

        public int RankPoints { get; set; }

        // TODO: Add owned BattleStations

        /// <summary>
        /// Clan id, diplomacy relations
        /// </summary>
        public Dictionary<int,Diplomacy> Diplomacy { get; set; }

        public Dictionary<int, ClanMember> Members { get; set; }

        public Clan(int id, string name, string tag, int rankPoints)
        {
            Id = id;
            Name = name;
            Tag = tag;
            Diplomacy = new Dictionary<int, Diplomacy>();
            Members = new Dictionary<int, ClanMember>();
            RankPoints = rankPoints;
        }
        
        public short GetRelation(Clan clan)
        {
            if (clan == this && clan.Id != 0)
                return (short)objects.Diplomacy.ALLIED;
            if (Diplomacy.ContainsKey(clan.Id))
                return (short)Diplomacy[clan.Id];
            return (short) objects.Diplomacy.NONE;
        }
    }
}
