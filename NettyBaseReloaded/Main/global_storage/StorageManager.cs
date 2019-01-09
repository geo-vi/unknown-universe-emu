using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Main.global_storage
{
    class StorageManager
    {
        public Dictionary<int, Clan> Clans = new Dictionary<int, Clan>();

        public Dictionary<int, ClanDiplomacy> ClanDiplomacys = new Dictionary<int, ClanDiplomacy>();

        public int ServerId => 1; // Global

        public Clan GetClan(int id)
        {
            if (Clans.ContainsKey(id))
                return Clans[id];
            return Global.QueryManager.GetClan(id);
        }

        public Clan GetClan(string tag)
        {
            return Clans.Values.FirstOrDefault(x => x.Tag == tag);
        }
    }
}