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

        private Dictionary<Clan,Diplomacy> Diplomacy { get; set; }

        public Clan(int id, string name, string tag, int rankPoints)
        {
            Id = id;
            Name = name;
            Tag = tag;
            Diplomacy = new Dictionary<Clan, Diplomacy>();
            RankPoints = rankPoints;
        }

        public void LoadDiplomacy()
        {
            // TEMP
            //if (Tag == "DEV")
            //{
            //    Diplomacy.Add(Global.StorageManager.Clans[1], objects.Diplomacy.AT_WAR);
            //}
            //else if (Tag == "ADM")
            //{
            //    Diplomacy.Add(Global.StorageManager.Clans[2], objects.Diplomacy.AT_WAR);
            //}
        }

        public short GetRelation(Clan clan)
        {
            if (clan == this && clan.Id != 0)
                return (short)objects.Diplomacy.ALLIED;
            if (Diplomacy.ContainsKey(clan))
                return (short)Diplomacy[clan];
            return (short) objects.Diplomacy.NONE;
        }
    }
}
