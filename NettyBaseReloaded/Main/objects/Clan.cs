using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;

namespace NettyBaseReloaded.Main.objects
{
    class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

        public int RankPoints { get; set; }

        public Dictionary<int, ClanBattleStation> OwnedBattleStations = new Dictionary<int, ClanBattleStation>();

        /// <summary>
        /// Clan id, diplomacy relations
        /// </summary>

        public ConcurrentDictionary<int, ClanMember> Members { get; set; }

        public Clan(int id, string name, string tag, int rankPoints)
        {
            Id = id;
            Name = name;
            Tag = tag;
            Members = new ConcurrentDictionary<int, ClanMember>();
            RankPoints = rankPoints;
        }
        
        public short GetRelation(Clan clan)
        {
            var relationship = Global.StorageManager.ClanDiplomacys.Values.FirstOrDefault(x =>
                x.Clans.Exists(z => z == clan) && x.Clans.Exists(z => z == this));
            if (relationship != null)
            {
                return (short) relationship.Diplomacy;
            }

            return 0;
        }
    }
}
